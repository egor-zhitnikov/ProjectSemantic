using Microsoft.AspNetCore.Mvc;
using OpenLinkedDataLibrary.DBPedia;
using ProjectSemantic3WebMVC.Models;

namespace ProjectSemantic3WebMVC.Controllers
{
    public class SearchController : Controller
    {
        private const int MAX_SEARCH_RESULTS = 5;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Results(string input)
        {
            List<PersonModel> models = null; // Загородній Юрій Іванович

            if (string.IsNullOrEmpty(input))
            {
                return RedirectToAction("Index", "Search");
            }

            await Task.Run(() =>
            {
                DBPediaPersonQueries.GetPersonByName(input, out var m);
                // models = Queries.GetAll();
                models = m;
            });

            if (models.Count > MAX_SEARCH_RESULTS)
            {
                models.RemoveRange(MAX_SEARCH_RESULTS, models.Count - MAX_SEARCH_RESULTS);
            }
            else if (models.Count == 1)
            {
                return RedirectToAction("Index", "Person", models[0]);
            }

            return View(models);
        }

        [HttpPost]
        public IActionResult StartSearching(SearchModel sm)
        {
            string i = sm.Input;
            return RedirectToAction("Results", "Search", new { input = i });
        }

        private static string TruncateAndAddEllipsis(string inputString, int maxLength)
        {
            if (inputString.Length <= maxLength)
            {
                return inputString;
            }
            else
            {
                string truncatedString = inputString.Substring(0, maxLength) + "...";
                return truncatedString;
            }
        }
    }
}