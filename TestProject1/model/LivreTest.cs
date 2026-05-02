using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class LivreTest
    {
        [TestMethod]
        public void ConstructeurTest()
        {
            string id = "1";
            string titre = "le titre du livre";
            string image = "le chemin de l'image";
            string isbn = "123456789";
            string auteur = "l'auteur";
            string resume = "le resume";
            string idGenre = "1";
            string genre = "le genre";
            string idPublic = "1";
            string lePublic = "le public";
            string idRayon = "4";
            string rayon = "le rayon";
            Livre leLivre = new Livre(id, titre, image, isbn, auteur, resume, idGenre, genre, idPublic, lePublic, idRayon, rayon);
            Assert.AreEqual(titre, leLivre.Titre);
        }
    }
}
