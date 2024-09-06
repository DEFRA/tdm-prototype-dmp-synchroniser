

using System.Diagnostics.CodeAnalysis;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class WebService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables)
    : BaseService(loggerFactory, environmentVariables), IWebService
{
    
    public async Task<Status> CheckTradeApiAsync()
    {
        try
        {
            Extensions.AssertIsNotNull(environmentVariables.TradeApiGatewayUri);
            
            Logger.LogInformation("Attempting connection to {0}", environmentVariables.TradeApiGatewayUri);
            HttpClient client = new()
            {
                BaseAddress = new Uri(environmentVariables.TradeApiGatewayUri),
                Timeout = TimeSpan.FromSeconds(10),
            };
            
            using HttpRequestMessage request = new(
                HttpMethod.Head, 
                environmentVariables.TradeApiGatewayUri);

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