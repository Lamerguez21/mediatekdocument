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
    public class UtilisateurTests
    {
        private const string id = "01";
        private static string login = "Marie";
        private const string password = "Marie04";
        private const string idService = "01";
        private static readonly Utilisateur utilisateur = new Utilisateur(id, login, password, idService);

        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(id, utilisateur.id, "devrait réussir : id valorisé");
            Assert.AreEqual(login, utilisateur.login, "devrait réussir : login valorisée");
            Assert.AreEqual(password, utilisateur.password, "devrait réussir : password valorisé");
            Assert.AreEqual(idService, utilisateur.idService, "devrait réussir : idService valorisé");
        }
    }
}