using SQLite;

namespace LandlordManager.Models
{
    public class Property
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        // --- Restored Fields ---
        public string City { get; set; }
        public string Province { get; set; }
        // -----------------------

        public string Type { get; set; }
        public int Units { get; set; }
        public decimal MonthlyRent { get; set; }

        // New Financial Fields from Base44 Blueprint
        public decimal PurchasePrice { get; set; }
        public decimal CurrentValue { get; set; }

        public string Status { get; set; }
        public string ImagePath { get; set; }
    }
}