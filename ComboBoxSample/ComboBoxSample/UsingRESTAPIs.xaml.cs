using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ComboBoxSample
{
    public partial class UsingRESTAPIs : ContentPage
    {
        public UsingRESTAPIs()
        {
            InitializeComponent();
        }

		private async void OnLoadClicked(object sender, EventArgs e)
		{
			if (BindingContext is RestViewModel vm)
				await vm.LoadAsync();
		}

        private async void OnCreateClicked(object sender, EventArgs e)
        {
			if (BindingContext is RestViewModel vm)
			{
				var name = NameEntry?.Text?.Trim();
				if (!string.IsNullOrWhiteSpace(name))
					await vm.CreateAsync(name);
			}
		}
	}

    public class UserDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
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