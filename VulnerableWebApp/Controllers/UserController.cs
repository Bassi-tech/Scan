
namespace VulnerableWebApp.Controllers

{
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint pour se loguer (volontairement vulnérable)
        [HttpGet("Login", Name = "UserLogin")]
        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }

        [HttpPost("Login")]
        public IActionResult LoginPost(string username, string password)
        {
            // Vulnérabilité : on exécute une requête SQL brute (sans paramètre)
            // => Injection possible dans 'username' ou 'password'
            var query = $"SELECT * FROM UserAccounts WHERE Username = '{username}' AND Password = '{password}'";

            var user = _context.UserAccounts
                               .FromSqlRaw(query)
                               .FirstOrDefault();

            if (user != null)
            {
                 return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewBag.Message = "Identifiants invalides ou injection mal formée...";
                return View("Login");
            }

            
        }

        [HttpGet("Logout", Name = "UserLogout")]
public IActionResult Logout()
{
    // Ici, vous pourriez effacer la session ou les cookies d'authentification si vous en utilisez.
    // Puis rediriger vers la page d'accueil (Index du HomeController).
    return RedirectToAction("Index", "Home");
}


        // Endpoint pour laisser un commentaire (vulnérable à la XSS)
        [HttpGet("Comment")]
        public IActionResult Comment()
        {
            // Affiche la liste des commentaires
            var comments = _context.Comments.ToList();
            return View(comments);
        }

        [HttpPost("Comment")]
        public IActionResult CommentPost(string content)
        {
            // Vulnérabilité : pas de filtrage/encodage => XSS
            var comment = new Comment { Content = content };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            // On récupère la liste des commentaires
            var comments = _context.Comments.ToList();
            return View("Comment", comments);
        }
    }
}