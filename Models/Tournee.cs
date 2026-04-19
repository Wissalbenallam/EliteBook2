using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMS_Project.Models
{
    public class Tournee
    {
        public int TourneeId { get; set; }
        public int TransporteurId { get; set; }
        public int CamionId { get; set; }
        public int ChauffeurId { get; set; }
        public DateTime DateTournee { get; set; }
        public string StatutTournee { get; set; } = "Planifiée"; // Planifiée, En cours, Terminée
        public decimal CoutTotal { get; set; } = 0m;

        // Foreign Keys / Navigation
        public Transporteur Transporteur { get; set; } = null!;
        public Camion Camion { get; set; } = null!;
        public Chauffeur Chauffeur { get; set; } = null!;

        // Relationships
        public ICollection<Livraison> Livraisons { get; set; } = new List<Livraison>();
        public ICollection<Cout> Couts { get; set; } = new List<Cout>();
    }
}
