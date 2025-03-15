using Microsoft.AspNetCore.Mvc;

namespace VulnerableWebApp.Controllers
{
    public class HomeController : Controller
    {
        // Action "Index" appelée quand on va sur "/"
        public IActionResult Index()
        {
            // Soit on renvoie une vue, soit on renvoie du texte
             return View("~/Views/User/Index.cshtml"); 
            // return Content("Bonjour, bienvenue sur la page d'accueil !");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}