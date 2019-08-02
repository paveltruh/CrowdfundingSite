using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class CompanyController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly UsersContext _usersContext;

        public CompanyController(UserManager<User> userManager,  UsersContext usersContext)
        {
            _userManager = userManager;
            _usersContext = usersContext;
        }

        public IActionResult Index(string id)
        {

            Company company = _usersContext.Companies.FirstOrDefault(c => c.Name.Equals(id));
            company.News = _usersContext.News.Where(n => n.CompanyId==company.Id).ToList();

            company.Description = Markdig.Markdown.ToHtml(company.Description);
            foreach (var news in company.News)
            {
                news.Text = Markdig.Markdown.ToHtml(news.Text);
            }
            return View(company);
        }


        public IActionResult News() {
            return PartialView("News");
        }

        public IActionResult AddNews(string companyName)
        {
            Company company = _usersContext.Companies.FirstOrDefault(c => c.Name.Equals(companyName));
            if (company==null || !Validation(company.UserId))
                return RedirectToAction("Index", "Home");
            CreateNewsViewModel model = new CreateNewsViewModel { CompanyId = company.Id, CompanyName = company.Name };
            return View(model);
        }

        private bool Validation(string OwnerId)
        {
            if (User.IsInRole("admin") || _userManager.GetUserId(User).Equals(OwnerId))
                return true;
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(CreateNewsViewModel model)
        {
            if (ModelState.IsValid)
            {

                DropBoxManager dropBoxManager = new DropBoxManager();
                string ImageUrl = await dropBoxManager.Upload($"{model.CompanyName}/news", model.Image.FileName, model.Image);

                News news = new News
                {
                    Text = model.Text,
                    CompanyId = model.CompanyId,
                    Heading = model.Heading,
                    Image = ImageUrl
                };
                _usersContext.News.Add(news);
                await _usersContext.SaveChangesAsync();
                return RedirectToAction("Index", "Company", new { id = model.CompanyName });
            }
            return View(model);
        }
    }
}