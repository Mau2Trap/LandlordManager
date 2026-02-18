using SQLite;

namespace LandlordManager.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PropertyId { get; set; } // Links money to a specific unit
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // "Income" or "Expense"
        public string Category { get; set; } // e.g., "Rent", "Maintenance", "Tax"
        public string ReceiptPath { get; set; } // For tax proof [cite: 137]

        public string DisplayAmount => Type == "Income" ? $"+${Amount:F2}" : $"-${Amount:F2}";
        public string ColorHex => Type == "Income" ? "#2ecc71" : "#e74c3c";
    }
}