using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class InfosExpirationTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            string titre = "le titre du document";
            DateTime dateExpiration = new DateTime(2025, 04, 10);
            InfosExpiration infosExpiration = new InfosExpiration(titre, dateExpiration);
            Assert.AreEqual(titre, infosExpiration.Titre);
        }
    }
}
