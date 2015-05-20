using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GetWiki.Models;
using Newtonsoft.Json;

namespace GetWiki.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index(string searchString)
        {
            var articles = db.Articles.Include(a => a.Category);
            ViewBag.Category = db.Categories.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Category.CategoryName.Contains(searchString));
            }

            return View(articles.ToList().OrderByDescending(a => a.TimeFetched));
        }

        /*
         * rnnamespace=0 makes sure the API only include "Real Content", such as articles.
         * http://www.mediawiki.org/wiki/Manual:Namespace
         */
        public ActionResult GetRandomArticles()
        {
            string url = "http://en.wikipedia.org/w/api.php?action=query&list=random&format=json&rnlimit=10&rnnamespace=0";
            var webClient = new WebClient();
            var json = webClient.DownloadString(url);

            dynamic articleJson = JsonConvert.DeserializeObject(json);

            foreach (var item in articleJson.query.random)
            {
                Article article = new Article();

                article.WikiArticleId = item.id;
                article.Ns = item.ns;
                article.Title = item.title;
                article.TimeFetched = DateTime.Now;

                db.Articles.Add(article);
            }

            db.SaveChanges();
            return View(db.Articles.OrderByDescending(p => p.TimeFetched).Take(10).ToList());
        }

        public ActionResult ClearDatabase()
        {
            /*
             * Using stored procedure to clear database.
             */
            db.Database.ExecuteSqlCommand("ClearDatabase");
            db.SaveChanges();
            return RedirectToAction("Clear");
        }

        public ActionResult Clear()
        {
            return View();
        }

        /*
         * Remove a category from an Article. Does not remove the category itself from the DB.
         */
        public ActionResult RemoveCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Article articleToUpdate = db.Articles.Find(id);
            articleToUpdate.CategoryID = null;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,WikiArticleId,Ns,Title,TimeFetched,CategoryID")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", article.CategoryID);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", article.CategoryID);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,WikiArticleId,Ns,Title,TimeFetched,CategoryID")] Article article)
        {
            var articleToUpdate = db.Articles.Find(article.ArticleID);

            if (ModelState.IsValid)
            {
                articleToUpdate.CategoryID = article.CategoryID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", article.CategoryID);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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
