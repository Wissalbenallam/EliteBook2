using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    public class TransporteurController : Controller
    {
        private readonly TmsDbContext _context;

        public TransporteurController(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transporteurs = await _context.Transporteurs
                .Include(t => t.Camions)
                .Include(t => t.Chauffeurs)
                .OrderBy(t => t.Nom)
                .ToListAsync();

            return View(transporteurs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var transporteur = await _context.Transporteurs
                .Include(t => t.Camions)
                .Include(t => t.Chauffeurs)
                .Include(t => t.Tournees)
                .FirstOrDefaultAsync(t => t.TransporteurId == id);

            if (transporteur == null) return NotFound();
            return View(transporteur);
        }

        public IActionResult Create()
        {
            return View(new Transporteur
            {
                DateCreation = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transporteur transporteur)
        {
            if (!ModelState.IsValid)
            {
                return View(transporteur);
            }

            _context.Transporteurs.Add(transporteur);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Transporteur {transporteur.Nom} cree avec succes.";
            return RedirectToAction(nameof(Index));
        }
    }
}
