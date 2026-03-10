using DirectoryService.Models;
using MongoDB.Driver;

namespace DirectoryService.Services;

public class EmployeeService
{
    private readonly IMongoCollection<Employee> _employees;

    public EmployeeService(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var databaseName = configuration["MongoDbSettings:DatabaseName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        
        _employees = database.GetCollection<Employee>("Employees");
    }

    public async Task<List<Employee>> GetAsync() =>
        await _employees.Find(_ => true).ToListAsync();

    public async Task<Employee?> GetAsync(string id) =>
        await _employees.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Employee newEmployee) =>
        await _employees.InsertOneAsync(newEmployee);

    public async Task UpdateAsync(string id, Employee updatedEmployee) =>
        await _employees.ReplaceOneAsync(x => x.Id == id, updatedEmployee);

    public async Task RemoveAsync(string id) =>
        await _employees.DeleteOneAsync(x => x.Id == id);
}