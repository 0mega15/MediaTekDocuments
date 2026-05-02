using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class GenreTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string id = "1";
            string libelle = "Enfants";

            Genre legenre = new Genre(id, libelle);

            Assert.AreEqual(id, legenre.Id);
        }
    }
}
