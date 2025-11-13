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

namespace MauiApp1
{
    public partial class UsingJSON : ContentPage
    {
        public UsingJSON()
        {
            InitializeComponent();
        }

		private void Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new MainPage());
		}

		private async void OnLoadClicked(object sender, EventArgs e)
		{
			if (BindingContext is JsonViewModel vm)
				await vm.LoadAsync();
		}
	}
	public class Country
	{
		public string Name { get; set; }
		public string Code { get; set; }
	}

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
}