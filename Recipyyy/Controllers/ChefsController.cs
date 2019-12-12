using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Recipyyy.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace Recipyyy.Controllers
{
    [Authorize]
    public class ChefsController : Controller
    {
        private RecipyyyContext db = new RecipyyyContext();

        // GET: Chefs
        public ActionResult Index()
        {
            return View(db.chef.ToList());
        }

        // GET: Chefs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chef chef = db.chef.Find(id);
            
            if (chef == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.recdRecipes = db.recipe.Include(m => m.cusine).Include(m => m.chef).Where(m => (m.recipeIsPrivate == false) && (m.chefId == id));

            return View(chef);
        }

        // GET: Chefs/Create
        public ActionResult Create()
        {
            if (TempData["step2"] == null)
            {
                return RedirectToAction("Register", "Account");
            }
            return View();
            
        }

        // POST: Chefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chefId,chefName,chefUsername,chefBio,chefRating,chefTipsEnable,chefCreationDate,chefImageFile")] Chef chef)
        {
           
            if (ModelState.IsValid)
            {
                if (chef.chefImageFile == null)
                {
                    chef.chefImagePath = "~/Content/ChefImages/" + "CF-" + "Dummy" + ".jpg";
                }
                else
                {
                    string extension = Path.GetExtension(chef.chefImageFile.FileName);

                    string fileName = "CF-" + chef.chefName + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    chef.chefImagePath = "~/Content/ChefImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/ChefImages/"), fileName);
                    chef.chefImageFile.SaveAs(fileName);
                }

                // To connect Asp user to our chef model
                chef.chefCreationDate = DateTime.Now;
                chef.chefUsername = User.Identity.GetUserName();
                chef.chefRating = -1;
                db.chef.Add(chef);
                db.SaveChanges();
                TempData["step2"] = null;
                return RedirectToAction("Index","Recipes");
            }

            return View(chef);
        }

        // GET: Chefs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chef chef = db.chef.Find(id);
            TempData["imagePathChef"] = chef.chefImagePath;
            if (chef == null)
            {
                return HttpNotFound();
            }
            return View(chef);
        }

        // POST: Chefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chefId,chefName,chefUsername,chefBio,chefRating,chefTipsEnable,chefCreationDate,chefImageFile")] Chef chef)
        {
            if (ModelState.IsValid)
            {
                if (chef.chefImageFile == null)
                {
                    if (TempData["imagePathChef"] == null)
                        return View(chef);
                    chef.chefImagePath = TempData["imagePathChef"].ToString();
                    TempData["imagePathChef"] = null;
                }
                else
                {
                    string extension = Path.GetExtension(chef.chefImageFile.FileName);
                    string fileName = "CH-" + chef.chefName + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    chef.chefImagePath = "~/Content/ChefImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/ChefImages/"), fileName);
                    chef.chefImageFile.SaveAs(fileName);
                }
                chef.chefCreationDate = DateTime.Now;
                db.Entry(chef).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chef);
        }

        // GET: Chefs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chef chef = db.chef.Find(id);
            if (chef == null)
            {
                return HttpNotFound();
            }
            return View(chef);
        }

        // POST: Chefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chef chef = db.chef.Find(id);
            db.chef.Remove(chef);
            db.SaveChanges();
            return RedirectToAction("Index");
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
