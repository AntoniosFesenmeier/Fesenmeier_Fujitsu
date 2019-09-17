using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fesenmeier_Fujitsu.Models;

namespace Fesenmeier_Fujitsu.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly IMovieRepository _movieRepository;

        public HomeController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [Route("")]
        [Route("/Home")]
        [Route("/Home/Index")]
        public async Task<IActionResult> Index(string searchString)
        {           
            ViewData["SearchString"] = searchString;        
            return View(await _movieRepository.GetAllMovies(searchString));
        }
 
        public async Task<IActionResult> Edit(string Id)
        {
            var movie = await _movieRepository.GetMovie(Id);
            return View(movie);
        }

        [HttpGet] 
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieRepository.Create(movie);
                return RedirectToAction("Index", "");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Movie movie)
        {
            if (ModelState.IsValid)
            {
                var editMovie = await _movieRepository.Update(id, movie);
                return View(editMovie);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _movieRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
