using CloudKit;
using LandlordManager.Models;
using SQLite;

namespace LandlordManager.Services
{
    public class DatabaseService
    {
        SQLiteAsyncConnection _database;

        // Inside Init() method:
        await _database.CreateTableAsync<Message>();

        // Add these helper methods for Messaging:
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

        public async Task<(int PropCount, int TenantCount, decimal NetIncome)> GetDashboardStatsAsync()
        {
            await Init();

            // 1. Count Properties
            var props = await _database.Table<Property>().CountAsync();

            // 2. Count Active Tenants
            var tenants = await _database.Table<Tenant>().Where(t => t.IsActive).CountAsync();

            // 3. Calculate Income (Income - Expenses)
            var transactions = await _database.Table<Transaction>().ToListAsync();
            decimal income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);

            return (props, tenants, income - expenses);
        }

        public async Task<(int PropCount, int TenantCount, decimal NetIncome)> GetDashboardStatsAsync()
        {
            await Init();

            // 1. Count Properties
            var props = await _database.Table<Property>().CountAsync();

            // 2. Count Active Tenants
            var tenants = await _database.Table<Tenant>().Where(t => t.IsActive).CountAsync();

            // 3. Calculate Income (Income - Expenses)
            var transactions = await _database.Table<Transaction>().ToListAsync();
            decimal income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);

            return (props, tenants, income - expenses);
        }
    }
}