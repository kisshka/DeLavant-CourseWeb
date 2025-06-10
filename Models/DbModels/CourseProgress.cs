using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class CourseProgress {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? CourseId { get; set; }
    public int TotalSteps { get; set; } // Всего шагов в курсе
    public int CompletedSteps { get; set; } // Сколько шагов уже выполнено
    public List<string>? TestProgresses { get; set; } // Прогресс по каждому тесту

}