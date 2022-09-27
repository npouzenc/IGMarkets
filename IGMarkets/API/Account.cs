using System.Text.Json.Serialization;

namespace IGMarkets.API
{
    public class Account
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public bool Preferred{ get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; }
        public string AccountAlias { get; set; }
        public string Currency { get; set; }
        public bool CanTransferFrom { get; set; }
        public bool CanTransferTo { get; set; }
        public AccountInfo Balance { get; set; }
    }
}