# Effortlessly Bind JSON and REST APIs in the Syncfusion .NET MAUI ComboBox

## Introduction
In this blog, you will populate Syncfusion’s .NET MAUI SfComboBox from two real world data sources: a local JSON file and a REST API. It is MVVM friendly, async, and user centric with clean bindings and responsive loading.

What you will build
•	Bind SfComboBox to a local JSON file and a REST API endpoint
•	Configure ItemsSource, DisplayMemberPath/TextMemberPath, and SelectedItem
•	Apply simple, resilient async patterns with clear UI updates

## Bind SfComboBox to a local JSON file
JSON (JavaScript Object Notation) is a lightweight, text based data format used for transmitting structured data. It represents data as key value pairs and arrays, is easy for humans to read/write, and easy for machines to parse; its MIME type is application/json.

### MVVM (Model-View-ViewModel) 
We call the API asynchronously with HttpClient, deserialize the JSON into DTOs, sort by Name, and repopulate an ObservableCollection so ItemsSource refreshes cleanly.

```
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
```


### XAML
We provide a button to fetch data and bind the ComboBox to Users with Name for display/text, ready for selection and search.

```
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
```


### Sample JSON (countries.json)
Load and deserialize a bundled country.json file asynchronously, then fill an Observable Collection for binding.
Bind the ComboBox to Countries, show Name, and reflect selection via Selected.

```
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
```

![ComboBoxwithJson](https://github.com/user-attachments/assets/c05d9b92-a857-4418-932e-cabad5aa4519)


## Bind SfComboBox to a REST API
A REST (Representational State Transfer) API is an architectural style for building web services that treat data as resources accessible via URLs and manipulated with standard HTTP methods (GET, POST, PUT, PATCH, DELETE). It is stateless, cacheable, and uses a uniform interface, commonly exchanging data in JSON.

### MVVM (Model-View-ViewModel) 
We fetch users asynchronously from a REST endpoint with HttpClient, deserialize JSON into DTOs, sort by Name, and repopulate an ObservableCollection so the ComboBox updates immediately.
The click handler triggers the async load via the page’s BindingContext, keeping the flow MVVM friendly and responsive.

```
public class RestViewModel
{
	public ObservableCollection<UserDto> Users { get; } = new();

	private readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
	private const string BaseUrl = "https://jsonplaceholder.typicode.com/users";

	public async Task LoadAsync(CancellationToken ct = default)
	{
		Users.Clear();
		using var req = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
		using var resp = await http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
		resp.EnsureSuccessStatusCode();
		var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		await using var stream = await resp.Content.ReadAsStreamAsync(ct);
		var data = await JsonSerializer.DeserializeAsync<List<UserDto>>(stream, options, ct);

		if (data is null || data.Count == 0)
		{
			// Nothing returned; leave Users empty
			return;
		}
		foreach (var u in data
		.Where(u => u is not null)
		.OrderBy(u => u.Name ?? string.Empty))
		{
			u.Name ??= string.Empty; // avoid null in bindings
			Users.Add(u);
		}		
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
```

### XAML
We provide a Load button and bind Items source to Users; DisplayMemberPath/TextMemberPath use Name so search and display align.
The ComboBox is ready for selection and type ahead; it refreshes automatically when Users are repopulated after the API call.

```
<Grid Padding="50" RowDefinitions="60,60" ColumnDefinitions="*,*">
	<Button Text="Load from API"
			WidthRequest="320"
			HeightRequest="45"
			Grid.Row="0"
			Grid.Column="0"
			Clicked="OnLoadClicked" />

	<Entry x:Name="NameEntry"
	Grid.Row="1"
	Grid.Column="1"
		   HeightRequest="45"
		   WidthRequest="320"
		   Placeholder="Enter new user name to POST" />

	<Button Text="Create user (POST)"
			WidthRequest="320"
			HeightRequest="45"
			Grid.Row="0"
			Grid.Column="1"
			Clicked="OnCreateClicked" />

	<inputs:SfComboBox WidthRequest="320"
	HeightRequest="45"
					   Grid.Row="1"
					   Grid.Column="0"
					   ItemsSource="{Binding Users}"
					   DisplayMemberPath="Name"
					   TextMemberPath="Name" />

</Grid>
```

![ComboBoxwithRestAPIs](https://github.com/user-attachments/assets/375c8d9c-f3b4-4240-86e5-101c9161b2b3)


## Troubleshooting
If you are facing a path too long exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.Path too long exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.