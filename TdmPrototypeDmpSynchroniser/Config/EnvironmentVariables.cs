namespace TdmPrototypeDmpSynchroniser.Config;

public class EnvironmentVariables
{
    public string DmpEnvironment { get; set; } = default!;
    public string DmpBusNamespace { get; set; } = default!;
    public string DmpBusTopic { get; set; } = default!;
    public string DmpBusSubscription { get; set; } = default!;
    public string DmpBlobUri { get; set; } = default!;
    public string DmpBlobContainer { get; set; } = default!;
    public string? TradeApiEmvironment { get; set; } = default!;
    public string? TradeApiUri { get; set; } = default!;
    public string? TdmBackendApiUri { get; set; } = default!;
    public string? AzureClientId { get; set; } = default!;
    
    public EnvironmentVariables()
    {
        
        var dmpSlot = System.Environment.GetEnvironmentVariable("DMP_SLOT")!;
        
        DmpEnvironment = System.Environment.GetEnvironmentVariable("DMP_ENVIRONMENT")!;
        AzureClientId = System.Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
        TradeApiEmvironment = System.Environment.GetEnvironmentVariable("TRADE_API_ENVIRONMENT");
        TradeApiUri = $"https://{TradeApiEmvironment}-gateway.trade.azure.defra.cloud";
        TdmBackendApiUri = System.Environment.GetEnvironmentVariable("TDM_BACKEND_API_URI");
        DmpBusNamespace = $"{System.Environment.GetEnvironmentVariable("DMP_SERVICE_BUS_NAME")!}.servicebus.windows.net";
        DmpBusTopic = $"defra.trade.dmp.ingestipaffs.{DmpEnvironment}.{dmpSlot}.topic";
        DmpBusSubscription = $"defra.trade.dmp.{DmpEnvironment}.{dmpSlot}.subscription";
        DmpBlobUri = $"https://{System.Environment.GetEnvironmentVariable("DMP_BLOB_STORAGE_NAME")!}.blob.core.windows.net";
        DmpBlobContainer = $"dmp-data-{dmpSlot}";
    }
}