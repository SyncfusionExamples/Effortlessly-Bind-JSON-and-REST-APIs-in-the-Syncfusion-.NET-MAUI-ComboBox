using ComboBoxBindingSample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace ComboBoxBindingSample.ViewModel
{
	/// <summary>
	/// ViewModel for loading and managing country data from a JSON file.
	/// </summary>
	public class JsonViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="name">The name of the property that changed.</param>
		void Raise([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		/// <summary>
		/// Gets the collection of countries.
		/// </summary>
		public ObservableCollection<Country> Countries { get; } = new();

		Country selected;

		/// <summary>
		/// Gets or sets the currently selected country.
		/// </summary>
		public Country Selected
		{
			get => selected;
			set { selected = value; Raise(); }
		}

		/// <summary>
		/// Asynchronously loads country data from the "countries.json" file.
		/// </summary>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task LoadAsync()
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync("countries.json");
			using var reader = new StreamReader(stream);
			var json = await reader.ReadToEndAsync();

			var data = JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

			Countries.Clear();
			foreach (var item in data)
				Countries.Add(item);
		}
	}

	/// <summary>
	/// ViewModel for consuming a REST API to manage user data.
	/// </summary>
	public class RestViewModel
	{
		/// <summary>
		/// Gets the collection of users.
		/// </summary>
		public ObservableCollection<UserDto> Users { get; } = new();

		private readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
		private const string BaseUrl = "https://jsonplaceholder.typicode.com/users";

		/// <summary>
		/// Asynchronously loads user data from the REST API.
		/// </summary>
		public async Task LoadAsync(CancellationToken ct = default)
		{
			// Optional: clear first so UI reflects fresh load attempt
			Users.Clear();

			using var req = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
			using var resp = await http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

			resp.EnsureSuccessStatusCode();

			// Read JSON and deserialize with case-insensitive property matching
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			await using var stream = await resp.Content.ReadAsStreamAsync(ct);
			var data = await JsonSerializer.DeserializeAsync<List<UserDto>>(stream, options, ct);

			if (data is null || data.Count == 0)
			{
				// Nothing returned; leave Users empty
				return;
			}

			// Add only valid items, ordering by Name (safe default if null)
			foreach (var item in data
				.Where(item => item is not null)
				.OrderBy(item => item.Name ?? string.Empty))
			{
				item.Name ??= string.Empty; // avoid null in bindings
				Users.Add(item);
			}
			
		}

		/// <summary>
		/// Asynchronously creates a new user via the REST API.
		/// </summary>
		/// <param name="name">The name of the new user.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task CreateAsync(string name)
		{
			var newUser = new UserDto { Name = name };
			var resp = await http.PostAsJsonAsync(BaseUrl, newUser);
			resp.EnsureSuccessStatusCode();

			// JSONPlaceholder returns the created resource with an id
			var created = await resp.Content.ReadFromJsonAsync<UserDto>();
			if (created != null)
			{
				// Add to the top
				Users.Insert(0, created);
			}
		}
	}
}
