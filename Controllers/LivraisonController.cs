using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;
using TMS_Project.ViewModels;

namespace TMS_Project.Controllers
{
    public class LivraisonController : Controller
    {
        private readonly TmsDbContext _context;

        public LivraisonController(TmsDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────
        // INDEX
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var livraisons = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.Tournee)
                .OrderByDescending(l => l.DateLivraison)
                .ToListAsync();

            return View(livraisons);
        }

        // ─────────────────────────────────────────────
        // DETAILS
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var livraison = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.Tournee)
                    .ThenInclude(t => t.Camion)
                .Include(l => l.Tournee)
                    .ThenInclude(t => t.Chauffeur)
                .FirstOrDefaultAsync(l => l.LivraisonId == id);

            if (livraison == null) return NotFound();
            return View(livraison);
        }

        // ─────────────────────────────────────────────
        // CREATE — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Create(int? tourneeId)
        {
            var vm = await BuildFormViewModelAsync();
            if (tourneeId.HasValue)
                vm.Livraison.TourneeId = tourneeId.Value;

            return View(vm);
        }

        // ─────────────────────────────────────────────
        // CREATE — POST
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivraisonFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync();
                rebuilt.Livraison = vm.Livraison;
                return View(rebuilt);
            }

            _context.Livraisons.Add(vm.Livraison);
            await _context.SaveChangesAsync();

            // Recalculer le coût total de la tournée associée
            await RecalculerCoutTournee(vm.Livraison.TourneeId);

            TempData["Success"] = $"✅ Livraison #{vm.Livraison.LivraisonId} créée avec succès !";

            // Retourner aux détails de la tournée si on vient de là
            if (vm.Livraison.TourneeId > 0)
                return RedirectToAction("Details", "Tournee", new { id = vm.Livraison.TourneeId });

            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // EDIT — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison == null) return NotFound();

            var vm = await BuildFormViewModelAsync();
            vm.Livraison = livraison;
            return View(vm);
        }

        // ─────────────────────────────────────────────
        // EDIT — POST
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LivraisonFormViewModel vm)
        {
            if (id != vm.Livraison.LivraisonId) return NotFound();

            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync();
                rebuilt.Livraison = vm.Livraison;
                return View(rebuilt);
            }

            // Si livrée, enregistrer la date de réalisation
            if (vm.Livraison.StatutLivraison == "Livrée" && vm.Livraison.DateRealisation == null)
                vm.Livraison.DateRealisation = DateTime.Now;

            _context.Attach(vm.Livraison).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✅ Livraison #{id} modifiée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // DELETE — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Delete(int id)
        {
            var livraison = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.Tournee)
                .FirstOrDefaultAsync(l => l.LivraisonId == id);

            if (livraison == null) return NotFound();
            return View(livraison);
        }

        // ─────────────────────────────────────────────
        // DELETE — POST
        // ─────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison != null)
            {
                int tourneeId = livraison.TourneeId;
                _context.Livraisons.Remove(livraison);
                await _context.SaveChangesAsync();
                await RecalculerCoutTournee(tourneeId);
                TempData["Success"] = $"🗑️ Livraison #{id} supprimée.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // HELPERS PRIVÉS
        // ─────────────────────────────────────────────
        private async Task<LivraisonFormViewModel> BuildFormViewModelAsync()
        {
            var clients = await _context.Clients.OrderBy(c => c.Nom).ToListAsync();
            var tournees = await _context.Tournees
                .Where(t => t.StatutTournee != "Terminée")
                .OrderByDescending(t => t.DateTournee)
                .ToListAsync();

            return new LivraisonFormViewModel
            {
                Clients = clients.Select(c => new SelectListItem
                {
                    Value = c.ClientId.ToString(),
                    Text = c.Nom
                }),
                Tournees = tournees.Select(t => new SelectListItem
                {
                    Value = t.TourneeId.ToString(),
                    Text = $"Tournée #{t.TourneeId} — {t.DateTournee:dd/MM/yyyy} ({t.StatutTournee})"
                })
            };
        }

        private async Task RecalculerCoutTournee(int tourneeId)
        {
            // Cette méthode est un placeholder — le CoutTotal sur la Tournée
            // est géré principalement par TourneeController.AjouterCout
            // Ici on peut juste rafraîchir si nécessaire
            if (tourneeId <= 0) return;
            var tournee = await _context.Tournees
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == tourneeId);
            if (tournee != null)
            {
                tournee.CoutTotal = tournee.Couts.Sum(c => c.Montant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
