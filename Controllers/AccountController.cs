using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<User> userManager, ApplicationContext usersContext,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _applicationContext = usersContext;
            _configuration = configuration;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Companies(string id)
        {
            if (!await ValidationAsync(id))
                return RedirectToAction("Index", "Home");

            return View(_applicationContext.Companies
                .Where(c=>c.UserId == _applicationContext.Users.FirstOrDefault(u=>u.UserName.Equals(id)).Id));
        }
        public async Task<IActionResult> Bonuses(string id)
        {
            if (!await ValidationAsync(id))
                return RedirectToAction("Index", "Home");
            return View();
        }
        
        public async Task<IActionResult> EditProfile(string id)
        {
            if (!await ValidationAsync(id))
                return RedirectToAction("Index", "Home");

            User user = await _userManager.FindByNameAsync(id);
            if (user == null)
                return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            ChangeRoleViewModel model1 = new ChangeRoleViewModel
            {
                UserId = user.Id,
            };
            EditUserViewModel model = new EditUserViewModel { UserId = user.Id,
                Name = user.UserName, UserRoles = userRoles, AllRoles = allRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(EditUserViewModel model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    // получаем все роли
                    var allRoles = _roleManager.Roles.ToList();
                    // получаем список ролей, которые были добавлены
                    var addedRoles = roles.Except(userRoles);
                    // получаем роли, которые были удалены
                    var removedRoles = userRoles.Except(roles);

                    await _userManager.AddToRolesAsync(user, addedRoles);

                    await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    return RedirectToAction("EditProfile", new { id = model.Name }); ;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.UserName = model.Name;

                    user.Id = model.UserId;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        ViewData["UserName"] = model.Name;
                        return RedirectToAction("EditProfile", new { id = model.Name }); ;
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CreateCompany(string id)
        {
            if (! await ValidationAsync(id))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(string id,CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                DropBoxManager dropBoxManager = new DropBoxManager(_configuration["DropBoxAccessToken"]);
                string FotoUrl = await dropBoxManager.Upload(model.Name, model.Foto.FileName, model.Foto);



                Company user = new Company {
                    UserId = _userManager.Users.FirstOrDefault(u=>u.UserName.Equals(id)).Id,
                    Name = model.Name, Description = model.Description, Foto = FotoUrl,
                    Deadline = model.Deadline, TargetAmount = model.TargetAmount,
                    Category = model.Category
                };

                _applicationContext.Add(user);
                await _applicationContext.SaveChangesAsync();
                return RedirectToAction("Companies","Account",new { id });
            }
            return View(model);
        }

        private async Task<bool> ValidationAsync(string userName)
        {
            if (!_signInManager.IsSignedIn(User) || String.IsNullOrEmpty(userName) || await _userManager.FindByNameAsync(userName) == null ||
                (!User.Identity.Name.Equals(userName) && !User.IsInRole("admin")))
            {
                return false;
            }
            ViewData["UserName"] = userName;
            return true;
        }

    }
}