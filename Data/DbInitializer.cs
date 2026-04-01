using TMS_Project.Models;

namespace TMS_Project.Data
{
    public static class DbInitializerExtensions
    {
        public static void Initialize(TmsDbContext context)
        {
            // Check if database already has data
            if (context.Transporteurs.Any())
            {
                return; // Database already seeded
            }

            // Create Transporteurs
            var transporteurs = new List<Transporteur>
            {
                new Transporteur
                {
                    Nom = "Express Transport",
                    Adresse = "123 Rue de la Paix, Paris",
                    Telephone = "01-23-45-67-89",
                    Email = "contact@expresstransport.fr",
                    DateCreation = DateTime.Now
                },
                new Transporteur
                {
                    Nom = "Logistique Plus",
                    Adresse = "456 Avenue des Routes, Lyon",
                    Telephone = "04-56-78-90-12",
                    Email = "info@logistiqueplus.fr",
                    DateCreation = DateTime.Now
                },
                new Transporteur
                {
                    Nom = "Transco Solutions",
                    Adresse = "789 Boulevard Commerce, Marseille",
                    Telephone = "04-91-12-34-56",
                    Email = "contact@transco.fr",
                    DateCreation = DateTime.Now
                }
            };

            context.Transporteurs.AddRange(transporteurs);
            context.SaveChanges();

            // Create Camions
            var camions = new List<Camion>
            {
                new Camion
                {
                    TransporteurId = 1,
                    Marque = "Volvo",
                    Modele = "FH16",
                    Immatriculation = "AB-123-CD",
                    CapaciteKg = 25000,
                    DateAcquisition = new DateTime(2022, 1, 15),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 1,
                    Marque = "Mercedes",
                    Modele = "Actros",
                    Immatriculation = "EF-456-GH",
                    CapaciteKg = 20000,
                    DateAcquisition = new DateTime(2021, 6, 20),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 2,
                    Marque = "Renault",
                    Modele = "T480",
                    Immatriculation = "IJ-789-KL",
                    CapaciteKg = 18000,
                    DateAcquisition = new DateTime(2023, 3, 10),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 3,
                    Marque = "DAF",
                    Modele = "XF",
                    Immatriculation = "MN-012-OP",
                    CapaciteKg = 24000,
                    DateAcquisition = new DateTime(2020, 11, 5),
                    EstActif = true
                }
            };

            context.Camions.AddRange(camions);
            context.SaveChanges();

            // Create Chauffeurs
            var chauffeurs = new List<Chauffeur>
            {
                new Chauffeur
                {
                    TransporteurId = 1,
                    Nom = "Dupont",
                    Prenom = "Jean",
                    Telephone = "06-11-22-33-44",
                    Email = "jean.dupont@expresstransport.fr",
                    NumeroPermis = "FR12345678",
                    DateEmbauche = new DateTime(2019, 9, 1),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 1,
                    Nom = "Martin",
                    Prenom = "Pierre",
                    Telephone = "06-55-66-77-88",
                    Email = "pierre.martin@expresstransport.fr",
                    NumeroPermis = "FR87654321",
                    DateEmbauche = new DateTime(2020, 3, 15),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 2,
                    Nom = "Bernard",
                    Prenom = "Michel",
                    Telephone = "06-99-88-77-66",
                    Email = "michel.bernard@logistiqueplus.fr",
                    NumeroPermis = "FR11223344",
                    DateEmbauche = new DateTime(2021, 1, 10),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 3,
                    Nom = "Moreau",
                    Prenom = "Luc",
                    Telephone = "06-44-55-66-77",
                    Email = "luc.moreau@transco.fr",
                    NumeroPermis = "FR55667788",
                    DateEmbauche = new DateTime(2018, 7, 20),
                    EstActif = true
                }
            };

            context.Chauffeurs.AddRange(chauffeurs);
            context.SaveChanges();

            // Create Clients
            var clients = new List<Client>
            {
                new Client
                {
                    Nom = "Supermarché Central",
                    Adresse = "100 Rue Commerce, Paris",
                    Telephone = "01-45-67-89-01",
                    Email = "delivery@supermarche-central.fr",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Petite Épicerie Local",
                    Adresse = "50 Rue Principale, Lyon",
                    Telephone = "04-12-34-56-78",
                    Email = "contact@epicerie-local.fr",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Hyper Magasin",
                    Adresse = "200 Avenue Commerçant, Marseille",
                    Telephone = "04-98-76-54-32",
                    Email = "logistique@hypermagasin.fr",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Magasin Spécialisé",
                    Adresse = "75 Boulevard Niche, Toulouse",
                    Telephone = "05-61-22-33-44",
                    Email = "achat@specialise.fr",
                    DateCreation = DateTime.Now
                }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();

            // Create Tournees
            var tournees = new List<Tournee>
            {
                new Tournee
                {
                    TransporteurId = 1,
                    CamionId = 1,
                    ChauffeurId = 1,
                    DateTournee = new DateTime(2024, 1, 15, 8, 0, 0),
                    StatutTournee = "Terminée",
                    CoutTotal = 450.50m
                },
                new Tournee
                {
                    TransporteurId = 1,
                    CamionId = 2,
                    ChauffeurId = 2,
                    DateTournee = new DateTime(2024, 1, 16, 8, 30, 0),
                    StatutTournee = "En cours",
                    CoutTotal = 350.00m
                },
                new Tournee
                {
                    TransporteurId = 2,
                    CamionId = 3,
                    ChauffeurId = 3,
                    DateTournee = new DateTime(2024, 1, 17, 7, 0, 0),
                    StatutTournee = "Planifiée",
                    CoutTotal = 0
                },
                new Tournee
                {
                    TransporteurId = 3,
                    CamionId = 4,
                    ChauffeurId = 4,
                    DateTournee = new DateTime(2024, 1, 18, 9, 0, 0),
                    StatutTournee = "Terminée",
                    CoutTotal = 520.75m
                }
            };

            context.Tournees.AddRange(tournees);
            context.SaveChanges();

            // Create Livraisons
            var livraisons = new List<Livraison>
            {
                new Livraison
                {
                    TourneeId = 1,
                    ClientId = 1,
                    Adresse = "100 Rue Commerce, Paris",
                    PoidsKg = 5000,
                    DateLivraison = new DateTime(2024, 1, 15, 10, 30, 0),
                    StatutLivraison = "Livrée",
                    DateRealisation = new DateTime(2024, 1, 15, 10, 35, 0)
                },
                new Livraison
                {
                    TourneeId = 1,
                    ClientId = 2,
                    Adresse = "50 Rue Principale, Lyon",
                    PoidsKg = 3000,
                    DateLivraison = new DateTime(2024, 1, 15, 16, 0, 0),
                    StatutLivraison = "Livrée",
                    DateRealisation = new DateTime(2024, 1, 15, 16, 5, 0)
                },
                new Livraison
                {
                    TourneeId = 2,
                    ClientId = 3,
                    Adresse = "200 Avenue Commerçant, Marseille",
                    PoidsKg = 4500,
                    DateLivraison = new DateTime(2024, 1, 16, 14, 0, 0),
                    StatutLivraison = "En attente",
                    DateRealisation = null
                },
                new Livraison
                {
                    TourneeId = 3,
                    ClientId = 4,
                    Adresse = "75 Boulevard Niche, Toulouse",
                    PoidsKg = 2000,
                    DateLivraison = new DateTime(2024, 1, 17, 11, 30, 0),
                    StatutLivraison = "En attente",
                    DateRealisation = null
                },
                new Livraison
                {
                    TourneeId = 4,
                    ClientId = 1,
                    Adresse = "100 Rue Commerce, Paris",
                    PoidsKg = 6000,
                    DateLivraison = new DateTime(2024, 1, 18, 15, 0, 0),
                    StatutLivraison = "Livrée",
                    DateRealisation = new DateTime(2024, 1, 18, 15, 20, 0)
                }
            };

            context.Livraisons.AddRange(livraisons);
            context.SaveChanges();

            // Create Couts
            var couts = new List<Cout>
            {
                new Cout { TourneeId = 1, TypeCout = "Carburant", Montant = 250.00m, Description = "Essence pour trajet Paris-Lyon", DateCout = DateTime.Now },
                new Cout { TourneeId = 1, TypeCout = "Péage", Montant = 150.00m, Description = "Péage autoroute A6", DateCout = DateTime.Now },
                new Cout { TourneeId = 1, TypeCout = "Maintenance", Montant = 50.50m, Description = "Remplissage AdBlue", DateCout = DateTime.Now },
                new Cout { TourneeId = 2, TypeCout = "Carburant", Montant = 200.00m, Description = "Diesel pour trajet Lyon-Marseille", DateCout = DateTime.Now },
                new Cout { TourneeId = 2, TypeCout = "Péage", Montant = 150.00m, Description = "Péage autoroute A7", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Carburant", Montant = 350.00m, Description = "Essence pour trajet long", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Maintenance", Montant = 100.50m, Description = "Révision mineure", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Péage", Montant = 70.25m, Description = "Péage autoroute", DateCout = DateTime.Now }
            };

            context.Couts.AddRange(couts);
            context.SaveChanges();
        }
    }
}
