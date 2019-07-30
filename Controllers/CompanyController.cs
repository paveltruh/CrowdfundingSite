using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CompanyController : Controller
    {
        private readonly UsersContext _usersContext;

        public CompanyController( UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public IActionResult Index(string id)
        {
            Company company = _usersContext.Companies.FirstOrDefault(c => c.Name.Equals(id));
            return View(company);
        }
    }
}