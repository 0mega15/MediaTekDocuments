using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class EtatTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string id = "1";
            string libelle = "neuf";

            Etat etat = new Etat(id, libelle);

            Assert.AreEqual(libelle, etat.Libelle);
        }
    }
}
