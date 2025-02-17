using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class AbonnementTests
    {
        private const string id = "001";
        private static DateTime dateCommande = DateTime.Parse("2025-02-16");
        private const double montant = 12;
        private const string idRevue = "10001";
        private static DateTime dateFinAbonnement = DateTime.Parse("2025-02-10");
        private const string titre = "Géo";
        private static readonly Abonnement abonnement = new Abonnement(id, dateCommande, montant, idRevue, dateFinAbonnement, titre);


        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abonnement.id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, abonnement.dateCommande, "devrait réussir : dateCommande valorisée");
            Assert.AreEqual(montant, abonnement.montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(idRevue, abonnement.idRevue, "devrait réussir : idRevue valorisé");
            Assert.AreEqual(dateFinAbonnement, abonnement.dateFinAbonnement, "devrait réussir : dateFinAbonnement valorisé");
            Assert.AreEqual(titre, abonnement.titre, "devrait réussir : titre valorisé");
        }
    }
}