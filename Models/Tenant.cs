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
        public int PropertyId { get; set; } // Links tenant to a specific property
        public DateTime LeaseStart { get; set; }
        public DateTime LeaseEnd { get; set; }
        public bool IsActive { get; set; }
    }
}