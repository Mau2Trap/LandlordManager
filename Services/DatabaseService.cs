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

            // Create ALL tables (Ensure you created the Document and RepairRequest files above first!)
            await _database.CreateTableAsync<Property>();
            await _database.CreateTableAsync<Tenant>();
            await _database.CreateTableAsync<Transaction>();
            await _database.CreateTableAsync<Message>();
            await _database.CreateTableAsync<Document>();
            await _database.CreateTableAsync<RepairRequest>();
        }

        // --- DASHBOARD ---
        public async Task<(int PropCount, int TenantCount, decimal NetIncome)> GetDashboardStatsAsync()
        {
            await Init();
            // Use simple checks to avoid crashing if tables are empty
            int props = 0;
            int tenants = 0;
            decimal netIncome = 0;

            try
            {
                props = await _database.Table<Property>().CountAsync();
                tenants = await _database.Table<Tenant>().Where(t => t.IsActive).CountAsync();
                var transactions = await _database.Table<Transaction>().ToListAsync();
                var income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
                var expenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
                netIncome = income - expenses;
            }
            catch
            {
                // Table might not exist yet, ignore
            }

            return (props, tenants, netIncome);
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

        // --- FINANCIALS ---
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

        // --- MESSAGING ---
        public async Task<List<Message>> GetMessagesForTenantAsync(int tenantId)
        {
            await Init();
            return await _database.Table<Message>().Where(m => m.TenantId == tenantId).OrderBy(m => m.Timestamp).ToListAsync();
        }

        public async Task<int> SendMessageAsync(Message message)
        {
            await Init();
            return await _database.InsertAsync(message);
        }

        // --- DOCUMENTS ---
        public async Task<List<Document>> GetDocumentsAsync()
        {
            await Init();
            return await _database.Table<Document>().OrderByDescending(d => d.UploadDate).ToListAsync();
        }

        public async Task<int> SaveDocumentAsync(Document doc)
        {
            await Init();
            return await _database.InsertAsync(doc);
        }

        // --- REPAIRS ---
        public async Task<List<RepairRequest>> GetRepairsAsync()
        {
            await Init();
            return await _database.Table<RepairRequest>()
                                  .OrderByDescending(r => r.Priority)
                                  .ThenByDescending(r => r.ReportedDate)
                                  .ToListAsync();
        }

        public async Task<int> SaveRepairAsync(RepairRequest repair)
        {
            await Init();
            return await _database.InsertAsync(repair);
        }

        public async Task<int> UpdateRepairStatusAsync(RepairRequest repair)
        {
            await Init();
            return await _database.UpdateAsync(repair);
        }
    }
}