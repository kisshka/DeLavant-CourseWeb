using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
namespace DeLavant_CourseWeb.Models;
// Типы:
//     OneOption,
//     ManyOptions,
//     ExtendedResponse,
//     Sequence

public class Question {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? courseId { get; set; }
    public string? testId { get; set; }

    [Display(Name = "Текст вопроса")]
    public string? Text { get; set; }
    public string? QuestionType { get; set; }

    //структуры для разных типов ответов
    public List<AnswerOption>? OneAnswerOptions { get; set; }
    public List<AnswerOption>? ManyAnswersOptions { get; set; }
    public string? ExtendedResponse { get; set; }
    public List<Step>? StepsSequence { get; set; } 
    //баллы за вопрос
    public int Points { get; set; }

}