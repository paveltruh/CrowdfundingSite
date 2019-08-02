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
        private readonly SignInManager<User> _signInManager;
        private readonly UsersContext _usersContext;

        public CompanyController(UserManager<User> userManager, SignInManager<User> signInManager, UsersContext usersContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersContext = usersContext;
        }

        public IActionResult Index(string id)
        {

            Company company = _usersContext.Companies.FirstOrDefault(c => c.Name.Equals(id));
            company.News = _usersContext.News.Where(n => n.CompanyId==company.Id).OrderByDescending(n=>n.Date).ToList();
            ViewData["IsItTheOwner"] = IsItTheOwner(company.UserId);
            ViewData["IsSignedIn"] = _signInManager.IsSignedIn(User);



            company.Description = Markdig.Markdown.ToHtml(company.Description);
            foreach (var news in company.News)
                news.Text = Markdig.Markdown.ToHtml(news.Text);
            return View(company);
        }

        public IActionResult DonateToCompany(int CompanyId, string CompanyName)
        {
            if (!_signInManager.IsSignedIn(User) || !_usersContext.Companies.Any(c=>c.Id==CompanyId))
                return RedirectToAction("Index", "Home");
            CreateDonationViewModel model = new CreateDonationViewModel {
                CompanyId =CompanyId,
                UserId = _userManager.GetUserId(User),
                CompanyName = CompanyName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DonateToCompany(CreateDonationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Donation donation = new Donation
                {
                    CompanyId = model.CompanyId,
                    UserId = model.UserId,
                    AmountOfDonation = model.AmountOfDonation
                };
                Company company = _usersContext.Companies.FirstOrDefault(c=>c.Id == model.CompanyId);
                company.CollectedAmount += model.AmountOfDonation;

                _usersContext.Companies.Update(company);
                _usersContext.Donations.Add(donation);
                await _usersContext.SaveChangesAsync();
                return RedirectToAction("Index", "Company", new { id = model.CompanyName });
            }
            return View(model);
        }

        public IActionResult News() {
            return PartialView("News");
        }

        public IActionResult AddNews(string companyName)
        {
            Company company = _usersContext.Companies.FirstOrDefault(c => c.Name.Equals(companyName));
            if (company==null || !IsItTheOwner(company.UserId))
                return RedirectToAction("Index", "Home");
            CreateNewsViewModel model = new CreateNewsViewModel { CompanyId = company.Id, CompanyName = company.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(CreateNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                string ImageUrl="";
                if (model.Image!=null) {
                    DropBoxManager dropBoxManager = new DropBoxManager();
                    ImageUrl = await dropBoxManager.Upload($"{model.CompanyName}/news", model.Image.FileName, model.Image);
                }

                News news = new News
                {
                    Text = model.Text,
                    CompanyId = model.CompanyId,
                    Heading = model.Heading,
                    Image = ImageUrl,
                    Date = DateTime.Now
                };
                _usersContext.News.Add(news);
                await _usersContext.SaveChangesAsync();
                return RedirectToAction("Index", "Company", new { id = model.CompanyName });
            }
            return View(model);
        }

        private bool IsItTheOwner(string userId)
        {
            if (_signInManager.IsSignedIn(User) && (User.IsInRole("admin") || _userManager.GetUserId(User).Equals(userId)))
                return true;
            return false;
        }
    }
}