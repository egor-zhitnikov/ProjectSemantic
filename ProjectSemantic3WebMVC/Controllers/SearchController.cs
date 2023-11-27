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
        private const int MAX_SEARCH_RESULTS = 50;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Results(string input, bool[] filters)
        {
            List<PersonModel> models = null; 

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