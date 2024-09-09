namespace TdmPrototypeDmpSynchroniser.Config;

public class EnvironmentVariables
{
    public string DmpBusNamespace { get; set; } = default!;
    public string DmpBusTopic { get; set; } = default!;
    public string DmpBusSubscription { get; set; } = default!;
    public string DmpBlobUri { get; set; } = default!;
    public string DmpBlobContainer { get; set; } = default!;
    public string? TradeApiGatewayUri { get; set; } = default!;
    public string? TdmBackendApiUri { get; set; } = default!;
    public string? AzureClientId { get; set; } = default!;
    
    public EnvironmentVariables()
    {
        var dmpEnvironment = System.Environment.GetEnvironmentVariable("DMP_ENVIRONMENT")!;
        var dmpSlot = System.Environment.GetEnvironmentVariable("DMP_SLOT")!;
        
        AzureClientId = System.Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
        TradeApiGatewayUri = System.Environment.GetEnvironmentVariable("TRADE_API_GATEWAY_URI");
        TdmBackendApiUri = System.Environment.GetEnvironmentVariable("TDM_BACKEND_API_URI");
        
        DmpBusNamespace = String.Format("{0}.servicebus.windows.net", System.Environment.GetEnvironmentVariable("DMP_SERVICE_BUS_NAME")!);
        DmpBusTopic = String.Format("defra.trade.dmp.ingestipaffs.{0}.{1}.topic", dmpEnvironment, dmpSlot);
        DmpBusSubscription = String.Format("defra.trade.dmp.{0}.{1}.subscription", dmpEnvironment, dmpSlot);
        DmpBlobUri = String.Format("https://{0}.blob.core.windows.net", System.Environment.GetEnvironmentVariable("DMP_BLOB_STORAGE_NAME")!);
        DmpBlobContainer = String.Format("dmp-data-{0}", dmpSlot);
    }
}