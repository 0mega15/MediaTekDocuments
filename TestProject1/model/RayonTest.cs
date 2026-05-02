using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class RayonTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string id = "1";
            string libelle = "le rayon";
            Rayon rayon = new Rayon(id, libelle);
            Assert.AreEqual(libelle, rayon.Libelle);
        }
    }
}
