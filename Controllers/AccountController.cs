using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private UsersContext _usersContext;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
        }

        public IActionResult Index(string id)
        {
            if (!Validation(id))
                return RedirectToAction("Index", "Home");
            return View();
        }
        public IActionResult Companies(string id)
        {
            //IQueryable<User> users = null; _companyContext.Companies.i);
            //_companyContext.Companies.Select()
            if (!Validation(id))
                return RedirectToAction("Index", "Home");

            //IQueryable<Company> companies = _usersContext.Companies.ToList();
            return View(_usersContext.Companies
                .Where(c=>c.UserId == _usersContext.Users.FirstOrDefault(u=>u.UserName.Equals(id)).Id));
        }
        public IActionResult Bonuses(string id)
        {
            if (!Validation(id))
                return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult CreateCompany(string id)
        {
            if (!Validation(id))
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(string id,CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company user = new Company { UserId = _userManager.GetUserId(User), Name = model.Name, Description = model.Description  };
                _usersContext.Add(user);
                await _usersContext.SaveChangesAsync();
                return RedirectToAction("Companies","Account",new { id });
            }
            return View(model);
        }

        private bool Validation(string userName)
        {
            if (String.IsNullOrEmpty(userName) || (!User.Identity.Name.Equals(userName) && !User.IsInRole("admin")))
            {
                return false;
            }
            ViewData["UserName"] = userName;
            return true;
        }
    }
}