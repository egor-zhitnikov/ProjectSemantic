using Microsoft.AspNetCore.Mvc;
using OpenLinkedDataLibrary.DBPedia;
using ProjectSemantic3WebMVC.Models;

namespace ProjectSemantic3WebMVC.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartSearching(SearchModel sm) // todo: Вести поиск по базе данных и выводить нужный профиль на экран, либо ошибку
        {
            string input = sm.Input;
            var model = DBPediaPersonQueries.GetPersonByName(input, out var res); //new PersonModel() { Name };

            if (res.Count != 0)
            {
                return RedirectToAction("Index", "Person", model);
            }
            else
            {
                return Redirect("/");
            }
            //return Redirect("/Person/Index");


            ////if (ModelState.IsValid)
            ////{
            ////	return View("Index");
            ////}
            //return Redirect("/");
        }
    }
}