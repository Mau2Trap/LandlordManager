using SQLite;

namespace LandlordManager.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // "income" or "expense"

        // New Fields from Base44 Blueprint
        public string Category { get; set; } // "utility", "tax", "maintenance", "insurance", "mortgage"
        public bool IsRecurring { get; set; }
        public string Vendor { get; set; } // e.g., "Home Depot", "City Water"

        public string ReceiptPath { get; set; }

        public string DisplayAmount => Type == "income" ? $"+${Amount:F2}" : $"-${Amount:F2}";
        public string ColorHex => Type == "income" ? "#2ecc71" : "#e74c3c";
    }
}