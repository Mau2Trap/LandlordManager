using LandlordManager.Models;
using LandlordManager.Services;

namespace LandlordManager;

public partial class DocumentsPage : ContentPage
{
    DatabaseService _dbService;

    public DocumentsPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        cvDocuments.ItemsSource = await _dbService.GetDocumentsAsync();
    }

    private async void OnUploadClicked(object sender, EventArgs e)
    {
        // 1. Pick a file
        var result = await FilePicker.Default.PickAsync();
        if (result != null)
        {
            // 2. Ask for a name
            string title = await DisplayPromptAsync("Save Document", "Enter a name for this file:");
            if (string.IsNullOrWhiteSpace(title)) title = result.FileName;

            // 3. Save reference to Database
            var doc = new Document
            {
                Title = title,
                FilePath = result.FullPath,
                FileType = result.FileName.EndsWith(".pdf") ? "PDF" : "Image",
                UploadDate = DateTime.Now
            };

            await _dbService.SaveDocumentAsync(doc);
            OnAppearing(); // Refresh list
        }
    }
}