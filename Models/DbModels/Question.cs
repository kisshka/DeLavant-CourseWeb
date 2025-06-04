using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public enum QuestionType
{
    AnswerOptions,
    ExtendedResponse,
    Sequence
}
public class Question {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Text { get; set; }
    public QuestionType QuestionType { get; set; }

    //структуры для разных типов ответов
    public List<AnswerOption>? AnswerOptions { get; set; }
    public string? ExtendedResponse { get; set; }
    public List<Step>? StepsSequence { get; set; } 
    //баллы за вопрос
    public int Points { get; set; }

}