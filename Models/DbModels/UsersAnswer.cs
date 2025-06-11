namespace DeLavant_CourseWeb.Models;
public class UsersAnswer
    {
        public string? QuestionId { get; set; }

        //структуры для выбора ответов пользователем
        public List<string>? OneAnswerOptionsFromUser { get; set; }
        public List<string>? ManyAnswersOptionsFromUser { get; set; }
        public List<string>? StepsSequenceFromUser { get; set; } 
    }