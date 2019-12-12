using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Recipyyy.Models;
using Microsoft.AspNet.Identity;

namespace Recipyyy.Controllers
{
    public class CommentsController : Controller
    {
        private RecipyyyContext db = new RecipyyyContext();

        // GET: Comments
        public ActionResult Index()
        {
            
            return View(db.comment.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["rid"] = id;
            ViewBag.backTo = id;

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentId,commentText,commentCreationTime")] Comment comment)
        {
            if (ModelState.IsValid && comment.commentText != null && TempData["rid"] != null)
            {
                string currentUserName = User.Identity.GetUserName();
                int currentChefId = db.chef.First(m => m.chefUsername == currentUserName).chefId;
                int rid = Convert.ToInt32(TempData["rid"]);
                comment.recipe = db.recipe.First(m => m.recipeId == rid);
                comment.chef = db.chef.First(m => m.chefId == currentChefId);
                comment.commentCreationTime = DateTime.Now;
                db.comment.Add(comment);
                db.SaveChanges();
                TempData["rid"] = null;
                int rcid = comment.recipe.recipeId;
                return RedirectToAction("Details", "Recipes", new { @id = rcid });
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comment.Include(m => m.chef).Include(m => m.recipe).First(m => m.commentId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.backTo = comment.recipeId;
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentId,commentText,commentCreationTime,chefId,recipeId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.commentCreationTime = DateTime.Now;
                comment.chef = db.chef.First(m => m.chefId == comment.chefId);
                comment.recipe = db.recipe.First(m => m.recipeId == comment.recipeId);

                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                int rid = comment.recipe.recipeId;
                
                return RedirectToAction("Details", "Recipes", new { @id = rid });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int rid)
        {
            Comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details","Recipes",new { @id = rid});
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
