using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Recipyyy.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Recipyyy.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private RecipyyyContext db = new RecipyyyContext();

        
        // GET: Recipes
        public ActionResult Index()
        {
            string currentUserName = User.Identity.GetUserName();
            int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
            var recipe = db.recipe.Include(r => r.chef).Include(r => r.cusine).Where(m => (m.recipeIsPrivate == false) || ((m.recipeIsPrivate == true) && (m.chefId == currentChefId)));
            
            return View(recipe.ToList());
        }

        // GET: Recipes/MyRecipes
        public ActionResult MyRecipes()
        {
            string currentUserName = User.Identity.GetUserName();
            int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
            var recipe = db.recipe.Where(m => m.chefId == currentChefId).Include(r => r.chef).Include(r => r.cusine);
            return View(recipe.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = db.recipe.Include(m => m.chef).Include(m => m.cusine).First(m=> m.recipeId == id);

            string currentUserName = User.Identity.GetUserName();
            int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
            if (recipe.chefId == currentChefId)
                ViewBag.showEdit = 1;
            else
            {
                ViewBag.showEdit = 0;
            }
            
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin == true)
                ViewBag.showDelete = 1;
            else
            {
                ViewBag.showDelete = 0;
            }

            if (recipe == null)
            {
                return HttpNotFound();
            }

            ViewBag.comments = db.comment.OrderByDescending(m => m.commentCreationTime).Include(m => m.chef).Include(m => m.recipe).Where(m => m.recipe.recipeId == id);
            
            ViewBag.recdRecipes = db.recipe.Include(m => m.cusine).Include(m => m.chef).Where(m => (m.recipeIsPrivate == false) && (m.chefId == currentChefId) && (m.recipeId != recipe.recipeId));

            ViewBag.currentChefId = currentChefId;
            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            ViewBag.chefId = new SelectList(db.chef, "chefId", "chefName", "chefName");
            ViewBag.cuisineId = new SelectList(db.cuisine, "cuisineId", "cuisineName");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "recipeId,recipeTitle,recipeDescription,recipeIngredients,recipeDirection,recipeServesFor,recipeType,recipeNutritionFacts,recipeIsPrivate,recipePrepTime,recipeTags,recipeImageFile,cuisineId")] Recipe recipe)
        {
            
            if (ModelState.IsValid)
            {
                if (recipe.recipeImageFile == null)
                {
                    recipe.recipeImagePath = "~/Content/RecipeImages/" + "R-" + "Dummy" + ".jpg";
                }
                else
                {
                    string extension = Path.GetExtension(recipe.recipeImageFile.FileName);

                    string fileName = "R-" + recipe.recipeTitle + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    recipe.recipeImagePath = "~/Content/RecipeImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/RecipeImages/"), fileName);
                    recipe.recipeImageFile.SaveAs(fileName);
                }

                recipe.recipeLikes = 0;
                recipe.recipeCreationDate = DateTime.Now;


                string currentUserName = User.Identity.GetUserName();
                int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
                recipe.chefId = currentChefId;

                db.recipe.Add(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.chefId = new SelectList(db.chef, "chefId", "chefName", recipe.chefId);
            ViewBag.cuisineId = new SelectList(db.cuisine, "cuisineId", "cuisineName", recipe.cuisineId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.recipe.Find(id);
            TempData["imagePathRecipe"] = recipe.recipeImagePath;
            if (recipe == null)
            {
                return HttpNotFound();
            }

            string currentUserName = User.Identity.GetUserName();
            int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
            if (recipe.chefId != currentChefId)
            {
                TempData["imagePathRecipe"] = null;
                return RedirectToAction("Index");
            }


            ViewBag.chefId = new SelectList(db.chef, "chefId", "chefName", recipe.chefId);
            ViewBag.cuisineId = new SelectList(db.cuisine, "cuisineId", "cuisineName", recipe.cuisineId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "recipeId,recipeTitle,recipeDescription,recipeIngredients,recipeDirection,recipeServesFor,recipeType,recipeNutritionFacts,recipeIsPrivate,recipePrepTime,recipeTags,recipeImageFile,cuisineId,chefId,recipeCreationDate")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                if (recipe.recipeImageFile == null)
                {
                    if (TempData["imagePathRecipe"] == null)
                        return View(recipe);
                    recipe.recipeImagePath = TempData["imagePathRecipe"].ToString();
                    TempData["imagePathRecipe"] = null;
                }
                else
                {
                    string extension = Path.GetExtension(recipe.recipeImageFile.FileName);
                    string fileName = "R-" + recipe.recipeTitle + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    recipe.recipeImagePath = "~/Content/RecipeImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/RecipeImages/"), fileName);
                    recipe.recipeImageFile.SaveAs(fileName);
                }

                // November TODO change this vivek
                recipe.recipeCreationDate = DateTime.Now;
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.chefId = new SelectList(db.chef, "chefId", "chefName", recipe.chefId);
            ViewBag.cuisineId = new SelectList(db.cuisine, "cuisineId", "cuisineName", recipe.cuisineId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.recipe.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            string currentUserName = User.Identity.GetUserName();
            int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
            if (recipe.chefId != currentChefId)
                return RedirectToAction("Index");
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.recipe.Find(id);
            db.recipe.Remove(recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult SendEmail(int id)
        {
            ViewBag.emailRecipes = db.recipe.Include(m => m.cusine).Include(m => m.chef).First(m => (m.recipeId == id));
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message, string rID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("recipyyy@gmail.com", "Recipyyy");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "November@123";
                    var sub = subject;
                    var body = message;
                    var reID = rID;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("Details", "Recipes", new { id = reID });

                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
