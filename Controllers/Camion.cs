using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    public class CamionController : Controller
    {
        private readonly TmsDbContext _context;

        public CamionController(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var camions = await _context.Camions
                .Include(c => c.Transporteur)
                .OrderBy(c => c.Immatriculation)
                .ToListAsync();

            return View(camions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var camion = await _context.Camions
                .Include(c => c.Transporteur)
                .Include(c => c.Tournees)
                .FirstOrDefaultAsync(c => c.CamionId == id);

            if (camion == null) return NotFound();
            return View(camion);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateTransporteursAsync();
            return View(new Camion
            {
                DateAcquisition = DateTime.Today,
                EstActif = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Camion camion)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTransporteursAsync(camion.TransporteurId);
                return View(camion);
            }

            _context.Camions.Add(camion);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Camion {camion.Immatriculation} cree avec succes.";
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
