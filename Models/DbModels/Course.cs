namespace DeLavant_CourseWeb.Models;

public class Course {
    public string? Id;
    public string? Name;
    public string? Description;
    public DateOnly? CreatingDate;
    // Публичный, индивидуальный, групповой 
    public string? AccessTag;
    //Список разрешенных пользователей для индивидуальных курсов
    public List<string>? AccessUsers { get; set; }
    //Список разрешенных отделов для групповых курсов
    public List<string>? AccessDepartments { get; set; }

    //Список лекций и тестов
    public List<Lecture>? Lectures { get; set; }
    public List<Test>? Tests { get; set; }
}