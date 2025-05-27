namespace DeLavant_CourseWeb.Models;

public class Question {
    public string? id;
    public string? Text;
    public List<Answer>? Answers { get; set; }
}