using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {

        private readonly DeLavantContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(DeLavantContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var user = _context.Users
                .Include(p => p.Posts)
                .Include(a => a.Areas).ToList();
            return View(user);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            ViewBag.Posts = _context.Posts.Select(p => new SelectListItem
            {
                Value = p.IdPost.ToString(),
                Text = p.Title
            }).OrderBy(p => p.Text).ToList();

            var areas = _context.Areas.OrderBy(a => a.NameArea).ToList();
            areas.Insert(0, new Area { IdArea = 0, NameArea = "Без участка" });
            ViewBag.Areas = new SelectList(areas, "IdArea", "NameArea");

            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterInputModel inputUser)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserSurName = inputUser.UserSurName,
                    Name = inputUser.UserName,
                    UserName = inputUser.Email,
                    UserFatherName = inputUser.UserFatherName,
                    Email = inputUser.Email,
                    IdArea = inputUser.SelectedAreaId
                };

                // Сохраняем пользователя
                var result = await _userManager.CreateAsync(user, inputUser.Password);

                if (result.Succeeded)
                {
                    if (inputUser.SelectedPostIds != null && inputUser.SelectedPostIds.Any())
                    {
                        foreach (var postId in inputUser.SelectedPostIds.Where(id => id.HasValue))
                        {
                            var post = await _context.Posts.FindAsync(postId.Value);
                            if (post != null)
                            {
                                post.Users ??= new List<User>();
                                post.Users.Add(user);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // В случае ошибки нужно вернуть пользователю представление с заполненными данными
            ViewBag.Posts = _context.Posts.Select(p => new SelectListItem
            {
                Value = p.IdPost.ToString(),
                Text = p.Title
            }).OrderBy(p => p.Text).ToList();

            var areas = _context.Areas.OrderBy(a => a.NameArea).ToList();
            areas.Insert(0, new Area { IdArea = 0, NameArea = "Без участка" });
            ViewBag.Areas = new SelectList(areas, "IdArea", "NameArea");

            return View(inputUser);
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
