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

    }
}
