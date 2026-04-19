namespace TMS_Project.Models
{
    public class DemandeLivraison
    {
        public int DemandeLivraisonId { get; set; }
        public int? ClientId { get; set; }            // peut être null si inscription après
        public string Adresse { get; set; } = string.Empty;
        public int PoidsKg { get; set; }
        public DateTime DateSouhaitee { get; set; }
        public string Statut { get; set; } = "En attente"; // En attente, Acceptée, Refusée
        public decimal MontantEstime { get; set; }
        public int? CamionAffecteId { get; set; }
        public int? ChauffeurAffecteId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}