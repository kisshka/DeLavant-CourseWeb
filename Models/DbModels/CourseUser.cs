using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class CourseUser
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? IdentityUserId { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronimyc { get; set; }
        public string? Email { get; set; }
        //Список отделов
        public string? Department { get; set; }
        public List<string>? Posts { get; set; }

        public List<CourseProgress>? CourseProgresses { get; set; }
    }