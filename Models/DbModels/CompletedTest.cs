using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class CompletedTest {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? CourseId { get; set; }
    public string? TestId { get; set; }
    public string? Result { get; set; }
    public string? AttemptsNumber { get; set; }
    public List<UsersAnswer>? UsersAnswers { get; set; }
}