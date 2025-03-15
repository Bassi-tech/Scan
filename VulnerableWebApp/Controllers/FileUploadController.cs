using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace VulnerableWebApp.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: Affiche le formulaire de téléversement
        [HttpGet("FileUpload")]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Traite le fichier téléversé
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(IFormFile file)
        {
            // Vérifiez simplement que le fichier n'est pas nul
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Aucun fichier sélectionné.";
                return View();
            }

            // Chemin de stockage (dans un dossier "uploads" dans wwwroot)
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Créez le dossier s'il n'existe pas
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // On ne fait AUCUNE vérification sur l'extension, le type ou le contenu !
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            // Sauvegarder le fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewBag.Message = $"Fichier {file.FileName} téléversé avec succès.";
            return View();
        }
    }
}
