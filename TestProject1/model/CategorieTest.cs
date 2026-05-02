using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTest.model
{
    internal class CategorieTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string id = "1";
            string libelle = "Horreur";

            Categorie categorie = new Categorie(id, libelle);

            Assert.AreEqual(id, categorie.Id);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Categorie categorie = new Categorie("1", "Horreur");
            string result = categorie.ToString();
            Assert.AreEqual("Horreur", result);
        }
    }
}
