using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Data;
using TMS_Project.Models;
using TMS_Project.ViewModels;

namespace TMS_Project.Controllers
{
    public class DemandeLivraisonController : Controller
    {
        private readonly TmsDbContext _context;

        public DemandeLivraisonController(TmsDbContext context)
        {
            _context = context;
        }

        public IActionResult Inscription()
        {
            return View(new ClientDemandeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscription(ClientDemandeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == vm.EmailClient);
            if (client == null)
            {
                client = new Client
                {
                    Nom = vm.NomClient,
                    Email = vm.EmailClient,
                    Telephone = vm.TelephoneClient,
                    Adresse = vm.AdresseClient,
                    DateCreation = DateTime.Now
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            else
            {
                client.Nom = vm.NomClient;
                client.Telephone = vm.TelephoneClient;
                client.Adresse = vm.AdresseClient;
                await _context.SaveChangesAsync();
            }

            var demande = new DemandeLivraison
            {
                ClientId = client.ClientId,
                TypeService = vm.TypeService,
                DescriptionMarchandise = vm.DescriptionMarchandise,
                Quantite = vm.Quantite,
                Adresse = vm.AdresseLivraison,
                PoidsKg = vm.PoidsKg,
                DateSouhaitee = vm.DateSouhaitee,
                Statut = "En attente",
                MontantEstime = CalculerMontant(vm.TypeService, vm.PoidsKg, vm.Quantite),
                CreatedAt = DateTime.Now
            };

            _context.DemandesLivraison.Add(demande);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Confirmation), new { id = demande.DemandeLivraisonId });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            if (demande == null) return NotFound();
            return View(demande);
        }

        public async Task<IActionResult> Admin()
        {
            var demandes = await _context.DemandesLivraison
                .Include(d => d.Client)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return View(demandes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accepter(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            if (demande == null) return NotFound();

            demande.Statut = "Acceptée";
            demande.DateValidation = DateTime.Now;
            if (string.IsNullOrWhiteSpace(demande.NumeroRecu))
            {
                demande.NumeroRecu = $"REC-{DateTime.Now:yyyyMMdd}-{demande.DemandeLivraisonId:D4}";
            }
            demande.CommentaireValidation = "Demande validée avec succès.";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Recu), new { id = demande.DemandeLivraisonId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refuser(int id)
        {
            var demande = await _context.DemandesLivraison.FindAsync(id);
            if (demande == null) return NotFound();

            demande.Statut = "Refusée";
            demande.DateValidation = DateTime.Now;
            demande.CommentaireValidation = "Demande refusée.";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        public async Task<IActionResult> Recu(int id)
        {
            var demande = await _context.DemandesLivraison
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.DemandeLivraisonId == id);

            if (demande == null) return NotFound();
            return View(demande);
        }

        private static decimal CalculerMontant(string typeService, int poidsKg, int quantite)
        {
            decimal basePrice = 120m + (poidsKg * 1.5m) + (quantite * 10m);

            return typeService switch
            {
                "Express" => basePrice * 1.45m,
                "Fragile" => basePrice * 1.25m,
                "Réfrigéré" => basePrice * 1.60m,
                _ => basePrice
            };
        }
    }
}
