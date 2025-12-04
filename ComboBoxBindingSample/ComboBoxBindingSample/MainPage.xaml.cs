
namespace ComboBoxBindingSample
{
	/// <summary>
	/// The main page of the application, providing navigation to different samples.
	/// </summary>
	public partial class MainPage : ContentPage
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
		public MainPage()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Handles the click event for the button that navigates to the REST APIs sample.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
		private void Button_Clicked_1(object sender, EventArgs e)
		{
			Navigation.PushAsync(new UsingRESTAPIs());
		}

		/// <summary>
		/// Handles the click event for the button that navigates to the JSON sample.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event data.</param>
		private void Button_Clicked_2(object sender, EventArgs e)
		{
			Navigation.PushAsync(new UsingJSON());
		}
	}
}
