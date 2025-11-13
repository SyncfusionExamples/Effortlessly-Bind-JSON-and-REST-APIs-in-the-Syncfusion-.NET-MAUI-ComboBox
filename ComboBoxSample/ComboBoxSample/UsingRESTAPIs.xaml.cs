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
	}

	public class UserDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class RestViewModel
	{
		public ObservableCollection<UserDto> Users { get; } = new();

		public async Task LoadAsync()
		{
			using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
			var url = "https://jsonplaceholder.typicode.com/users";

			var data = await http.GetFromJsonAsync<List<UserDto>>(url);

			Users.Clear();
			foreach (var u in data.OrderBy(u => u.Name))
				Users.Add(u);
		}
	}
}