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

namespace Recipyyy.Controllers
{
    [Authorize]
    public class CuisinesController : Controller
    {
        private RecipyyyContext db = new RecipyyyContext();

        // GET: Cuisines
        public ActionResult Index()
        {
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin == true)
                ViewBag.showDelete = 1;
            else
            {
                ViewBag.showDelete = 0;
            }
            return View(db.cuisine.ToList());
        }

        // GET: Cuisines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.cuisine.Find(id);
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // GET: Cuisines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cuisines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cuisineId,cuisineName,cuisineDescription,cuisineImageFile")] Cuisine cuisine)
        {
            
            if (ModelState.IsValid)
            {
                if (cuisine.cuisineImageFile == null)
                {
                    cuisine.cuisineImagePath = "~/Content/CuisineImages/" + "C-" + "Dummy" + ".jpg";
                }
                else 
                {
                    string extension = Path.GetExtension(cuisine.cuisineImageFile.FileName);
                    
                    string fileName = "C-" + cuisine.cuisineName + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    cuisine.cuisineImagePath = "~/Content/CuisineImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/CuisineImages/"), fileName);
                    cuisine.cuisineImageFile.SaveAs(fileName);
                }

                


                db.cuisine.Add(cuisine);
                db.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(cuisine);
        }

        // GET: Cuisines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.cuisine.Find(id);
            TempData["imagePath"] = cuisine.cuisineImagePath;
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // POST: Cuisines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cuisineId,cuisineName,cuisineDescription,cuisineImageFile")] Cuisine cuisine)
        {
            if (ModelState.IsValid)
            {
                if (cuisine.cuisineImageFile == null)
                {
                    if(TempData["imagePath"] == null)
                        return View(cuisine);
                    cuisine.cuisineImagePath = TempData["imagePath"].ToString();
                    TempData["imagePath"] = null; 
                }
                else
                {
                    string extension = Path.GetExtension(cuisine.cuisineImageFile.FileName);
                    string fileName = "C-" + cuisine.cuisineName + DateTime.Now.ToString("yyMMddHHmm") + extension;
                    cuisine.cuisineImagePath = "~/Content/CuisineImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/CuisineImages/"), fileName);
                    cuisine.cuisineImageFile.SaveAs(fileName);
                }
                db.Entry(cuisine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cuisine);
        }

        // GET: Cuisines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.cuisine.Find(id);
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // POST: Cuisines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuisine cuisine = db.cuisine.Find(id);
            db.cuisine.Remove(cuisine);
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
