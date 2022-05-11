using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViewsLab.Models;

namespace ViewsLab.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "I am learning ASP.NET Core MVC";
            return View("SampleView");
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Your application description page.";

            ViewsLab.Models.Movie movie = new ViewsLab.Models.Movie
            {
                ID = 1,
                Title = "Follow the Wind",
                ReleaseDate = new DateTime(2017, 01, 21)
            };

            return View(movie);
        }

        [HttpPost]
        public IActionResult Privacy(Models.Movie movieIncoming)
        {
            // Your logic here

            return View(movieIncoming);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
