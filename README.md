Binding the Syncfusion .NET MAUI ComboBox to JSON Files and REST APIs

In this article, you’ll learn how to populate the Syncfusion .NET MAUI SfComboBox from two common real-world data sources: local JSON files and REST APIs. We’ll cover clean MVVM-friendly patterns, best practices for JSON deserialization, configuring ItemsSource and display/value members, and implementing asynchronous loading for a smooth user experience. What you’ll learn:

•	Bind SfComboBox to a local JSON file
•	Bind SfComboBox to a REST API endpoint
•	Recommended patterns and best practices for MVVM and async data flows

Bind SfComboBox to a local JSON file

JSON (JavaScript Object Notation) is a lightweight, text-based data format used for transmitting structured data. It represents data as key–value pairs and arrays, is easy for humans to read/write, and easy for machines to parse; its MIME type is application/json.

Follow Steps 1,2,3 pattern.

Create the JSON file named as countries.json and paste this sample content:

[
  { "Name": "United States", "Code": "US" },
  { "Name": "Canada", "Code": "CA" },
  { "Name": "Germany", "Code": "DE" },
  { "Name": "France", "Code": "FR" },
  { "Name": "United Kingdom", "Code": "GB" },
  { "Name": "India", "Code": "IN" },
  { "Name": "Japan", "Code": "JP" },
  { "Name": "Australia", "Code": "AU" },
  { "Name": "Brazil", "Code": "BR" },
  { "Name": "South Africa", "Code": "ZA" }
]


Xaml

<ContentPage.Content>
<VerticalStackLayout Padding="16"
                        Spacing="12">
    <Button Text="Load JSON"
            Clicked="OnLoadClicked" />
    <inputs:SfComboBox WidthRequest="300"
                        ItemsSource="{Binding Countries}" 
                        DisplayMemberPath="Name"
                        TextMemberPath="Name"
                        SelectedItem="{Binding Selected, Mode=TwoWay}" />
    <Label Text="{Binding Selected.Name, StringFormat='Selected country: {0}'}" />
</VerticalStackLayout>
</ContentPage.Content>



C#

private async void OnLoadClicked(object sender, EventArgs e)
{
     if (BindingContext is JsonViewModel vm)
         await vm.LoadAsync();
}

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class JsonViewModel
{
    public ObservableCollection<Country> Countries { get; } = new();
    public async Task LoadAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("countries.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var data = JsonSerializer.Deserialize<List<Country>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

        Countries.Clear();
        foreach (var c in data)
            Countries.Add(c);
    }
}


Bind SfComboBox to a REST API

A REST (Representational State Transfer) API is an architectural style for building web services that treat data as resources accessible via URLs and manipulated with standard HTTP methods (GET, POST, PUT, PATCH, DELETE). It is stateless, cacheable, and uses a uniform interface, commonly exchanging data in JSON.

C#

private async void OnLoadClicked(object sender, EventArgs e)
{
    if (BindingContext is RestViewModel vm)
        await vm.LoadAsync();
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


Xaml

<VerticalStackLayout Padding="16"
			  Spacing="12">
	<Button Text="Load from API"
			Clicked="OnLoadClicked" />
                    <inputs:SfComboBox WidthRequest="320"
					   ItemsSource="{Binding Users}"
					   DisplayMemberPath="Name"
					   TextMemberPath="Name" />
</VerticalStackLayout>




Get the complete getting started sample from the GitHub link.

Conclusion 

Thanks for reading! In this blog, we’ve seen how to bind the Syncfusion .NET MAUI SfComboBox to both local JSON data and live REST API data using clean, asynchronous patterns. Check out our Release Notes and What’s New pages to see the other updates in this release and leave your feedback in the comments section below. 
For current Syncfusion customers, the newest version of Essential Studio is available from the license and downloads page. If you are not yet a customer, you can try our 30-day free trial to check out these new features. 
For questions, you can contact us through our support forums, feedback portal, or support portal. We are always happy to assist you!
