using VaultService.Models;
using MongoDB.Driver;

namespace VaultService.Services;

public class CompensationService
{
    private readonly IMongoCollection<Compensation> _compensations;

    public CompensationService(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var databaseName = configuration["MongoDbSettings:DatabaseName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        
        // A MongoDB automatikusan létrehozza ezt a kollekciót a VaultDb-ben
        _compensations = database.GetCollection<Compensation>("Compensations");
    }

    public async Task<List<Compensation>> GetAsync() =>
        await _compensations.Find(_ => true).ToListAsync();

    // Fontos újítás: Dolgozó ID alapján keressük a béradatot!
    public async Task<Compensation?> GetByEmployeeIdAsync(string employeeId) =>
        await _compensations.Find(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();

    public async Task CreateAsync(Compensation newCompensation) =>
        await _compensations.InsertOneAsync(newCompensation);

    public async Task UpdateAsync(string id, Compensation updatedCompensation) =>
        await _compensations.ReplaceOneAsync(x => x.Id == id, updatedCompensation);

    public async Task RemoveAsync(string id) =>
        await _compensations.DeleteOneAsync(x => x.Id == id);
}