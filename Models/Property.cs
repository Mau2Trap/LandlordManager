using SQLite;

namespace LandlordManager.Models
{
    public class Property
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; } // Supports Canadian focus (ON, BC, etc.) [cite: 298]
        public decimal MonthlyRent { get; set; }
        public string ImagePath { get; set; } // For storing property photos
    }
}