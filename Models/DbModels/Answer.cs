using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class Answer
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Text { get; set; }
        public bool IsRight { get; set; }
    }