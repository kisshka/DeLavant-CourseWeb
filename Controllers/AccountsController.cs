using DeLavant_CourseWeb.Models.UserBd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace DeLavant_CourseWeb.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var allUsers = _context.Users.Include(p => p.Posts).Include(a => a.Areas).ToList();
            var notAdminUsers = allUsers.Where(u => !_userManager.IsInRoleAsync(u,"Admin").Result).ToList();
            return View(notAdminUsers);
        }

        // GET: UserController/Details/5
        public ActionResult Details(string id)
        {
            var user = _context.Users
                .Include(p => p.Posts)
                .Include(a => a.Areas)
                .FirstOrDefault(u => u.Id == id);
            return View(user);
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
        public async Task<IActionResult> Create(RegisterInputViewModel inputUser)
        {
            if (ModelState.IsValid)
            {
                if (inputUser.SelectedAreaId == 0)
                {
                    inputUser.SelectedAreaId = null;
                }
                var user = new User
                {
                    UserSurName = inputUser.UserSurName,
                    Name = inputUser.Name,
                    UserName = inputUser.UserName,
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
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users
                .Include(p => p.Posts)
                .Include(a => a.Areas)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Маппируем данные пользователя в RegisterInputModel
            var editInputModel = new EditUserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                UserSurName = user.UserSurName,
                Name = user.Name,
                UserFatherName = user.UserFatherName,
                SelectedPostIds = user.Posts.Select(p => p.IdPost).Cast<int?>().ToList(),
                SelectedAreaId = user.IdArea ?? 0
            };

            ViewBag.Posts = _context.Posts.Select(p => new SelectListItem
            {
                Value = p.IdPost.ToString(),
                Text = p.Title
            }).OrderBy(p => p.Text).ToList();

            var areas = _context.Areas.OrderBy(a => a.NameArea).ToList();
            areas.Insert(0, new Area { IdArea = 0, NameArea = "Без участка" });
            ViewBag.Areas = new SelectList(areas, "IdArea", "NameArea");

            return View(editInputModel); // Передаем новую модель представления
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel editUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            var user = await _context.Users.Include(u => u.Posts).SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Обновляем поля пользователя из входящей модели
            user.UserName = editUser.UserName;
            user.UserSurName = editUser.UserSurName;
            user.Name = editUser.Name;
            user.UserFatherName = editUser.UserFatherName;
            user.Email = editUser.Email;
            if (editUser.SelectedAreaId == 0)
            {
                user.IdArea = null;
            }
            else
            {
                user.IdArea = editUser.SelectedAreaId;
            }


            // Удаляем старые связи с должностями и добавляем новые
            user.Posts.Clear();
            if (editUser.SelectedPostIds != null && editUser.SelectedPostIds.Any())
            {
                foreach (var postId in editUser.SelectedPostIds.Where(id => id.HasValue))
                {
                    var post = await _context.Posts.FindAsync(postId.Value);
                    if (post != null)
                    {
                        user.Posts.Add(post);
                    }
                }
            }

            

            // Сохраняем изменения
            await _context.SaveChangesAsync();
            ViewBag.Posts = _context.Posts.Select(p => new SelectListItem
            {
                Value = p.IdPost.ToString(),
                Text = p.Title
            }).OrderBy(p => p.Text).ToList();

            var areas = _context.Areas.OrderBy(a => a.NameArea).ToList();
            areas.Insert(0, new Area { IdArea = 0, NameArea = "Без участка" });
            ViewBag.Areas = new SelectList(areas, "IdArea", "NameArea");
            return RedirectToAction(nameof(Index)); // Перенаправление на страницу пользователей
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Генерация случайного логина и пароля
        private async Task<string> GenerateUniqueUsernameAsync()
        {
            while (true)
            {
                char firstChar = GetRandomLetter();
                List<char> restChars = Enumerable.Range(1, 7)
                                        .Select(_ => GetRandomChar())
                                        .ToList();
                int capitalPosition = Random.Shared.Next(restChars.Count);
                restChars[capitalPosition] = Char.ToUpper(restChars[capitalPosition]);
                var username = firstChar + new string(restChars.ToArray());
                bool exists = await _context.Users.AnyAsync(u => u.UserName == username);
                if (!exists)
                    return username;
            }
        }

        // Функция для выбора случайной буквы
        private static char GetRandomLetter()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return letters[Random.Shared.Next(letters.Length)];
        }

        // Функция для выбора случайного символа
        private static char GetRandomChar()
        {
            string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return allChars[Random.Shared.Next(allChars.Length)];
        }

        private static string GenerateSecurePassword(int length = 10)
        {
            Span<byte> randomBytes = stackalloc byte[length];
            RandomNumberGenerator.Fill(randomBytes);

            string basePassword = Convert.ToBase64String(randomBytes).Substring(0, length);

            string filteredPassword = new string(
                basePassword.Where(c => char.IsLetterOrDigit(c)).Take(length - 1).ToArray()
            );
            int specialSymbolPosition = Random.Shared.Next(filteredPassword.Length + 1);
            string finalPassword = filteredPassword.Insert(specialSymbolPosition, "!");

            return finalPassword;
        }

        [HttpPost]
        public async Task<IActionResult> Generate()
        {
            try
            {
                // Генерируем уникальное имя пользователя
                var uniqueUsername = await GenerateUniqueUsernameAsync();

                // Генерируем безопасный пароль
                var securePassword = GenerateSecurePassword();

                return Json(new
                {
                    success = true,
                    username = uniqueUsername,
                    password = securePassword
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}
