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
    public class DocumentTests
    {
        private const string Id = "00001";
        private const string Titre = "Le temps des paquerettes";
        private const string Image = "";
        private const string idGenre = "01";
        private const string Genre = "fiction";
        private const string idPublic = "01";
        private const string Public = "Ado";
        private const string idRayon = "01";
        private const string Rayon = "A09";
        private static readonly Document document = new Document(Id, Titre, Image, idGenre, Genre, idPublic, Public, idRayon, Rayon);


        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(Id, document.Id);
            Assert.AreEqual(Titre, document.Titre);
            Assert.AreEqual(Image, document.Image);
            Assert.AreEqual(idGenre, document.idGenre);
            Assert.AreEqual(Genre, document.Genre);
            Assert.AreEqual(idPublic, document.idPublic);
            Assert.AreEqual(Public, document.Public);
            Assert.AreEqual(idRayon, document.idRayon);
            Assert.AreEqual(Rayon, document.Rayon);
        }
    }
}