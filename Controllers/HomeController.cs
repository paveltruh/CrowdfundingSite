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
        private readonly ApplicationContext _ApplicationContext;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext ApplicationContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ApplicationContext = ApplicationContext;
        }

        public IActionResult Index()
        {
            return View(_ApplicationContext.Companies.ToList());
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
