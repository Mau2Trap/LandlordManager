using LandlordManager.Models;
using LandlordManager.Services;

namespace LandlordManager;

public partial class PropertiesPage : ContentPage
{
    DatabaseService _dbService;

    public PropertiesPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var properties = await _dbService.GetPropertiesAsync();
        cvProperties.ItemsSource = properties;
    }

    private async void OnAddPropertyClicked(object sender, EventArgs e)
    {
        string address = await DisplayPromptAsync("New Property", "Enter Address:");
        string rentStr = await DisplayPromptAsync("Rent", "Enter Monthly Rent Amount:", keyboard: Keyboard.Numeric);

        if (!string.IsNullOrWhiteSpace(address) && decimal.TryParse(rentStr, out decimal rent))
        {
            await _dbService.SavePropertyAsync(new Property
            {
                Address = address,
                MonthlyRent = rent,
                City = "Calgary", // Default for now
                Province = "AB"
            });
            OnAppearing(); // Refresh list
        }
    }
}