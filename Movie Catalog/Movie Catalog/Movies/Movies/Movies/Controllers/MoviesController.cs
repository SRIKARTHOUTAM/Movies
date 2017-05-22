using System.Collections.Generic;
using System.Web.Mvc;
using Movies.Data.Entities;
using System.Xml.Linq;
using Kendo.Mvc.UI;
using System;
using Kendo.Mvc.Extensions;
using System.Linq;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        public XDocument MoviesXmlDoc;
        // GET: Movies
        public ActionResult DisplayMovieGrid()
        {
            return View("MoviesList");
        }


        public ActionResult Read_MoviesList([DataSourceRequest]DataSourceRequest request)
        {
            MoviesXmlDoc = new XDocument();
            MoviesXmlDoc = XDocument.Load(Server.MapPath("~/App_Data/MoviesList.xml"));
            if (MoviesXmlDoc.Root == null)
                return Json(((List<MoviesClass>) null).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            var movienames = MoviesXmlDoc.Root.Elements("Movie").
                Select(q => new MoviesClass { Id = (int)q.Element("Id"),
                    MovieName =q.Element("MovieName")?.Value,
                    MovieGenere = q.Element("MovieGener")?.Value,
                    MovieReleaseYear = (int)q.Element("MovieReleaseYear"),
                    MovieCollectionAmount = (short)q.Element("MovieCollectionAmount")});                
            var moviesList = movienames.ToList();
            return Json(moviesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update_MoviesList([DataSourceRequest]DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<MoviesClass> updatedMovieModel)
        {
            MoviesXmlDoc = new XDocument();
            MoviesXmlDoc = XDocument.Load(Server.MapPath("~/App_Data/MoviesList.xml"));
            var moviesClasses = updatedMovieModel as MoviesClass[] ?? updatedMovieModel.ToArray();
            foreach (var item in moviesClasses)
            {
                var node = MoviesXmlDoc.Root?.Elements("Movie").FirstOrDefault(i => (int)i.Element("Id") == item.Id);
                if (node != null)
                {
                    node.SetElementValue("MovieGener", item.MovieGenere);
                    node.SetElementValue("MovieReleaseYear", item.MovieReleaseYear);
                    node.SetElementValue("MovieCollectionAmount", item.MovieCollectionAmount);
                }
                MoviesXmlDoc.Save(Server.MapPath("~/App_Data/MoviesList.xml"));
            }
            return Json(moviesClasses.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create_MoviesList([DataSourceRequest]DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<MoviesClass> insertMovieModel)
        {
            if (insertMovieModel == null) throw new ArgumentNullException(nameof(insertMovieModel));
            MoviesXmlDoc = new XDocument();
            MoviesXmlDoc = XDocument.Load(Server.MapPath("~/App_Data/MoviesList.xml"));
            var moviesClasses = insertMovieModel as MoviesClass[] ?? insertMovieModel.ToArray();
            foreach (var item in moviesClasses)
            {
                var first = MoviesXmlDoc.Descendants("Movie").OrderByDescending(s => (int) s.Element("Id")).Select(s1 => (int) s1.Element("Id")).FirstOrDefault();
                item.Id = first + 1;

                MoviesXmlDoc.Root?.Add(new XElement("Movie", new XElement("Id", item.Id),
                    new XElement("MovieName", item.MovieName),
                    new XElement("MovieGener", item.MovieGenere),
                    new XElement("MovieReleaseYear", item.MovieReleaseYear),
                    new XElement("MovieCollectionAmount", item.MovieCollectionAmount)
                ));
                MoviesXmlDoc.Save(Server.MapPath("~/App_Data/MoviesList.xml"));
            }

            return Json(moviesClasses.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete_MoviesList([DataSourceRequest]DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<MoviesClass> deleteMovieModel)
        {
            if (deleteMovieModel == null) throw new ArgumentNullException(nameof(deleteMovieModel));
            MoviesXmlDoc = new XDocument();
            MoviesXmlDoc = XDocument.Load(Server.MapPath("~/App_Data/MoviesList.xml"));
            var moviesClasses = deleteMovieModel as MoviesClass[] ?? deleteMovieModel.ToArray();
            foreach (MoviesClass item in moviesClasses)
            {
                MoviesXmlDoc.Root?.Elements("Movie").Where(i => (int) i.Element("Id") == item.Id).Remove();

                MoviesXmlDoc.Save(Server.MapPath("~/App_Data/MoviesList.xml"));
            }
            return Json(moviesClasses.ToDataSourceResult(request, ModelState));
        }



    }
}

