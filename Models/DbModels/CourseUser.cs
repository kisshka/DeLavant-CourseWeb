namespace DeLavant_CourseWeb.Models;

public class CourseUser
    {
        public string? Id;
        public string? Surname;        
        public string? Name;
        public string? Patronimyc;
        public string? Email;
        public string? Department;
        public string? Post;
        public string? CourseId;
        public List<CompletedTest>? CompletedTests { get; set; }      
    }