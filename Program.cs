
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


// if(!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/error");
//     app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500));
// }

if(!app.Environment.IsProduction())
{
    app.MapGet("/prod", () => Results.Problem("This endpoint is only available in production.", statusCode: 500));
}

if(!app.Environment.IsStaging())
{
    app.MapGet("/stage", () => Results.Problem("This endpoint is only available in staging.", statusCode: 500));
}

if(!app.Environment.IsDevelopment())
{
    app.MapGet("/dev", () => Results.Problem("This endpoint is only available in development.", statusCode: 500));
}

if(app.Environment.IsEnvironment("Development"))
{
    app.MapGet("/env", () => Results.Problem("This endpoint is only available in development.", statusCode: 500));
}


// Run the app
app.Run();

// Define a record type
record Person(string Name);