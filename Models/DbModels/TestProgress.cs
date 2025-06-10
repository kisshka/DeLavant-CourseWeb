using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class TestProgress
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? CourseProgressId { get; set; }
    public string? TestId { get; set; }
    public int TotalSteps { get; set; } // Количество шагов в тестеы
    public int CompletedSteps { get; set; } // Пройденные шаги в тесте
    public string? Result { get; set; } // Итоговый результат теста
    public string? AttemptsNumber { get; set; } // Число попыток сдачи теста
    public List<UsersAnswer>? UsersAnswers { get; set; } // Ответы пользователя
}