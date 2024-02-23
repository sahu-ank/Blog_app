using Blog_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_app.Controllers
{
    public class AuthController : Controller
    {
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, UserName = "admin", Password = "admin", Role = "hr" },
            new User { Id = 2, UserName = "user", Password = "user", Role = "employee" }
        };
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public IActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = _users.Find(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetString("Role", user.Role);


                    return RedirectToAction("Index", "Home");
                }
                return BadRequest("Invailid attempt");
            }
            return View(model);
        }
    }

}
