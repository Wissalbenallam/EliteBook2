namespace TMS_Project.Models
{
    public class Livraison
    {
        public int LivraisonId { get; set; }
        public int TourneeId { get; set; }
        public int ClientId { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public int PoidsKg { get; set; }
        public DateTime DateLivraison { get; set; }
        public string StatutLivraison { get; set; } = "En attente"; // En attente, Livrée, Échouée
        public DateTime? DateRealisation { get; set; }

        // Foreign Keys
        public Tournee Tournee { get; set; } = null!;
        public Client Client { get; set; } = null!;
    }
}
