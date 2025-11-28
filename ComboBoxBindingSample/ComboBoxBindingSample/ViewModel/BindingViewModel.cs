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
	public class JsonViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		void Raise([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		public ObservableCollection<Country> Countries { get; } = new();

		Country selected;
		public Country Selected
		{
			get => selected;
			set { selected = value; Raise(); }
		}

		public async Task LoadAsync()
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync("countries.json");
			using var reader = new StreamReader(stream);
			var json = await reader.ReadToEndAsync();

			var data = JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

			Countries.Clear();
			foreach (var c in data)
				Countries.Add(c);
		}
	}

	public class RestViewModel
	{
		public ObservableCollection<UserDto> Users { get; } = new();

		private readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
		private const string BaseUrl = "https://jsonplaceholder.typicode.com/users";

		public async Task LoadAsync()
		{
			var data = await http.GetFromJsonAsync<List<UserDto>>(BaseUrl);

			Users.Clear();
			foreach (var u in data.OrderBy(u => u.Name))
				Users.Add(u);
		}

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
