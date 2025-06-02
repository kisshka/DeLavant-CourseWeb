using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class CourseUser
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronimyc { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Post { get; set; }
        public string? CourseId { get; set; }
        public List<CompletedTest>? CompletedTests { get; set; }      
    }