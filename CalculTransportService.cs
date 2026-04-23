namespace TMS_Project.Services
{
    public class CalculTransportService
    {
        // Calcul pour le DEVIS (Client)
        public decimal CalculerDevis(int poids, decimal distanceEstimee)
        {
            decimal prixBase = 100; // Frais fixes
            decimal prixAuKg = 0.5m;
            decimal prixAuKm = 1.2m;

            return prixBase + (poids * prixAuKg) + (distanceEstimee * prixAuKm);
        }

        // Calcul pour la TOURNEE (Réel)
        public decimal CalculerCoutReel(decimal distanceReelle, decimal consommationCamion, decimal prixCarburant)
        {
            // Exemple : (Distance / 100 * Consommation) * Prix du litre
            return (distanceReelle / 100 * consommationCamion) * prixCarburant;
        }
    }
}