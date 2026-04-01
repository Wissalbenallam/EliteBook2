namespace TMS_Project.Models
{
    public class Camion
    {
        public int CamionId { get; set; }
        public int TransporteurId { get; set; }
        public string Marque { get; set; } = string.Empty;
        public string Modele { get; set; } = string.Empty;
        public string Immatriculation { get; set; } = string.Empty;
        public int CapaciteKg { get; set; }
        public DateTime DateAcquisition { get; set; }
        public bool EstActif { get; set; } = true;

        // Foreign Key
        public Transporteur Transporteur { get; set; } = null!;

        // Relationships
        public ICollection<Tournee> Tournees { get; set; } = new List<Tournee>();
    }
}
