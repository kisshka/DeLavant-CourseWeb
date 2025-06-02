using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class Course {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id;
    public string? Name { get; set; }
    public string? Description  { get; set; }
    public DateOnly? CreatingDate  { get; set; }
    // Публичный, индивидуальный, групповой 
    public string? AccessTag  { get; set; }
    //Список разрешенных пользователей для индивидуальных курсов
    public List<string>? AccessUsers { get; set; }
    //Список разрешенных отделов для групповых курсов
    public List<string>? AccessDepartments { get; set; }

    //Список лекций и тестов
    public List<Lecture>? Lectures { get; set; }
    public List<Test>? Tests { get; set; }
    public List<CourseElementOrder>? CourseElementOrders { get; set; }
}