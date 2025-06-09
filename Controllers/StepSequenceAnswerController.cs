using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class StepSequenceAnswerController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<StepSequenceAnswerController> _logger;

        public StepSequenceAnswerController(ILogger<StepSequenceAnswerController> logger, IMongoDatabase database)
        {
            _database = database;
            _logger = logger;
        }

        public IActionResult Create(string testId, string courseId) {        
            ViewBag.testId = testId;
            ViewBag.courseId = courseId;
            return View();
        }

       [HttpPost]
        public async Task<IActionResult> Create(IFormCollection formCollection, string courseId, string testId)
        {
            try
            {
                // 1. Преобразовываем данные из формы в модель вопроса
                var questionText = formCollection["Text"]; // Тело вопроса

                // Множественные правильные ответы теперь приходят как строковые значения массива
                var stepsCount = formCollection.Keys.Count(k => k.StartsWith("AnswerOptions["));

                // 2. Сбор вариантов ответов
                var steps = new List<Step>();
                for (int i = 0; i < stepsCount; i++)
                {
                    var optionText = formCollection[$"AnswerOptions[{i}]"];

                    steps.Add(new Step
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Text = optionText,
                    });
                }

                // 3. Создание объекта Вопроса
                var newQuestion = new Question
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    courseId = courseId,
                    testId = testId,
                    Text = questionText,
                    StepsSequence = steps,
                    QuestionType = "Sequence",
                    Points = 1
                };

                // 4. Добавляем новый вопрос в коллекцию вопросов
                var questionsCollection = _database.GetCollection<Question>("Questions");
                await questionsCollection.InsertOneAsync(newQuestion);

                // 5. Получаем коллекции курсов и находим нужный курс
                var coursesCollection = _database.GetCollection<Course>("Courses");
                var course = await coursesCollection.Find(c => c.Id == courseId).FirstOrDefaultAsync();

                if (course == null)
                {
                    return NotFound();
                }

                // 6. Находим тест в рамках этого курса
                var targetTest = course.Tests.FirstOrDefault(t => t.Id == testId);

                if (targetTest == null)
                {
                    return NotFound();
                }

                // 7. Добавляем идентификатор нового вопроса в список вопросов теста
                if (targetTest.Questions == null)
                {
                    targetTest.Questions = new List<string>();
                }
                targetTest.Questions.Add(newQuestion.Id);

                // 8. Обновляем документ курса в базе данных
                await coursesCollection.ReplaceOneAsync(c => c.Id == courseId, course);

                TempData["SuccessMessage"] = "Вопрос успешно добавлен.";
                return RedirectToAction("Edit", "Tests", new { courseId = courseId, testId = testId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении вопроса.");
                ModelState.AddModelError("", "Ошибка при добавлении вопроса.");
                return View();
            }
        }
    }
}
