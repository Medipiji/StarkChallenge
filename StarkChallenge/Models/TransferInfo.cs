using System.Security.Principal;

namespace StarkChallenge.Models
{
    public class TransferInfo
    {
        public string BankCode { get; set; } = "";
        public string BranchCode { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string Name { get; set; } = "";
        public string TaxID { get; set; } = "";
        public string AccountType { get; set; } = "";
    }
}
