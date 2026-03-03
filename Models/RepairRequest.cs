using SQLite;

namespace LandlordManager.Models
{ 
    public class RepairRequest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PropertyId { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime ReportedDate { get; set; }

        // Helpers
        public string StatusColor => Status == "Closed" ? "#2ecc71" : "#e74c3c";
        public bool IsActive => Status != "Closed";
    }
}