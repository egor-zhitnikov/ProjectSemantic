using Microsoft.AspNetCore.Mvc;
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
            var model = new PersonModel() { FirstName = input };

            //return Redirect("/Person/Index");
            return RedirectToAction("Index", "Person", model);

            ////if (ModelState.IsValid)
            ////{
            ////	return View("Index");
            ////}
            //return Redirect("/");
        }
    }
}