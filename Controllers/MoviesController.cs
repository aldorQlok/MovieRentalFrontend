using Microsoft.AspNetCore.Mvc;
using MovieRentalFrontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace MovieRentalFrontend.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7127/";
        public MoviesController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Available Movies";

            // Anropar APIets endpoint för att hämta alla filmer
            var response = await _client.GetAsync($"{baseUri}api/Movies");

            // läser av JSON som en string från bodyn
            var json = await response.Content.ReadAsStringAsync();

            // Omvandlar JSON till ett objekt av typen List<Movie>();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(json);

            return View(movieList);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "New Movie";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }
            // konvertera objektet till en JSON-string
            var json = JsonConvert.SerializeObject(movie);

            // lägger till JSON-stringen till body-delen av vår begäran och sätter dess header 	 till typen JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //skickar POST-förfrågan till servern tillsammans med bodyn.
            var response = await _client.PostAsync($"{baseUri}api/movies/addMovie", content);

            // Skickar användaren till en action när operationen är färdig.
            // I det här fallet skickas användaren till "Index".
            // vill vi välja en annan controller ä nuvarande kan vi lägga till namnet så här:
            // RedirectToAction("Index", "Home");
            return RedirectToAction("Index");
        }


        // movies/edit/4
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/movies/movie/{id}");

            var json = await response.Content.ReadAsStringAsync();

            var movie = JsonConvert.DeserializeObject<Movie>(json);

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }
            var json = JsonConvert.SerializeObject(movie);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PutAsync($"{baseUri}api/movies/updateMovie/{movie.Id}", content);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}api/movies/deleteMovie/{id}");

            return RedirectToAction("Index");
        }
    }
}
