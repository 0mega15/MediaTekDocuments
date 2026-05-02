using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class RevueTest
    {
        [TestMethod]
        public void ConstructeurTest()
        {
            string id = "1";
            string titre = "le titre de la revue";
            string image = "le chemin de l'image";
            string periodicite = "30";
            string idGenre = "1";
            string genre = "le genre";
            string idPublic = "1";
            string lePublic = "le public";
            string idRayon = "4";
            string rayon = "le rayon";
            int delaiMiseADispo = 15;
            Revue laRevue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
            Assert.AreEqual(titre, laRevue.Titre);
        }
    }
}
