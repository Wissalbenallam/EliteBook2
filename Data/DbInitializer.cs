using TMS_Project.Models;

namespace TMS_Project.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TmsDbContext context)
        {
            // Check if database already has data
            context.Database.EnsureDeleted();

            // 2. هادي غتكرييها خاوية من جديد
            context.Database.EnsureCreated();
            // Create Transporteurs
            var transporteurs = new List<Transporteur>
            {
                new Transporteur
                {
                    Nom = "Express Transport",
                    Adresse = "123 Rue de la Paix, Casa Blanca",
                    Telephone = "**-**-**-**-**",
                    Email = "contact@expresstransport.ma",
                    DateCreation = DateTime.Now
                },
                new Transporteur
                {
                    Nom = "Logistique Plus",
                    Adresse = "456 Avenue des Routes, Rabat",
                    Telephone = "**-**-**-**-**",
                    Email = "info@logistiqueplus.ma",
                    DateCreation = DateTime.Now
                },
                new Transporteur
                {
                    Nom = "Transco Solutions",
                    Adresse = "789 Boulevard Commerce, Tanger",
                    Telephone = "**-**-**-**-**",
                    Email = "contact@transco.ma",
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
                    Immatriculation = "123ABC456",
                    CapaciteKg = 25000,
                    DateAcquisition = new DateTime(2022, 1, 15),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 1,
                    Marque = "Mercedes",
                    Modele = "Actros",
                    Immatriculation = "456DEF789",
                    CapaciteKg = 20000,
                    DateAcquisition = new DateTime(2021, 6, 20),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 2,
                    Marque = "Renault",
                    Modele = "T480",
                    Immatriculation = "789GHI012",
                    CapaciteKg = 18000,
                    DateAcquisition = new DateTime(2023, 3, 10),
                    EstActif = true
                },
                new Camion
                {
                    TransporteurId = 3,
                    Marque = "DAF",
                    Modele = "XF",
                    Immatriculation = "012JKL345",
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
                    Nom = "Zaari",
                    Prenom = "Ali",
                    Telephone = "06-11-22-33-44",
                    Email = "Ali.Zaari@expresstransport.ma",
                    NumeroPermis = "MA123456789",
                    DateEmbauche = new DateTime(2019, 9, 1),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 1,
                    Nom = "Rachidi",
                    Prenom = "Hamid",
                    Telephone = "06-55-66-77-88",
                    Email = "Rachidi.Hamid@expresstransport.ma",
                    NumeroPermis = "MA987654321",
                    DateEmbauche = new DateTime(2020, 3, 15),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 2,
                    Nom = "Idrissi",
                    Prenom = "Aziz",
                    Telephone = "06-99-88-77-66",
                    Email = "Idrissi.Aziz@logistiqueplus.ma",
                    NumeroPermis = "MA456789123",
                    DateEmbauche = new DateTime(2021, 1, 10),
                    EstActif = true
                },
                new Chauffeur
                {
                    TransporteurId = 3,
                    Nom = "Fadili",
                    Prenom = "Ayoub",
                    Telephone = "06-44-55-66-77",
                    Email = "Fadili.Ayoub@transco.ma",
                    NumeroPermis = "MA321654987",
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
                    Adresse = "100 Rue Commerce, Casa Blanca",
                    Telephone = "**-**-**-**-**",
                    Email = "delivery@supermarche-central.ma",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Petite Épicerie Local",
                    Adresse = "50 Rue Principale, Rabat",
                    Telephone = "**-**-**-**-**",
                    Email = "contact@epicerie-local.ma",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Hyper Magasin",
                    Adresse = "200 Avenue Commerçant, Rabat",
                    Telephone = "**-**-**-**-**",
                    Email = "logistique@hypermagasin.ma",
                    DateCreation = DateTime.Now
                },
                new Client
                {
                    Nom = "Magasin Spécialisé",
                    Adresse = "75 Boulevard Niche, Tanger",
                    Telephone = "**-**-**-**-**",
                    Email = "achat@specialise.ma",
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
                    CoutTotal = 4505.50m
                },
                new Tournee
                {
                    TransporteurId = 1,
                    CamionId = 2,
                    ChauffeurId = 2,
                    DateTournee = new DateTime(2024, 1, 16, 8, 30, 0),
                    StatutTournee = "En cours",
                    CoutTotal = 3500.00m
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
                    CoutTotal = 5207.50m
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
                    Adresse = "100 Rue Commerce, Casa Blanca",
                    PoidsKg = 5000,
                    DateLivraison = new DateTime(2024, 1, 15, 10, 30, 0),
                    StatutLivraison = "Livrée",
                    DateRealisation = new DateTime(2024, 1, 15, 10, 35, 0)
                },
                new Livraison
                {
                    TourneeId = 1,
                    ClientId = 2,
                    Adresse = "50 Rue Principale, Rabat",
                    PoidsKg = 3000,
                    DateLivraison = new DateTime(2024, 1, 15, 16, 0, 0),
                    StatutLivraison = "Livrée",
                    DateRealisation = new DateTime(2024, 1, 15, 16, 5, 0)
                },
                new Livraison
                {
                    TourneeId = 2,
                    ClientId = 3,
                    Adresse = "200 Avenue Commerçant, Rabat",
                    PoidsKg = 4500,
                    DateLivraison = new DateTime(2024, 1, 16, 14, 0, 0),
                    StatutLivraison = "En attente",
                    DateRealisation = null
                },
                new Livraison
                {
                    TourneeId = 3,
                    ClientId = 4,
                    Adresse = "75 Boulevard Niche, Tanger",
                    PoidsKg = 2000,
                    DateLivraison = new DateTime(2024, 1, 17, 11, 30, 0),
                    StatutLivraison = "En attente",
                    DateRealisation = null
                },
                new Livraison
                {
                    TourneeId = 4,
                    ClientId = 1,
                    Adresse = "100 Rue Commerce, Casa Blanca",
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
                new Cout { TourneeId = 1, TypeCout = "Carburant", Montant = 2500.00m, Description = "Carburant Casa Blanca - Rabat (Diesel)", DateCout = DateTime.Now },
                new Cout { TourneeId = 1, TypeCout = "Péage", Montant = 1200.00m, Description = "Péage autoroute Rabat", DateCout = DateTime.Now },
                new Cout { TourneeId = 1, TypeCout = "Maintenance", Montant = 805.50m, Description = "Révision mineure et AdBlue", DateCout = DateTime.Now },
                new Cout { TourneeId = 2, TypeCout = "Carburant", Montant = 2000.00m, Description = "Carburant Rabat - Tanger (Essence)", DateCout = DateTime.Now },
                new Cout { TourneeId = 2, TypeCout = "Péage", Montant = 1500.00m, Description = "Péage autoroute Tanger", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Carburant", Montant = 3500.00m, Description = "Carburant Casa Blanca - Tanger (long trajet)", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Maintenance", Montant = 1007.50m, Description = "Révision technique véhicule", DateCout = DateTime.Now },
                new Cout { TourneeId = 4, TypeCout = "Péage", Montant = 700.00m, Description = "Péage autoroute sections multiples", DateCout = DateTime.Now }
            };

            context.Couts.AddRange(couts);
            context.SaveChanges();
        }
    }
}
