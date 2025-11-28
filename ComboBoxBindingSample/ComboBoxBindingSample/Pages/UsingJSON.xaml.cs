using ComboBoxBindingSample.ViewModel;
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

namespace ComboBoxBindingSample
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
}