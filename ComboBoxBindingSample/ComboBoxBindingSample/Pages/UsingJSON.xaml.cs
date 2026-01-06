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
	/// Represents the page for demonstrating JSON data loading and binding.
	/// </summary>
	public partial class UsingJSON : ContentPage
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="UsingJSON"/> class.
		/// </summary>
		public UsingJSON()
        {			
			InitializeComponent();
        }

		/// <summary>
		/// Handles the click event for the button that navigates back to the main page.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
		private void Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new MainPage());
		}

		/// <summary>
		/// Handles the click event for the button that initiates loading of JSON data.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
		private async void OnLoadClicked(object sender, EventArgs e)
		{
			if (BindingContext is JsonViewModel view)
				await view.LoadAsync();
		}
	}
}