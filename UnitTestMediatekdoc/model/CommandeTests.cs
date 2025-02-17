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
    public class CommandeTests
    {
        private const string id = "001";
        private static DateTime dateCommande = DateTime.Parse("2025-02-16");
        private const double montant = 12;
        private static readonly Commande commande = new Commande(id, dateCommande, montant);


        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, commande.id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commande.dateCommande, "devrait réussir : dateCommande valorisée");
            Assert.AreEqual(montant, commande.montant, "devrait réussir : montant valorisé");
        }
    }
}