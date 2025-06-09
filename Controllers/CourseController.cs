using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class CourseController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly DeLavantContext _context;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ILogger<CourseController> logger, IMongoDatabase database, DeLavantContext context)
        {
            _database = database;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {   
            var coursesCollection = _database.GetCollection<Course>("Courses");
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

        public IActionResult AllCourses (string? accessType)
        { 
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var courses = coursesCollection.AsQueryable().AsEnumerable().Reverse().ToList();
            if(accessType != "All")
            {
            courses = coursesCollection.AsQueryable().Where(c => c.AccessTag == accessType).ToList();
            }

            return View(courses);
        }

        public IActionResult Access(string id, string? accessType)
        {   
            ViewBag.SelectedId = id;
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);
            var editedCourse = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();
            accessType = editedCourse.AccessTag;
            ViewBag.CurrentAccessType = accessType;
            return View();
        }
        [HttpPost]
        public IActionResult Access(string accessType)
        {
            switch(accessType)
            {
                case "Common":
                    return PartialView("_CommonAccess");
                    
                case "Group":
                    ViewBag.Posts = _context.Posts.Select(p => new SelectListItem
                    {
                        Value = p.IdPost.ToString(),
                        Text = p.Title
                    }).OrderBy(p => p.Text).ToList();
                    return PartialView("_GroupAccess");
                    
                case "Individual":
                    ViewBag.Users = _context.Users
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.UserSurName} {p.Name} {p.UserFatherName} {p.Email}"
                        }).AsEnumerable().OrderBy(p => p.Text).ToList();
                    return PartialView("_IndividualAccess");
                    
                case "Hidden":
                    return PartialView("_HiddenAccess");

                default:
                    return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Save(string SelectedAccessType, string id, string[] SelectedPostIds)
        {
            var coursesCollection = _database.GetCollection<Course>("Courses");
            var objectId = new ObjectId(id);

            var editedCourse = coursesCollection.Find(c => c.Id == objectId.ToString()).FirstOrDefault();

            if (editedCourse != null)
            {
                editedCourse.AccessTag = SelectedAccessType;

                if (SelectedAccessType == "Group")
                {
                    var posts = _context.Posts.Where(p => SelectedPostIds.Contains(p.IdPost.ToString())).Select(p => p.Title).ToList();
                    editedCourse.AccessDepartments = posts;
                    editedCourse.AccessUsers = null;
                }
                else if (SelectedAccessType == "Individual")
                {
                    var users = _context.Users.Where(u => SelectedPostIds.Contains(u.Id)).Select(u => $"{u.UserSurName} {u.Name} {u.UserFatherName} ({u.Email})").ToList();
                    editedCourse.AccessUsers = users;
                    editedCourse.AccessDepartments = null;
                }
                else
                {
                    editedCourse.AccessUsers = null;
                    editedCourse.AccessDepartments = null;
                }

                coursesCollection.ReplaceOne(c => c.Id == objectId.ToString(), editedCourse);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
