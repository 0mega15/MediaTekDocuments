using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class PublicTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string id = "1";
            string libelle = "Adulte";
            Public lepublic = new Public(id, libelle);
            Assert.AreEqual(libelle, lepublic.Libelle);
        }
    }
}
