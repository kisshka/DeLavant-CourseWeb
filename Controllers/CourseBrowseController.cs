using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class CourseBrowseController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly DeLavantContext _context;
        private readonly UserManager<User> _userManager;

        public CourseBrowseController(IMongoDatabase database, DeLavantContext context, UserManager<User> userManager)
        {
            _database = database;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {   
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var courses = coursesCollection.AsQueryable().ToList();
            return View(courses);
        }

//Страница с названием и описанием теста, отсюда начинается прохождение
        public async Task <IActionResult> Start(string id)
        {   
            var usersCollection = _database.GetCollection<CourseUser>("Users");
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);
            var course = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();
            
            var identityUser = _userManager.GetUserAsync(User).Result;
            var user = await usersCollection.Find(u => u.IdentityUserId == identityUser.Id).FirstOrDefaultAsync();

            if (user != null) {
                var courseProgress = user.CourseProgresses?.FirstOrDefault(cp => cp.CourseId == id);
                    if (courseProgress == null) {
                        ViewBag.Progress = null;                
                    }
                    else ViewBag.Progress = courseProgress.CompletedSteps;
            }
            else {
                ViewBag.Progress = null;
            }

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


//ОТКРЫВАЕТ КУРС С САМОГО НАЧАЛА 
    public async Task<IActionResult> OpenCourse(string courseId)
    {
        var coursesCollection = _database.GetCollection<Course>("Courses");
        var usersCollection = _database.GetCollection<CourseUser>("Users");

        var identityUser = _userManager.GetUserAsync(User).Result;

        var course = await coursesCollection.Find(c => c.Id == courseId).FirstOrDefaultAsync();
        if (course == null)
        {
            return NotFound();
        }

        var user = await usersCollection.Find(u => u.IdentityUserId == identityUser.Id).FirstOrDefaultAsync();

        // Создание нового пользователя, если он отсутствует
        if (user == null)
        {
            user = new CourseUser
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdentityUserId = identityUser.Id,
                Surname = identityUser.UserSurName,
                Name = identityUser.Name,
                Patronimyc = identityUser.UserFatherName,
                Email = identityUser.Email,
                CourseProgresses = new List<CourseProgress>()
            };
            usersCollection.InsertOneAsync(user);
        }

        // Проверка наличия прогресса по курсу
        var courseProgress = user.CourseProgresses?.FirstOrDefault(cp => cp.CourseId == courseId);

        // Создание прогресса, если его нет
        if (courseProgress == null)
        {
            courseProgress = new CourseProgress
            {
                CourseId = courseId,
                TotalSteps = course.Lectures.Count + course.Tests.Count,
                CompletedSteps = 0
            };
            user.CourseProgresses.Add(courseProgress);
            await usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        //При повторном прохождении курса прогресс обнуляется
        courseProgress.CompletedSteps = 0;
            await usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        // Открываем первый шаг
        return RedirectToAction("ViewStep", new { courseId, userId = user.Id, step = 0 });
    }

//ФУНКЦИЯ КОТОРАЯ ПЕРЕНАПРАВЯЕТ ПОЛЬЗОВАТЕЛЯ НА ПОСЛЕДНИЙ СЕГМЕНТ КУРСА КОТОРЫЙ ОН ПРОХОДИТ
    public async Task<IActionResult> ContinueCourse(string courseId)
    {
        var coursesCollection = _database.GetCollection<Course>("Courses");
        var usersCollection = _database.GetCollection<CourseUser>("Users");

        var identityUser = _userManager.GetUserAsync(User).Result;

        var course = await coursesCollection.Find(c => c.Id == courseId).FirstOrDefaultAsync();
        if (course == null)
        {
            return NotFound();
        }

        var user = await usersCollection.Find(u => u.IdentityUserId == identityUser.Id).FirstOrDefaultAsync();

        // Проверка наличия прогресса по курсу
        var courseProgress = user.CourseProgresses?.FirstOrDefault(cp => cp.CourseId == courseId);

        if (courseProgress == null || courseProgress.CompletedSteps == 0)
        {
            // Если прогресса нет или он пустой, начинаем с первого шага
            return RedirectToAction("ViewStep", new { courseId, userId = user.Id, step = 0 });
        }
        else
        {
            // Открываем следующий шаг после последнего пройденного
            return RedirectToAction("ViewStep", new { courseId, userId = user.Id, step = courseProgress.CompletedSteps });
        }
    }

//ДЕМОНСТРАЦИЯ ТЕКУЩЕГО ШАГА КУРСА
public async Task<IActionResult> ViewStep(string courseId, int step)
{
    var coursesCollection = _database.GetCollection<Course>("Courses");
    var usersCollection = _database.GetCollection<CourseUser>("Users");
    var identityUser = _userManager.GetUserAsync(User).Result;

    var course = await coursesCollection.Find(c => c.Id == courseId).FirstOrDefaultAsync();
    if (course == null)
        return NotFound();

    var user = await usersCollection.Find(u => u.IdentityUserId == identityUser.Id).FirstOrDefaultAsync();
    var courseProgress = user.CourseProgresses?.FirstOrDefault(cp => cp.CourseId == courseId);

    if (courseProgress == null || step > courseProgress.CompletedSteps)
        return Forbid(); // Защита от обхода порядка шагов

    // Проверка на конец курса
    var totalSteps = course.Lectures.Count + course.Tests.Count;
    if (step >= totalSteps)
        return RedirectToAction("Final", new { courseId, identityUser.Id });

    // Определяем, какой элемент показать (лекция или тест)
    var isLecture = step < course.Lectures.Count;

    // Передача в представление
    ViewBag.courseId = courseId;
    ViewBag.step = step;

    if (isLecture)
    {
        var lecture = course.Lectures[step];
        return View("Lecture", lecture);
    }
    else
    {
        var test = course.Tests[step - course.Lectures.Count];
        return View("Test", test);
    }
}

// Завершение шага
public async Task<IActionResult> FinishStep(string courseId, int step)
{
    var usersCollection = _database.GetCollection<CourseUser>("Users");
    var identityUser = _userManager.GetUserAsync(User).Result;

    var user = await usersCollection.Find(u => u.IdentityUserId == identityUser.Id).FirstOrDefaultAsync();
    var courseProgress = user.CourseProgresses?.FirstOrDefault(cp => cp.CourseId == courseId);

    if (courseProgress == null)
        return NotFound();

    // Только здесь увеличиваем прогресс
    courseProgress.CompletedSteps++;
    await usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

    // Идём на следующий шаг
    return RedirectToAction("ViewStep", new { courseId, step = step + 1 });
}

    //Завершение курса
        public IActionResult Final(string courseId, string userId)
        {
            return View();
        }
    }
}
