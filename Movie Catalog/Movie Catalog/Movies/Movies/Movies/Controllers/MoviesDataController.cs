using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Movies.Data.Entities;
using Movies.Models;

namespace Movies.Controllers
{
    public class MoviesDataController : Controller
    {
        public readonly MoviesContext Db = new MoviesContext();

        // GET: MoviesData
        public ActionResult Index()
        {
            return View(Db.MoviesClasses.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var moviesClass = Db.MoviesClasses.Find(id);
            if (moviesClass == null)
            {
                return HttpNotFound();
            }
            return View(moviesClass);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MovieName,MovieGenere,MovieReleaseYear,MovieCollectionAmount")] MoviesClass moviesClass)
        {
            if (ModelState.IsValid)
            {
                Db.MoviesClasses.Add(moviesClass);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moviesClass);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var moviesClass = Db.MoviesClasses.Find(id);
            if (moviesClass == null)
            {
                return HttpNotFound();
            }
            return View(moviesClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MovieName,MovieGenere,MovieReleaseYear,MovieCollectionAmount")] MoviesClass moviesClass)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(moviesClass).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moviesClass);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var moviesClass = Db.MoviesClasses.Find(id);
            if (moviesClass == null)
            {
                return HttpNotFound();
            }
            return View(moviesClass);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var moviesClass = Db.MoviesClasses.Find(id);
            if (moviesClass != null) Db.MoviesClasses.Remove(moviesClass);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
