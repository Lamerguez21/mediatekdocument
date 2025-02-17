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
    public class SuiviTests
    {
        private const string id = "01";
        private const string etapeSuivi = "en cours";
        private static readonly Suivi suivi = new Suivi(id, etapeSuivi);

        [TestMethod()]
        public void SuiviTest()
        {
            Assert.AreEqual(id, suivi.id, "devrait réussir : id valorisé");
            Assert.AreEqual(etapeSuivi, suivi.etapeSuivi, "devrait réussir : etapeSuivi valorisée");
        }
    }
}