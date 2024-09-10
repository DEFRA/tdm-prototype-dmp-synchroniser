namespace TdmPrototypeDmpSynchroniser.Config;

public class SynchroniserConfig
{
    public string DmpEnvironment { get; set; } = default!;
    public string DmpBusNamespace { get; set; } = default!;
    public string DmpBusTopic { get; set; } = default!;
    public string DmpBusSubscription { get; set; } = default!;
    
    public string? DmpBusPrivateEndpoint { get; set; } = default!;
    public string DmpBlobUri { get; set; } = default!;
    
    public string? DmpBlobPrivateEndpoint { get; set; } = default!;
    public string DmpBlobContainer { get; set; } = default!;
    public string? TradeApiEmvironment { get; set; } = default!;
    public string? TradeApiUri { get; set; } = default!;
    public string? TdmBackendApiUri { get; set; } = default!;
    public string? AzureClientId { get; set; } = default!;
    public string? AzureTenantId { get; set; } = default!;
    public string? AzureClientSecret { get; set; } = default!;
    
    // public string? CdsHttpsProxy { get; set; } = default!;
    
    public SynchroniserConfig(IConfiguration configuration)
    {
        // CdsHttpsProxy = configuration["CDP_HTTPS_PROXY"];
        var dmpSlot = configuration["DMP_SLOT"]!;
        
        DmpEnvironment = configuration["DMP_ENVIRONMENT"]!;
        
        AzureClientId = configuration["AZURE_CLIENT_ID"];
        AzureTenantId = configuration["AZURE_TENANT_ID"];
        AzureClientSecret = configuration["AZURE_CLIENT_SECRET"];
        
        TradeApiEmvironment = configuration["TRADE_API_ENVIRONMENT"];
        TradeApiUri = $"https://{TradeApiEmvironment}-gateway.trade.azure.defra.cloud";
        TdmBackendApiUri = configuration["TDM_BACKEND_API_URI"];
        
        DmpBusNamespace = $"{configuration["DMP_SERVICE_BUS_NAME"]!}.servicebus.windows.net";
        DmpBusTopic = $"defra.trade.dmp.ingestipaffs.{DmpEnvironment}.{dmpSlot}.topic";
        DmpBusSubscription = $"defra.trade.dmp.{DmpEnvironment}.{dmpSlot}.subscription";
        DmpBlobUri = $"https://{configuration["DMP_BLOB_STORAGE_NAME"]!}.blob.core.windows.net";
        DmpBlobContainer = $"dmp-data-{dmpSlot}";
        
        DmpBlobPrivateEndpoint = $"https://{configuration["DMP_BLOB_PRIVATE_ENDPOINT"]}";
        DmpBusPrivateEndpoint = configuration["DMP_BUS_PRIVATE_ENDPOINT"];
    }
}