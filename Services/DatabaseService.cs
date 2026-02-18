using SQLite;
using LandlordManager.Models;

namespace LandlordManager.Services
{
    public class DatabaseService
    {
        SQLiteAsyncConnection _database;

        async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "RentalystData.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Create ALL tables here
            await _database.CreateTableAsync<Property>();
            await _database.CreateTableAsync<Tenant>();
            await _database.CreateTableAsync<Transaction>();
            await _database.CreateTableAsync<Message>();
        }

        // --- DASHBOARD STATS ---
        public async Task<(int PropCount, int TenantCount, decimal NetIncome)> GetDashboardStatsAsync()
        {
            await Init();

            var props = await _database.Table<Property>().CountAsync();
            var tenants = await _database.Table<Tenant>().Where(t => t.IsActive).CountAsync();

            var transactions = await _database.Table<Transaction>().ToListAsync();
            decimal income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);

            return (props, tenants, income - expenses);
        }

        // --- PROPERTIES ---
        public async Task<List<Property>> GetPropertiesAsync()
        {
            await Init();
            return await _database.Table<Property>().ToListAsync();
        }

        public async Task<int> SavePropertyAsync(Property property)
        {
            await Init();
            return await _database.InsertAsync(property);
        }

        // --- TRANSACTIONS (FINANCIALS) ---
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            await Init();
            return await _database.Table<Transaction>().OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<int> SaveTransactionAsync(Transaction transaction)
        {
            await Init();
            return await _database.InsertAsync(transaction);
        }

        // --- TENANTS ---
        public async Task<List<Tenant>> GetTenantsAsync()
        {
            await Init();
            return await _database.Table<Tenant>().ToListAsync();
        }

        public async Task<int> SaveTenantAsync(Tenant tenant)
        {
            await Init();
            return await _database.InsertAsync(tenant);
        }

        // --- MESSAGING ---
        public async Task<List<Message>> GetMessagesForTenantAsync(int tenantId)
        {
            await Init();
            return await _database.Table<Message>()
                                    .Where(m => m.TenantId == tenantId)
                                    .OrderBy(m => m.Timestamp)
                                    .ToListAsync();
        }

        public async Task<int> SendMessageAsync(Message message)
        {
            await Init();
            return await _database.InsertAsync(message);
        }
    }
}