using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class CourseController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ILogger<CourseController> logger, IMongoDatabase database)
        {
            _database = database;
            _logger = logger;
        }

        public IActionResult Index()
        {   
            var coursesCollection = _database.GetCollection<Course>("Courses");
            // var courses = coursesCollection.Find(_ => true).ToList();
            var courses = coursesCollection.AsQueryable().ToList();
            return View(courses);
        }

//Добавление нового курса
      public IActionResult Create()
        {   
            return View();
        }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid)
        {
            return View(course);
        }

        try
        {
            course.CreatingDate = DateOnly.FromDateTime(DateTime.Today);
            course.AccessTag = "Hidden";
            var coursesCollection = _database.GetCollection<Course>("Courses");
            await coursesCollection.InsertOneAsync(course);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении курса.");
            ModelState.AddModelError("", "Возникла ошибка при создании курса.");
            return View(course);
        }
    }

    //Изменение курса и добавление в него разделов
    public IActionResult Edit(string id)
    {
        var coursesCollection = _database.GetCollection<Course>("Courses");
        var objectId = new ObjectId(id);
        var course = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();

        if (course != null)
        {
            var lectures = course.Lectures;
            var tests = course.Tests;

            return View(course);
        }
        else
        {
            return NotFound();
        }
    }

        [HttpPost]
        public IActionResult Edit(string id, Course course)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);
            var editedCourse = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return View(course);
            }
            editedCourse.Name = course.Name;
            editedCourse.Description = course.Description;
            coursesCollection.ReplaceOne(c => c.Id == objectId.ToString(), editedCourse);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddLection(string id)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);
            var editedCourse = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();

                var newLecture = new Lecture
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Новая лекция",
                    Description = "Описание" 
                };

            if (editedCourse.Lectures == null)
            {
                editedCourse.Lectures = new List<Lecture>();
            }
           editedCourse.Lectures.Add(newLecture);

                coursesCollection.UpdateOne(
                c => c.Id == objectId.ToString(),
                Builders<Course>.Update.Set(c => c.Lectures, editedCourse.Lectures)
            );

            return RedirectToAction(nameof(Edit), new { id });
        }

        public IActionResult AddTest(string id)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);
            var editedCourse = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();

                var newTest = new Test
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Новый тест",
                    Description = "Описание" 
                };

            if (editedCourse.Tests == null)
            {
                editedCourse.Tests = new List<Test>();
            }
           editedCourse.Tests.Add(newTest);

                coursesCollection.UpdateOne(
                c => c.Id == objectId.ToString(),
                Builders<Course>.Update.Set(c => c.Tests, editedCourse.Tests)
            );

            return RedirectToAction(nameof(Edit), new { id });
        }


        public IActionResult Access()
        {   
            return View();
        }

    }
}
