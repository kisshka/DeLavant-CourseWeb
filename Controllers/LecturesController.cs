using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class LecturesController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IMongoDatabase _database;
        private readonly ILogger<LecturesController> _logger;

        public LecturesController(ILogger<LecturesController> logger, IMongoDatabase database, IWebHostEnvironment environment)
        {
            _database = database;
            _logger = logger;
            this.environment = environment;
        }
        
//изменение лекции
        public IActionResult Edit(string courseId, string lectureId)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectCourseId = new ObjectId(courseId);
            var objectLectureId = new ObjectId(lectureId);
            var lecture = coursesCollection.Find(c => c.Id == objectCourseId.ToString()).FirstOrDefault().Lectures.Find(c => c.Id == objectLectureId.ToString());


            var lectureIn = new LectureIn
            {
                Id = lecture.Id,
                Name = lecture.Name,
                Description = lecture.Description,
                FileType = lecture.FileType
            };
            ViewBag.FileName = lecture.FileName;
            ViewBag.courseIdForLection = courseId;
            if (lecture != null)
            {
                return View(lectureIn);
            }
            else
            {
                return NotFound();
            }
        }

    [HttpPost]
    public async Task<IActionResult> Edit(string courseId, string lectureId, LectureIn lectureIn)
    {
        var coursesCollection = _database.GetCollection<Course>("Courses");
        var course = coursesCollection.Find(c => c.Id == courseId).FirstOrDefault();
        var editedLecture = course.Lectures.FirstOrDefault(l => l.Id == lectureId);

        if (editedLecture == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(lectureIn); 
        }

        string? newFileName = editedLecture.FileName;
    if (lectureIn.LectureFile != null)
    {

        newFileName = Guid.NewGuid().ToString() + Path.GetExtension(lectureIn.LectureFile.FileName);

        string imageFullPath = environment.WebRootPath + "/Pdf/" + newFileName;

        // Копируем файл на сервер
        using (var fileStream = new FileStream(imageFullPath, FileMode.Create))
        {
            lectureIn.LectureFile.CopyTo(fileStream);
        }

            editedLecture.Name = lectureIn.Name;
            editedLecture.Description = lectureIn.Description;
            editedLecture.FileType = lectureIn.FileType;
            editedLecture.FileName = newFileName;
        }

        // Проверка валидности модели


        coursesCollection.UpdateOne(
            c => c.Id == courseId,
            Builders<Course>.Update.Set(c => c.Lectures, course.Lectures)
        );

        return RedirectToAction(nameof(Edit), "Course", new { id = courseId });
    }
// Удаление

       public IActionResult Delete (string courseId, string lectureId)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var course = coursesCollection.Find(c => c.Id == courseId).FirstOrDefault();
            var indexOfLecture = course.Lectures.FindIndex(l => l.Id == lectureId);
            
            course.Lectures.RemoveAt(indexOfLecture);

        // Обновляем коллекцию в базе данных
        coursesCollection.UpdateOne(
            c => c.Id == courseId,
            Builders<Course>.Update.Set(c => c.Lectures, course.Lectures)
        );

            return RedirectToAction(nameof(Edit), "Course" , new { id=courseId}); // Перенаправляем на страницу списка курсов
        }

    }
}