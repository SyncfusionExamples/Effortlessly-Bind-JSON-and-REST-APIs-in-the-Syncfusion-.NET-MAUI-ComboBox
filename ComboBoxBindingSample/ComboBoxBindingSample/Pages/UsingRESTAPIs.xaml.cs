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
	/// <summary>
	/// Represents the page for demonstrating REST API consumption and data binding.
	/// </summary>
	public partial class UsingRESTAPIs : ContentPage
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="UsingRESTAPIs"/> class.
		/// </summary>
		public UsingRESTAPIs()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Handles the click event for the button that initiates loading of user data from the REST API.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
		private async void OnLoadClicked(object sender, EventArgs e)
		{
			if (BindingContext is RestViewModel vm)
				await vm.LoadAsync();
		}

		/// <summary>
		/// Handles the click event for the button that creates a new user via the REST API.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
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