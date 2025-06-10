using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize]
    public class DivisionController : Controller
    {
        // GET: DivisionController
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Course");
            }
            else
            {
                return RedirectToAction("Index", "CourseBrowse");
            }
        }
    }
}
