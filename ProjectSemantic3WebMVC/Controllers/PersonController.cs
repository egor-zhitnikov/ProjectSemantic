using OpenLinkedDataLibrary.DBPedia;
using Microsoft.AspNetCore.Mvc;

namespace ProjectSemantic3WebMVC.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index(PersonModel model)
        {
            return View(model);
        }
    }
}