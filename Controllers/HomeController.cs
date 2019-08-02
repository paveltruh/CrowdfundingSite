using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UsersContext _usersContext;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
        }

        public IActionResult Index()
        {
            return View(_usersContext.Companies.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult View1()
        {
            return PartialView();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
