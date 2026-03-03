using SQLite;

namespace LandlordManager.Models
{
    public class Tenant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int PropertyId { get; set; }

        public DateTime LeaseStart { get; set; }
        public DateTime LeaseEnd { get; set; }
        public decimal MonthlyRent { get; set; }
        public bool IsActive { get; set; }

        // New Security Deposit Fields
        public decimal SecurityDeposit { get; set; }
        public string DepositStatus { get; set; } // "held", "returned", "partially_returned"

        // New Lease Renewal Fields
        public string RenewalStatus { get; set; } // "none", "pending", "accepted", "declined"
        public decimal RenewalNewRent { get; set; }
        public DateTime? RenewalNewLeaseEnd { get; set; }
    }
}