using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenLinkedDataLibrary;
using OpenLinkedDataLibrary.DBPedia;
using OpenLinkedDataLibrary.Filter;
using ProjectSemantic3WebMVC.Models;

namespace ProjectSemantic3WebMVC.Controllers
{
    public class SearchController : Controller
    {
        private const int MAX_SEARCH_RESULTS = 15;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Results(string input, bool[] filters)
        {
            List<PersonModel> models = null; // Загородній Юрій Іванович

            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(input))
                {
                    QueryHelper.GetPersonByName(input, filters, out var m);

                    if (m.Count == 0)
                    {
                        var all = QueryHelper.GetAll(filters);
                        models = all.Where(x => JsonConvert.SerializeObject(x).ToLower().Contains(input.ToLower())).ToList();
                    }
                    else
                    {
                        models = m;
                    }
                }
                else
                {
                    models = QueryHelper.GetAll(filters);
                }

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
        public IActionResult StartSearching(SearchModel sm, List<string> selectedFilters)
        {
            string i = sm.Input;
            return RedirectToAction("Results", "Search", new { input = i, filters = FilterList.StringArrayToLogicalArray(selectedFilters) });
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