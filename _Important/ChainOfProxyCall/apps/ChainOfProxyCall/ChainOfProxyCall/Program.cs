using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpContext context) =>
{
    // Get the current host name
    string host = System.Net.Dns.GetHostName();

    // Retrieve the "url" query parameter
    var query = context.Request.Query;
    if (!query.ContainsKey("url") || string.IsNullOrWhiteSpace(query["url"]))
    {
        await context.Response.WriteAsync($"End of the road from {host}");
        return;
    }

    // Split the URL value on the pipe character.
    // The first segment is the target URL.
    // The rest (if any) will be forwarded as the new "url" query parameter.
    string urlParam = query["url"].ToString();
    var parts = urlParam.Split(new[] { '|' }, 2);
    
    string targetUrl = parts[0];
    string remainingUrls = parts.Length > 1 ? parts[1] : string.Empty;

    // Construct the forwarding URL.
    // If there are remaining URLs, add them as the "url" query parameter.
    string finalUrl = targetUrl;
    if (!string.IsNullOrEmpty(remainingUrls))
    {
        finalUrl = QueryHelpers.AddQueryString(targetUrl, "url", remainingUrls);
    }

    // Forward the request to the target URL using HttpClient.
    using var httpClient = new HttpClient();
    HttpResponseMessage response;
    try
    {
        response = await httpClient.GetAsync(finalUrl);
    }
    catch (Exception ex)
    {
        // If there's an error reaching the target, return an error message.
        await context.Response.WriteAsync($"Error forwarding to {targetUrl}: {ex.Message}");
        return;
    }

    // Read the response content.
    string responseBody = await response.Content.ReadAsStringAsync();

    // Append the current host info to the response.
    responseBody += $" | {host} forwarding.";

    // Write the final response.
    await context.Response.WriteAsync(responseBody);
});

app.Run();
