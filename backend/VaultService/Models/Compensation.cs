using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VaultService.Models;

public class Compensation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Ezt kötjük össze a DirectoryService-ben lévő dolgozóval
    public string EmployeeId { get; set; } = null!;

    public string TaxNumber { get; set; } = null!; // Adóazonosító
    public string BankAccountNumber { get; set; } = null!;
    public decimal GrossBaseSalary { get; set; }
    public string Currency { get; set; } = "HUF";
}