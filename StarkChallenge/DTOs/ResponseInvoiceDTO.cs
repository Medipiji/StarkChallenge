namespace StarkChallenge.DTOs
{
    public class ResponseInvoiceDTO
    {
        public StarkEvent Event { get; set; } = new StarkEvent();
    }

    public class StarkEvent
    {
        public DateTime Created { get; set; } = DateTime.MinValue;
        public string Id { get; set; } = string.Empty;  
        public string Subscription { get; set; } = string.Empty;
        public string WorkspaceId { get; set; } = string.Empty; 
        public StarkLog Log { get; set; } = new StarkLog();
    }

    public class StarkLog
    {
        public string Authentication { get; set; } = string.Empty;  
        public DateTime Created { get; set; } = new DateTime();
        public List<object> Errors { get; set; } = new List<object>();
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;    
        public StarkInvoice Invoice { get; set; } = new StarkInvoice();
    }

    public class StarkInvoice
    {
        public long Amount { get; set; } = long.MinValue;
        public string Brcode { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.MinValue;

        public List<object> Descriptions { get; set; } = new List<object>();

        public long DiscountAmount { get; set; } = long.MinValue;
        public List<object> Discounts { get; set; } = new List<object>();

        public string DisplayDescription { get; set; } = string.Empty;
        public DateTime Due { get; set; } = DateTime.MinValue;
        public long Expiration { get; set; } = long.MinValue;

        public long Fee { get; set; } = long.MinValue;

        public double Fine { get; set; } = double.MinValue;
        public long FineAmount { get; set; } = long.MinValue;

        public string Id { get; set; } = string.Empty;

        public double Interest { get; set; } = double.MinValue;
        public long InterestAmount { get; set; } = long.MinValue;

        public string Link { get; set; } = string.Empty;

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public string Name { get; set; } = string.Empty;
        public long NominalAmount { get; set; } = long.MinValue;    

        public string Pdf { get; set; } = string.Empty;
        public string ReversalDisplayDescription { get; set; } = string.Empty;

        public List<object> Rules { get; set; } = new List<object>();
        public List<object> Splits { get; set; } = new List<object>();

        public string Status { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();

        public string TaxId { get; set; } = string.Empty;

        public List<string> TransactionIds { get; set; } = new List<string>();

        public DateTime Updated { get; set; } = new DateTime(); 
    }
}
