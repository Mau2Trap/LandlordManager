using SQLite;

namespace LandlordManager.Models 
{
    public class Document
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public string IconImage => FileType == "PDF" ? "pdf_icon.png" : "image_icon.png";
    }
}