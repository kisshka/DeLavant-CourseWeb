using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class Question {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Text { get; set; }
    public List<Answer>? Answers { get; set; }
}