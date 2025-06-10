using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize]
    public class CourseBrowseController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly DeLavantContext _context;
        private readonly ILogger<CourseBrowseController> _logger;

        public CourseBrowseController(ILogger<CourseBrowseController> logger, IMongoDatabase database, DeLavantContext context)
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

        public IActionResult Start(string id)
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

         public IActionResult Passing (string id)
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
    }
}
