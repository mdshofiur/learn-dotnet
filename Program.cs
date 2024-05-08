
var builder = WebApplication.CreateBuilder(args);

// Register HttpClient in the services container
builder.Services.AddHttpClient();


var app = builder.Build();



// Hello Word API
app.MapGet("/", () => "Hello World!");


// Inject the HttpClientFactory instance into the endpoint handler using dependency injection
app.MapGet("/users", async (HttpContext context) =>
{
    var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
    var client = clientFactory.CreateClient();

    var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
    var users = await response.Content.ReadAsStringAsync();
    return users;
});


// Return a list of people
app.MapGet("/people", () => new[]
{
    new Person("Ana"), new Person("Filipe"), new Person("Emillia")
});

// Run the app
app.Run();

// Define a record type
record Person(string Name);