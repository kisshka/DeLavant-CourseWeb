using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize(Roles = "Admin")]
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
                Description = lecture.Description
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

    //Работа с файлом  
    if (lectureIn.LectureFile != null)
    {
        //Сначала происходит удаление старого файла
        string oldFileNameFullPath = environment.WebRootPath + $"/{editedLecture.FileType}/" + editedLecture.FileName;
        if (System.IO.File.Exists(oldFileNameFullPath))
            {
                System.IO.File.Delete(oldFileNameFullPath);
            }
        //далее определяется тип нового файла
        string? folder;
        switch(Path.GetExtension(lectureIn.LectureFile.FileName))
        {
            case ".pdf":
                folder = "Pdf";
                editedLecture.FileType = "Pdf";
            break;
            case ".docx":
                folder = "Word";
                editedLecture.FileType = "Word";
            break;
            case ".doc":
                folder = "Word";
                editedLecture.FileType = "Word";                
            break;
            case ".mp4":
                folder = "Mp4";              
                editedLecture.FileType = "Mp4";                
            break;
            case ".pptx":
                folder = "Pptx";   
                editedLecture.FileType = "Pptx";                           
            break;
            default:
                return View(lectureIn);
        }
        //присваивается новое имя
        newFileName = Guid.NewGuid().ToString() + Path.GetExtension(lectureIn.LectureFile.FileName);
        string imageFullPath = environment.WebRootPath + $"/{folder}/" + newFileName;

        // Копируем файл на сервер
        using (var fileStream = new FileStream(imageFullPath, FileMode.Create))
        {
            lectureIn.LectureFile.CopyTo(fileStream);
        }

    }

            editedLecture.Name = lectureIn.Name;
            editedLecture.Description = lectureIn.Description;
            editedLecture.FileName = newFileName;

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
            var deletedLecture = course.Lectures.FirstOrDefault(l => l.Id == lectureId);

        string? folderForDelete = GetFolderForDelete(deletedLecture.FileType);
            course.Lectures.RemoveAt(indexOfLecture);
            string oldFileNameFullPath = environment.WebRootPath + $"/{folderForDelete}/" + deletedLecture.FileName;
            if (System.IO.File.Exists(oldFileNameFullPath))
            {
                System.IO.File.Delete(oldFileNameFullPath);
            }

        // Обновляем коллекцию в базе данных
        coursesCollection.UpdateOne(
            c => c.Id == courseId,
            Builders<Course>.Update.Set(c => c.Lectures, course.Lectures)
        );

            return RedirectToAction(nameof(Edit), "Course" , new { id=courseId}); // Перенаправляем на страницу списка курсов
        }

// определяет папку для удаления по типу
        public string GetFolderForDelete (string? fileType) {
            switch(fileType)
                {
                    case "Pdf":
                        return "Pdf";
                    case "Word":
                       return "Word";
                    case "Mp4":
                        return "Mp4";                            
                    case "Pptx":
                        return "Pptx";                          
                    default:
                        return "None";
                }
        }

    }
}