using Microsoft.AspNetCore.Mvc;
using ProjectSemantic3WebMVC.Models;

namespace ProjectSemantic3WebMVC.Controllers
{
	public class PersonController : Controller
	{
        public IActionResult Index(PersonModel model)
        {
            // Use the searchData on the target page
            // For example, you can pass it to the view or use it in your logic

            // ViewData["SearchData"] = searchData;
            return View(model);
        }
    }
}