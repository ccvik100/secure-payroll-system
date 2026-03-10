using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DirectoryService.Models;

public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string Position { get; set; } = null!;
}