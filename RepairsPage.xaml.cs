using LandlordManager.Models;

namespace LandlordManager;

public partial class RepairsPage : ContentPage
{
    DatabaseService _dbService;

    public RepairsPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadRepairs();
    }

    async Task LoadRepairs()
    {
        cvRepairs.ItemsSource = await _dbService.GetRepairsAsync();
    }

    private async void OnAddRepairClicked(object sender, EventArgs e)
    {
        string title = await DisplayPromptAsync("New Repair", "What is the issue? (e.g. Leaky Faucet)");
        if (string.IsNullOrWhiteSpace(title)) return;

        string priority = await DisplayActionSheet("Priority Level?", "Cancel", null, "Low", "Medium", "High", "Critical");
        if (priority == "Cancel" || priority == null) return;

        var repair = new RepairRequest
        {
            Title = title,
            Description = "Reported by Landlord", // In a full app, this would come from the tenant
            Priority = priority,
            Status = "Open",
            ReportedDate = DateTime.Now
        };

        await _dbService.SaveRepairAsync(repair);
        await LoadRepairs();
    }

    private async void OnRepairSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is RepairRequest selectedRepair)
        {
            string action = await DisplayActionSheet($"Manage: {selectedRepair.Title}", "Cancel", null, "Mark as In Progress", "Mark as Completed");

            if (action == "Mark as In Progress")
            {
                selectedRepair.Status = "In Progress";
                await _dbService.UpdateRepairStatusAsync(selectedRepair);
            }
            else if (action == "Mark as Completed")
            {
                selectedRepair.Status = "Closed";
                await _dbService.UpdateRepairStatusAsync(selectedRepair);
            }

            cvRepairs.SelectedItem = null; // Deselect
            await LoadRepairs(); // Refresh list to update colors
        }
    }
}