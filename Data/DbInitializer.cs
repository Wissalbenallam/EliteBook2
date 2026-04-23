using Microsoft.EntityFrameworkCore;
using TMS_Project.Models;

namespace TMS_Project.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TmsDbContext context)
        {
            context.Database.EnsureCreated();
            EnsureDemandeLivraisonSchema(context);

            if (context.Transporteurs.Any())
            {
                return;
            }

            var transporteurs = new List<Transporteur>
            {
                new Transporteur { Nom = "Express Transport", Adresse = "Casablanca", Telephone = "0600000001", Email = "contact@express.ma", DateCreation = DateTime.Now },
                new Transporteur { Nom = "Logistique Plus", Adresse = "Rabat", Telephone = "0600000002", Email = "info@logistique.ma", DateCreation = DateTime.Now },
                new Transporteur { Nom = "Transco Solutions", Adresse = "Tanger", Telephone = "0600000003", Email = "contact@transco.ma", DateCreation = DateTime.Now }
            };
            context.Transporteurs.AddRange(transporteurs);
            context.SaveChanges();

            var camions = new List<Camion>
            {
                new Camion { TransporteurId = 1, Marque = "Volvo", Modele = "FH16", Immatriculation = "123ABC456", CapaciteKg = 25000, DateAcquisition = new DateTime(2022, 1, 15), EstActif = true },
                new Camion { TransporteurId = 2, Marque = "Renault", Modele = "T480", Immatriculation = "789GHI012", CapaciteKg = 18000, DateAcquisition = new DateTime(2023, 3, 10), EstActif = true }
            };
            context.Camions.AddRange(camions);
            context.SaveChanges();

            var chauffeurs = new List<Chauffeur>
            {
                new Chauffeur { TransporteurId = 1, Nom = "Zaari", Prenom = "Ali", Telephone = "0611223344", Email = "ali@express.ma", NumeroPermis = "MA123456", DateEmbauche = new DateTime(2019, 9, 1), EstActif = true },
                new Chauffeur { TransporteurId = 2, Nom = "Idrissi", Prenom = "Aziz", Telephone = "0699887766", Email = "aziz@logistique.ma", NumeroPermis = "MA456789", DateEmbauche = new DateTime(2021, 1, 10), EstActif = true }
            };
            context.Chauffeurs.AddRange(chauffeurs);
            context.SaveChanges();

            var clients = new List<Client>
            {
                new Client { Nom = "Supermarché Central", Adresse = "Casablanca", Telephone = "0601010101", Email = "client1@demo.ma", DateCreation = DateTime.Now },
                new Client { Nom = "Magasin Local", Adresse = "Rabat", Telephone = "0602020202", Email = "client2@demo.ma", DateCreation = DateTime.Now }
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            var tournees = new List<Tournee>
            {
                new Tournee { TransporteurId = 1, CamionId = 1, ChauffeurId = 1, DateTournee = DateTime.Today.AddDays(1), StatutTournee = "Planifiée", CoutTotal = 0m },
                new Tournee { TransporteurId = 2, CamionId = 2, ChauffeurId = 2, DateTournee = DateTime.Today.AddDays(2), StatutTournee = "En cours", CoutTotal = 0m }
            };
            context.Tournees.AddRange(tournees);
            context.SaveChanges();
        }

        private static void EnsureDemandeLivraisonSchema(TmsDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[DemandesLivraison]', N'U') IS NULL
BEGIN
    CREATE TABLE [DemandesLivraison](
        [DemandeLivraisonId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ClientId] INT NULL,
        [TypeService] NVARCHAR(100) NOT NULL DEFAULT 'Standard',
        [DescriptionMarchandise] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [Adresse] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [Quantite] INT NOT NULL DEFAULT 1,
        [PoidsKg] INT NOT NULL DEFAULT 0,
        [DateSouhaitee] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [Statut] NVARCHAR(100) NOT NULL DEFAULT 'En attente',
        [MontantEstime] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [CamionAffecteId] INT NULL,
        [ChauffeurAffecteId] INT NULL,
        [NumeroRecu] NVARCHAR(100) NOT NULL DEFAULT '',
        [DateValidation] DATETIME2 NULL,
        [CommentaireValidation] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
    )
END");

            context.Database.ExecuteSqlRaw(@"
IF COL_LENGTH('DemandesLivraison', 'TypeService') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [TypeService] NVARCHAR(100) NOT NULL DEFAULT 'Standard';
IF COL_LENGTH('DemandesLivraison', 'DescriptionMarchandise') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [DescriptionMarchandise] NVARCHAR(MAX) NOT NULL DEFAULT '';
IF COL_LENGTH('DemandesLivraison', 'Quantite') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [Quantite] INT NOT NULL DEFAULT 1;
IF COL_LENGTH('DemandesLivraison', 'NumeroRecu') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [NumeroRecu] NVARCHAR(100) NOT NULL DEFAULT '';
IF COL_LENGTH('DemandesLivraison', 'DateValidation') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [DateValidation] DATETIME2 NULL;
IF COL_LENGTH('DemandesLivraison', 'CommentaireValidation') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [CommentaireValidation] NVARCHAR(MAX) NOT NULL DEFAULT '';
");
        }
    }
}
