
namespace ComboBoxBindingSample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

		private void Button_Clicked_1(object sender, EventArgs e)
		{
			Navigation.PushAsync(new UsingRESTAPIs());
		}

		private void Button_Clicked_2(object sender, EventArgs e)
		{
			Navigation.PushAsync(new UsingJSON());
		}
	}
}
