using LandlordManager.Services;
using LandlordManager.Models;

namespace LandlordManager
{
    public partial class MainPage : ContentPage
    {
        DatabaseService _dbService;

        public MainPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDashboardStats();
        }

        async Task LoadDashboardStats()
        {
            // 1. Get the numbers from your database
            var stats = await _dbService.GetDashboardStatsAsync();

            // 2. Update the Dashboard Cards (Labels)
            lblPropCount.Text = stats.PropCount.ToString();
            lblTenantCount.Text = stats.TenantCount.ToString();

            // Format Income: shows $5,000 (Green) or -$500 (Red)
            lblIncome.Text = $"${stats.NetIncome:N0}";
            lblIncome.TextColor = stats.NetIncome >= 0 ? Color.FromArgb("#059669") : Color.FromArgb("#e11d48");
        }

        // Quick Action: Add Property
        // This takes you directly to the Properties tab we built earlier
        private async void OnAddPropertyClicked(object sender, EventArgs e)
        {
            // This navigates to the "Properties" tab defined in AppShell
            await Shell.Current.GoToAsync("///PropertiesPage");
        }

        // Quick Action: Add Expense
        // Keeps your existing logic but refreshes the new dashboard
        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Add Expense", "Enter amount:", keyboard: Keyboard.Numeric);
            if (decimal.TryParse(result, out decimal amount))
            {
                await _dbService.SaveTransactionAsync(new Transaction
                {
                    Title = "Quick Expense",
                    Amount = amount,
                    Date = DateTime.Now,
                    Type = "Expense"
                });

                // Refresh the dashboard numbers immediately
                await LoadDashboardStats();
            }
        }
    }
}