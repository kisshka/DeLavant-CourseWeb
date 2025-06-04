using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class AnswerOption {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Text { get; set; }
    bool? IsTrue { get; set; }
}