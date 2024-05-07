using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrameWorklessApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new HttpListener();
            var url = "http://localhost:8080/";
            server.Prefixes.Add(url);
            server.Start();


            Console.WriteLine($"Listening on {url}");

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var context = await server.GetContextAsync();  // Gets the request asynchronously

                    var message = context.Request.Url.AbsolutePath;

                    Console.WriteLine($"{context.Request.HttpMethod} request made to --> {context.Request.Url}");

                    // Check if the requested URL matches "/helloWorld"
                    if (message == "/helloWorld")
                    {
                        // Set the content type to JSON
                        context.Response.ContentType = "application/json";

                        // Define the JSON response
                        string jsonResponse = "{\"message\": \"Hello, World!\"}";

                        // Convert the JSON string to bytes
                        var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);

                        // Write the JSON response to the output stream
                        context.Response.ContentLength64 = buffer.Length;
                        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        // If the requested URL doesn't match "/helloWorld", return a 404 Not Found response
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    // Close the response
                    context.Response.Close();
                }
            });

            // Keep the program running until cancellation
            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                cancellationTokenSource.Cancel();
                e.Cancel = true;
            };

            await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);

            server.Stop();
        }
    }
}
