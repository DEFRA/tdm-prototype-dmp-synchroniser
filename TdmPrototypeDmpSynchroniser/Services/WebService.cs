

using System.Diagnostics.CodeAnalysis;
using System.Net;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class WebService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables, IHttpClientFactory clientFactory)
    : BaseService(loggerFactory, environmentVariables), IWebService
{

    public async Task<Status> CheckGoogleAsync()
    {
        return await CheckApiAsync("https://www.google.com");
    }
    
    public async Task<Status> CheckTradeApiAsync()
    {
        return await CheckApiAsync("https://www.google.com");
    }
    
    private async Task<Status> CheckApiAsync(string uri)
    {
        try
        {
            Extensions.AssertIsNotNull(uri);
            
            Logger.LogInformation("Attempting connection to {0}", uri);
            
            // Gets a proxied client when CDP_HTTP_PROXY is set whilst running in CDP 
            HttpClient client = clientFactory.CreateClient("proxy");

            using HttpRequestMessage request = new(
                HttpMethod.Head, 
                uri);

            using HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode()
                .WriteRequestToConsole();

            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine();
            
            return new Status() { Success = false, Description = "Connected" };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return new Status() { Success = false, Description = ex.Message };
        }
    }
}