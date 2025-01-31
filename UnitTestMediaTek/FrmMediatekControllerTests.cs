using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MediaTekDocuments.controller;

namespace MediaTekDocuments.Tests
{
    [TestClass]
    public class FrmMediatekControllerTests
    {
        private FrmMediatekController controller;

        [TestInitialize] // Cette méthode s'exécute avant chaque test
        public void Setup()
        {
            controller = new FrmMediatekController();
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateDansIntervalle_ReturnsTrue()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2024, 6, 15); // Date entre les deux

            // Act
            bool result = controller.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsTrue(result, "La méthode doit retourner 'true' si la date est dans l'intervalle.");
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateAvantCommande_ReturnsFalse()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2023, 12, 31); // Date avant la commande

            // Act
            bool result = controller.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsFalse(result, "La méthode doit retourner 'false' si la date est avant la commande.");
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateApresFinAbonnement_ReturnsFalse()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2025, 1, 1); // Date après l'abonnement

            // Act
            bool result = controller.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsFalse(result, "La méthode doit retourner 'false' si la date est après la fin de l'abonnement.");
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateEgaleCommande_ReturnsTrue()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2024, 1, 1); // Exactement la date de commande

            // Act
            bool result = controller.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsTrue(result, "La méthode doit retourner 'true' si la date de parution est exactement la date de commande.");
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateEgaleFinAbonnement_ReturnsTrue()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2024, 12, 31); // Exactement la date de fin d'abonnement

            // Act
            bool result = controller.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsTrue(result, "La méthode doit retourner 'true' si la date de parution est exactement la date de fin d'abonnement.");
        }
    }
}
