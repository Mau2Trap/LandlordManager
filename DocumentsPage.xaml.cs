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
        await LoadDocuments();
    }

    async Task LoadDocuments()
    {
        var docs = await _dbService.GetDocumentsAsync();
        cvDocuments.ItemsSource = docs;
        lblEmpty.IsVisible = docs.Count == 0;
    }

    private async void OnUploadClicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Open the Phone's File Picker (PDFs and Images)
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a Lease or Receipt",
                FileTypes = FilePickerFileType.Images // You can change this to .Pdf later if needed
            });

            if (result != null)
            {
                // 2. Ask user to name it
                string title = await DisplayPromptAsync("Save Document", "Enter a name (e.g., 'Unit 101 Lease'):");
                if (string.IsNullOrWhiteSpace(title)) title = result.FileName;

                // 3. Save to Database
                var doc = new Document
                {
                    Title = title,
                    FilePath = result.FullPath,
                    FileType = result.FileName.EndsWith(".pdf") ? "PDF" : "Image",
                    UploadDate = DateTime.Now
                };

                await _dbService.SaveDocumentAsync(doc);
                await LoadDocuments();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Could not upload file: " + ex.Message, "OK");
        }
    }

    private async void OnDocumentTapped(object sender, TappedEventArgs e)
    {
        // Feature for later: Open the file to view it
        if (e.Parameter is Document doc)
        {
            await DisplayAlert("File Selected", $"You tapped: {doc.Title}", "OK");
        }
    }
}