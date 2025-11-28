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
	
}