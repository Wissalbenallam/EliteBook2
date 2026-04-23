using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    public class ChauffeurController : Controller
    {
        private readonly TmsDbContext _context;

        public ChauffeurController(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var chauffeurs = await _context.Chauffeurs
                .Include(c => c.Transporteur)
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToListAsync();

            return View(chauffeurs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var chauffeur = await _context.Chauffeurs
                .Include(c => c.Transporteur)
                .Include(c => c.Tournees)
                .FirstOrDefaultAsync(c => c.ChauffeurId == id);

            if (chauffeur == null) return NotFound();
            return View(chauffeur);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateTransporteursAsync();
            return View(new Chauffeur
            {
                DateEmbauche = DateTime.Today,
                EstActif = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chauffeur chauffeur)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTransporteursAsync(chauffeur.TransporteurId);
                return View(chauffeur);
            }

            _context.Chauffeurs.Add(chauffeur);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Chauffeur {chauffeur.Prenom} {chauffeur.Nom} cree avec succes.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateTransporteursAsync(int? selectedId = null)
        {
            var transporteurs = await _context.Transporteurs
                .OrderBy(t => t.Nom)
                .ToListAsync();

            ViewBag.TransporteurId = new SelectList(transporteurs, "TransporteurId", "Nom", selectedId);
        }
    }
}
