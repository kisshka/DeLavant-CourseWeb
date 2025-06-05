using System.Diagnostics;
using DeLavant_CourseWeb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    public class ExtendedResponseAnswerController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<ExtendedResponseAnswerController> _logger;

        public ExtendedResponseAnswerController(ILogger<ExtendedResponseAnswerController> logger, IMongoDatabase database)
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
