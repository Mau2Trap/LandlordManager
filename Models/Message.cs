using SQLite;

namespace LandlordManager.Models
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int TenantId { get; set; } // Links the chat to a specific tenant

        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsFromLandlord { get; set; } // True = You sent it. False = Tenant sent it.

        // Helper for the UI design (Slate/White bubbles)
        public LayoutOptions Alignment => IsFromLandlord ? LayoutOptions.End : LayoutOptions.Start;
        public Color BubbleColor => IsFromLandlord ? Color.FromArgb("#0f172a") : Color.FromArgb("#e2e8f0"); // Dark Slate vs Light Gray
        public Color TextColor => IsFromLandlord ? Colors.White : Colors.Black;
    }
}