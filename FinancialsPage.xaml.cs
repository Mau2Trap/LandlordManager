using LandlordManager.Models;
using LandlordManager.Services;

namespace LandlordManager;

public partial class FinancialsPage : ContentPage
{
    DatabaseService _dbService;

    // A simple class to help us group expenses
    public class CategoryTotal
    {
        public string Name { get; set; }
        public decimal Total { get; set; }
        public double Percentage { get; set; }
        public Color BarColor { get; set; }
    }

    public FinancialsPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFinancialData();
    }

    async Task LoadFinancialData()
    {
        var transactions = await _dbService.GetTransactionsAsync();

        // 1. Calculate Totals
        decimal totalIncome = transactions.Where(t => t.Type == "Expense" == false).Sum(t => t.Amount); // Defaults to Income if type is missing
        decimal totalExpense = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        decimal netProfit = totalIncome - totalExpense;

        // Calculate Profit Margin
        double margin = totalIncome > 0 ? (double)(netProfit / totalIncome) * 100 : 0;

        // 2. Update UI Labels
        lblIncome.Text = $"${totalIncome:N2}";
        lblExpenses.Text = $"${totalExpense:N2}";
        lblNetProfit.Text = $"${netProfit:N2}";
        lblNetProfit.TextColor = netProfit >= 0 ? Color.FromArgb("#0f172a") : Color.FromArgb("#e11d48");

        lblMargin.Text = $"{margin:F0}% Margin";
        frmMargin.BackgroundColor = netProfit >= 0 ? Color.FromArgb("#ecfdf5") : Color.FromArgb("#fff1f2");
        lblMargin.TextColor = netProfit >= 0 ? Color.FromArgb("#059669") : Color.FromArgb("#e11d48");

        // 3. Build Expense Breakdown (Group by Category)
        slCategoryBreakdown.Children.Clear();

        if (totalExpense == 0)
        {
            slCategoryBreakdown.Children.Add(new Label { Text = "No expenses recorded yet.", TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center });
            return;
        }

        // Group expenses by category name
        var categoryGroups = transactions
            .Where(t => t.Type == "Expense")
            .GroupBy(t => string.IsNullOrWhiteSpace(t.Category) ? "Uncategorized" : t.Category)
            .Select(g => new CategoryTotal
            {
                Name = g.Key,
                Total = g.Sum(t => t.Amount),
                Percentage = (double)(g.Sum(t => t.Amount) / totalExpense)
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        // Colors to cycle through for the bars
        Color[] palette = { Color.FromArgb("#3b82f6"), Color.FromArgb("#f59e0b"), Color.FromArgb("#ef4444"), Color.FromArgb("#8b5cf6") };
        int colorIndex = 0;

        // Dynamically build the UI for each category
        foreach (var cat in categoryGroups)
        {
            cat.BarColor = palette[colorIndex % palette.Length];
            colorIndex++;

            var grid = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto) },
                Margin = new Thickness(0, 0, 0, 5)
            };

            // Category Name & Amount
            grid.Add(new Label { Text = cat.Name, FontSize = 14, TextColor = Color.FromArgb("#334155"), FontAttributes = FontAttributes.Bold }, 0, 0);
            grid.Add(new Label { Text = $"${cat.Total:N0}", FontSize = 14, TextColor = Color.FromArgb("#334155"), HorizontalOptions = LayoutOptions.End }, 1, 0);

            // Progress Bar Background (Gray)
            var bgBar = new BoxView { Color = Color.FromArgb("#f1f5f9"), HeightRequest = 8, CornerRadius = 4, Margin = new Thickness(0, 5, 0, 0) };
            grid.Add(bgBar, 0, 1);
            Grid.SetColumnSpan(bgBar, 2);

            // Actual Progress Bar (Colored)
            var fillBar = new BoxView
            {
                Color = cat.BarColor,
                HeightRequest = 8,
                CornerRadius = 4,
                Margin = new Thickness(0, 5, 0, 0),
                HorizontalOptions = LayoutOptions.Start
            };

            // Note: In MAUI, to make a BoxView scale via percentage, we bind its width or use a trick.
            // For simplicity, we'll let it fill a proportion of the screen width visually.
            // A more robust way is to use a nested Grid with column sizing.
            var barGrid = new Grid
            {
                ColumnDefinitions = {
                    new ColumnDefinition(new GridLength(cat.Percentage, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(1 - cat.Percentage, GridUnitType.Star))
                },
                Margin = new Thickness(0, 5, 0, 0)
            };
            barGrid.Add(new BoxView { Color = cat.BarColor, HeightRequest = 8, CornerRadius = 4 }, 0, 0);

            grid.Add(barGrid, 0, 1);
            Grid.SetColumnSpan(barGrid, 2);

            slCategoryBreakdown.Children.Add(grid);
        }
    }
}