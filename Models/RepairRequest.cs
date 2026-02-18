using SQLite;

namespace LandlordManager.Models
{
    public class RepairRequest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }        // e.g. "Broken Heater"
        public string Description { get; set; }  // e.g. "Making a loud banging noise"
        public int PropertyId { get; set; }      // Links to the property
        public string Priority { get; set; }     // "Low", "Medium", "High", "Critical"
        public string Status { get; set; }       // "Open", "In Progress", "Closed"
        public DateTime ReportedDate { get; set; }

        // UI Helpers
        public string StatusColor => Status == "Closed" ? "#2ecc71" : "#e74c3c"; // Green if done, Red if open
        public bool IsActive => Status != "Closed";
    }
}