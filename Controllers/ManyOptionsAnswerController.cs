using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class ManyOptionsAnswerController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<ManyOptionsAnswerController> _logger;

        public ManyOptionsAnswerController(ILogger<ManyOptionsAnswerController> logger, IMongoDatabase database)
        {
            _database = database;
            _logger = logger;
        }

        public IActionResult Create(string testId, string courseId) {        
            ViewBag.testId = testId;
            ViewBag.courseId = courseId;
            return View();
        }

    }
}
