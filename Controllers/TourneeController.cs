using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;

namespace TMS_Project.Controllers
{
    public class TourneeController : Controller
    {
        private readonly TmsDbContext _context;

        public TourneeController(TmsDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────
        // INDEX — liste toutes les tournées
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var tournees = await _context.Tournees
                .Include(t => t.Transporteur)
                .Include(t => t.Camion)
                .Include(t => t.Chauffeur)
                .Include(t => t.Livraisons)
                .Include(t => t.Couts)
                .OrderByDescending(t => t.DateTournee)
                .ToListAsync();

            return View(tournees);
        }

        // ─────────────────────────────────────────────
        // DETAILS
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Transporteur)
                .Include(t => t.Camion)
                .Include(t => t.Chauffeur)
                .Include(t => t.Livraisons).ThenInclude(l => l.Client)
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == id);

            if (tournee == null) return NotFound();

            var vm = new TourneeDetailsViewModel
            {
                Tournee = tournee,
                Couts = tournee.Couts.OrderByDescending(c => c.DateCout).ToList()
            };

            return View(vm);
        }

        // ─────────────────────────────────────────────
        // CREATE — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            var vm = await BuildFormViewModelAsync();
            return View(vm);
        }

        // ─────────────────────────────────────────────
        // CREATE — POST
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourneeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync();
                rebuilt.Tournee = vm.Tournee;
                rebuilt.SelectedLivraisonIds = vm.SelectedLivraisonIds;
                rebuilt.TypeCout = vm.TypeCout;
                rebuilt.MontantCout = vm.MontantCout;
                rebuilt.DescCout = vm.DescCout;
                return View(rebuilt);
            }

            // 1. Sauvegarder la tournée
            _context.Tournees.Add(vm.Tournee);
            await _context.SaveChangesAsync();

            // 2. Affecter les livraisons sélectionnées
            if (vm.SelectedLivraisonIds != null && vm.SelectedLivraisonIds.Any())
            {
                var livraisons = await _context.Livraisons
                    .Where(l => vm.SelectedLivraisonIds.Contains(l.LivraisonId))
                    .ToListAsync();

                foreach (var liv in livraisons)
                    liv.TourneeId = vm.Tournee.TourneeId;
            }

            // 3. Ajouter le coût initial si montant > 0
            if (vm.MontantCout > 0)
            {
                var cout = new Cout
                {
                    TourneeId = vm.Tournee.TourneeId,
                    TypeCout = vm.TypeCout,
                    Montant = vm.MontantCout,
                    Description = vm.DescCout,
                    DateCout = DateTime.Now
                };
                _context.Couts.Add(cout);
            }

            // 4. Recalculer CoutTotal
            await _context.SaveChangesAsync();
            await RecalculerCoutTotal(vm.Tournee.TourneeId);

            TempData["Success"] = $"✅ Tournée #{vm.Tournee.TourneeId} créée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // EDIT — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Livraisons)
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == id);

            if (tournee == null) return NotFound();

            var vm = await BuildFormViewModelAsync(tournee);
            vm.SelectedLivraisonIds = tournee.Livraisons.Select(l => l.LivraisonId).ToList();

            return View(vm);
        }

        // ─────────────────────────────────────────────
        // EDIT — POST
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourneeFormViewModel vm)
        {
            if (id != vm.Tournee.TourneeId) return NotFound();

            if (!ModelState.IsValid)
            {
                var rebuilt = await BuildFormViewModelAsync(vm.Tournee);
                rebuilt.SelectedLivraisonIds = vm.SelectedLivraisonIds;
                return View(rebuilt);
            }

            // Mise à jour tournée
            _context.Attach(vm.Tournee).State = EntityState.Modified;

            // Mise à jour livraisons :
            // — désaffecter celles qui étaient là mais plus cochées
            var livsActuelles = await _context.Livraisons
                .Where(l => l.TourneeId == id)
                .ToListAsync();
            foreach (var liv in livsActuelles)
                liv.TourneeId = 0; // temporairement vide (on va remettre ci-dessous)

            // — affecter les nouvellement sélectionnées
            if (vm.SelectedLivraisonIds != null && vm.SelectedLivraisonIds.Any())
            {
                var livsSelectionnees = await _context.Livraisons
                    .Where(l => vm.SelectedLivraisonIds.Contains(l.LivraisonId))
                    .ToListAsync();
                foreach (var liv in livsSelectionnees)
                    liv.TourneeId = id;
            }

            // Ajouter un nouveau coût si saisi
            if (vm.MontantCout > 0)
            {
                var cout = new Cout
                {
                    TourneeId = id,
                    TypeCout = vm.TypeCout,
                    Montant = vm.MontantCout,
                    Description = vm.DescCout,
                    DateCout = DateTime.Now
                };
                _context.Couts.Add(cout);
            }

            await _context.SaveChangesAsync();
            await RecalculerCoutTotal(id);

            TempData["Success"] = $"✅ Tournée #{id} modifiée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // DELETE — GET
        // ─────────────────────────────────────────────
        public async Task<IActionResult> Delete(int id)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Camion)
                .Include(t => t.Chauffeur)
                .Include(t => t.Livraisons)
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == id);

            if (tournee == null) return NotFound();
            return View(tournee);
        }

        // ─────────────────────────────────────────────
        // DELETE — POST
        // ─────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Livraisons)
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == id);

            if (tournee != null)
            {
                // Supprimer les coûts liés
                _context.Couts.RemoveRange(tournee.Couts);

                // Note: les Livraisons ont TourneeId non-nullable dans le modèle,
                // donc on les supprime aussi (ou on les garde selon la règle métier)
                // Ici on choisit de les GARDER en les désaffectant n'est pas possible
                // car TourneeId est non-nullable → on les supprime
                _context.Livraisons.RemoveRange(tournee.Livraisons);

                _context.Tournees.Remove(tournee);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"🗑️ Tournée #{id} et ses livraisons ont été supprimées.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────────
        // AJOUTER UN COÛT à une tournée existante (POST)
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterCout(int tourneeId, string typeCout,
                                                       decimal montant, string description)
        {
            var tournee = await _context.Tournees.FindAsync(tourneeId);
            if (tournee == null) return NotFound();

            if (montant > 0)
            {
                var cout = new Cout
                {
                    TourneeId = tourneeId,
                    TypeCout = typeCout,
                    Montant = montant,
                    Description = description,
                    DateCout = DateTime.Now
                };
                _context.Couts.Add(cout);
                await _context.SaveChangesAsync();
                await RecalculerCoutTotal(tourneeId);
                TempData["Success"] = $"💰 Coût de {montant:N2} MAD ajouté avec succès !";
            }

            return RedirectToAction(nameof(Details), new { id = tourneeId });
        }

        // ─────────────────────────────────────────────
        // SUPPRIMER UN COÛT
        // ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerCout(int coutId, int tourneeId)
        {
            var cout = await _context.Couts.FindAsync(coutId);
            if (cout != null)
            {
                _context.Couts.Remove(cout);
                await _context.SaveChangesAsync();
                await RecalculerCoutTotal(tourneeId);
                TempData["Success"] = "🗑️ Coût supprimé.";
            }
            return RedirectToAction(nameof(Details), new { id = tourneeId });
        }

        // ─────────────────────────────────────────────
        // HELPERS PRIVÉS
        // ─────────────────────────────────────────────
        private async Task RecalculerCoutTotal(int tourneeId)
        {
            var tournee = await _context.Tournees
                .Include(t => t.Couts)
                .FirstOrDefaultAsync(t => t.TourneeId == tourneeId);

            if (tournee != null)
            {
                tournee.CoutTotal = tournee.Couts.Sum(c => c.Montant);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<TourneeFormViewModel> BuildFormViewModelAsync(Tournee? existing = null)
        {
            var transporteurs = await _context.Transporteurs.OrderBy(t => t.Nom).ToListAsync();
            var camions = await _context.Camions.Where(c => c.EstActif).OrderBy(c => c.Immatriculation).ToListAsync();
            var chauffeurs = await _context.Chauffeurs.Where(c => c.EstActif).OrderBy(c => c.Nom).ToListAsync();

            // Livraisons dispo = non encore affectées + celles déjà dans CETTE tournée
            var query = _context.Livraisons.Include(l => l.Client).AsQueryable();
            if (existing != null)
                query = query.Where(l => l.TourneeId == 0 || l.TourneeId == existing.TourneeId);
            else
                query = query.Where(l => l.TourneeId == 0);

            var livraisonsDispos = await query.ToListAsync();

            return new TourneeFormViewModel
            {
                Tournee = existing ?? new Tournee { DateTournee = DateTime.Today, StatutTournee = "Planifiée" },
                Transporteurs = transporteurs.Select(t => new SelectListItem
                {
                    Value = t.TransporteurId.ToString(),
                    Text = t.Nom
                }),
                Camions = camions.Select(c => new SelectListItem
                {
                    Value = c.CamionId.ToString(),
                    Text = $"{c.Immatriculation} — {c.Marque} {c.Modele} ({c.CapaciteKg} kg)"
                }),
                Chauffeurs = chauffeurs.Select(c => new SelectListItem
                {
                    Value = c.ChauffeurId.ToString(),
                    Text = $"{c.Prenom} {c.Nom}"
                }),
                LivraisonsDisponibles = livraisonsDispos
            };
        }
    }
}
