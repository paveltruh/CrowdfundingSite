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
        private readonly UsersContext _usersContext;

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
            if (!Validation(id))
                return RedirectToAction("Index", "Home");

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
                DropBoxManager dropBoxManager = new DropBoxManager();
                string FotoUrl = await dropBoxManager.Upload(model.Name, model.Foto.FileName, model.Foto);

                Company user = new Company {
                    UserId = _userManager.Users.FirstOrDefault(u=>u.UserName.Equals(id)).Id,
                    Name = model.Name, Description = model.Description, Foto = FotoUrl,
                    Deadline = model.Deadline, TargetAmount = model.TargetAmount };

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