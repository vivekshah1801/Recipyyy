using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Recipyyy.Models;

namespace Recipyyy.Controllers
{
    public class HomeController : Controller
    {
        private RecipyyyContext db = new RecipyyyContext();
        public ActionResult Index()
        {

            ViewBag.homeChef = db.chef.OrderByDescending(m => m.chefCreationDate).Take(4);
            ViewBag.homeRecipe = db.recipe.OrderByDescending(m => m.recipeCreationDate).Take(4);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            ViewBag.username = User.Identity.GetUserName();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}