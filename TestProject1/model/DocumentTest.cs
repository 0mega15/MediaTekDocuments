using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{

    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void ConstructeurTest()
        {
            // Arrange
            string id = "154";
            string titre = "LivreTitre";
            string image = "";
            string idGenre = "1";
            string genre = "Roman";
            string idPublic = "2";
            string lePublic = "Enfants";
            string idRayon = "3";
            string rayon = "Littérature";

            // Act
            Document doc = new Document(
                id,
                titre,
                image,
                idGenre,
                genre,
                idPublic,
                lePublic,
                idRayon,
                rayon
            );

            // Assert
            Assert.AreEqual(id, doc.Id);
            Assert.AreEqual(titre, doc.Titre);
            Assert.AreEqual(image, doc.Image);
            Assert.AreEqual(idGenre, doc.IdGenre);
            Assert.AreEqual(genre, doc.Genre);
            Assert.AreEqual(idPublic, doc.IdPublic);
            Assert.AreEqual(lePublic, doc.Public);
            Assert.AreEqual(idRayon, doc.IdRayon);
            Assert.AreEqual(rayon, doc.Rayon);
        }
    }
}
