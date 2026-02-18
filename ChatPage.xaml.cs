using LandlordManager.Models;
using LandlordManager.Services;

namespace LandlordManager;

public partial class ChatPage : ContentPage
{
    DatabaseService _dbService;
    Tenant _currentTenant;

    public ChatPage(Tenant tenant)
    {
        InitializeComponent();
        _currentTenant = tenant;
        Title = $"Chat with {_currentTenant.FullName}"; // Set the top bar title
        _dbService = new DatabaseService();

        // Load messages immediately
        LoadMessages();
    }

    async void LoadMessages()
    {
        var messages = await _dbService.GetMessagesForTenantAsync(_currentTenant.Id);
        cvMessages.ItemsSource = messages;

        // Scroll to bottom (latest message)
        if (messages.Count > 0)
            cvMessages.ScrollTo(messages.Last(), position: ScrollToPosition.End, animate: false);
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;

        var newMessage = new Message
        {
            TenantId = _currentTenant.Id,
            Text = txtMessage.Text,
            Timestamp = DateTime.Now,
            IsFromLandlord = true // This marks it as YOUR message (Slate Blue)
        };

        await _dbService.SendMessageAsync(newMessage);
        txtMessage.Text = string.Empty;
        LoadMessages();
    }
}