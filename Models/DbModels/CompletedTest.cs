namespace DeLavant_CourseWeb.Models;

public class CompletedTest {
    public string? Id;
    public string? Result;
    public List<UsersAnswer>? UsersAnswers { get; set; }
}