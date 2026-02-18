using LandlordManager.Models;
using LandlordManager.Services;

namespace LandlordManager;

public partial class TenantsPage : ContentPage
{
    DatabaseService _dbService;

    public TenantsPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        cvTenants.ItemsSource = await _dbService.GetTenantsAsync();
    }

    private async void OnAddTenantClicked(object sender, EventArgs e)
    {
        // Simple Add Tenant Dialog
        string name = await DisplayPromptAsync("New Tenant", "Enter Full Name:");
        if (!string.IsNullOrWhiteSpace(name))
        {
            await _dbService.SaveTenantAsync(new Tenant { FullName = name, IsActive = true });
            OnAppearing();
        }
    }

    private async void OnTenantSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Tenant selectedTenant)
        {
            // Navigate to the Chat Page (We will create this next)
            await Navigation.PushAsync(new ChatPage(selectedTenant));
            // Clear selection
            cvTenants.SelectedItem = null;
        }
    }
}