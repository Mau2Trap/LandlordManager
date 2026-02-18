using SQLite;

namespace LandlordManager.Models
{
    public class Document
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } // e.g., "Lease - John Doe"
        public string FilePath { get; set; } // Where the file lives on the phone
        public string FileType { get; set; } // "Image" or "PDF"
        public DateTime UploadDate { get; set; }

        // Helper for UI icons
        public string IconImage => FileType == "PDF" ? "pdf_icon.png" : "image_icon.png";
    }
}