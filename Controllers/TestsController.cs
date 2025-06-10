using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestsController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<TestsController> _logger;

        public TestsController(ILogger<TestsController> logger, IMongoDatabase database)
        {
            _database = database;
            _logger = logger;
        }
        
    // редактирование теста
        public IActionResult Edit(string courseId, string testId)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectCourseId = new ObjectId(courseId);
            var objectTestId = new ObjectId(testId);
            var test = coursesCollection.Find(c => c.Id == objectCourseId.ToString()).FirstOrDefault().Tests.Find(c => c.Id == objectTestId.ToString());

        var questionCollection = _database.GetCollection<Question>("Questions");

        // Проверка на null и инициализация коллекции
        if (test.Questions == null)
        {
            test.Questions = new List<string>();
        }
    //Выборка вопросов, относящихся к тесту!
        var questions = questionCollection.AsQueryable().ToList();
        List<Question> questionsForTest = new List<Question>();
        foreach (Question question in questions) {
            if (question.testId == testId && question.courseId == courseId){
                questionsForTest.Add(question);
            }
        }
        ViewBag.questions = questionsForTest;

            ViewBag.courseIdForTest = courseId;
            if (test != null)
            {
                return View(test);
            }
            else
            {
                return NotFound();
            }
        }

    // редактирование теста
    [HttpPost]
    public IActionResult Edit(string courseId, string testId, Test test)
    {
        var coursesCollection = _database.GetCollection<Course>("Courses");
        var course = coursesCollection.Find(c => c.Id == courseId).FirstOrDefault();

        if (course == null)
        {
            return NotFound(); // Курса с таким идентификатором не существует
        }

        // Находим лекцию в курсе
        var editedTest = course.Tests.FirstOrDefault(l => l.Id ==  testId);

        if (editedTest == null)
        {
            return NotFound(); // Лекции с таким идентификатором не существует
        }

        editedTest.Name = test.Name;
        editedTest.Description = test.Description;

        // Проверка валидности модели
        if (!ModelState.IsValid)
        {
            return View(editedTest); // Вернём представление с лекцией и сообщением об ошибке
        }

        // Обновляем коллекцию в базе данных
        coursesCollection.UpdateOne(
            c => c.Id == courseId,
            Builders<Course>.Update.Set(c => c.Tests, course.Tests)
        );

        return RedirectToAction(nameof(Edit), "Course" , new { id=courseId});
    }
    // Удаление теста
       public IActionResult Delete (string courseId, string testId)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var course = coursesCollection.Find(c => c.Id == courseId).FirstOrDefault();
            var indexOfTest = course.Tests.FindIndex(l => l.Id == testId);
            
            course.Tests.RemoveAt(indexOfTest);

        // Обновляем коллекцию в базе данных
        coursesCollection.UpdateOne(
            c => c.Id == courseId,
            Builders<Course>.Update.Set(c => c.Tests, course.Tests)
        );

            return RedirectToAction(nameof(Edit), "Course" , new { id=courseId}); // Перенаправляем на страницу списка курсов
        }

        public IActionResult DeleteQuestion (string questionId)
        {
            var questionCollection = _database.GetCollection<Question>("Questions");
            var question = questionCollection.Find(c => c.Id == questionId).FirstOrDefault();
            questionCollection.DeleteOne(q => q.Id == questionId);
        // Обновляем коллекцию в базе данных

            return RedirectToAction(nameof(Edit), "Tests" , new { courseId=question.courseId, testId = question.testId}); // Перенаправляем на страницу списка курсов
        }

    }
}