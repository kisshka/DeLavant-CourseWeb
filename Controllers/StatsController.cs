using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Authorization;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly DeLavantContext _context;

        public StatsController(DeLavantContext context)
        {
            _context = context;
        }

        // GET: Posts
        public IActionResult Index()
        {
            return View( );
        }
    }
}