using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    public class AbonnementTest
    {
        [TestMethod]
        public void ConstructeurTest()
        {
            DateTime dateCommande = new DateTime(2026, 04, 29);
            DateTime dateFin = new DateTime(2027, 04, 29);
            string idRevue = "10002";
            double montant = 120.0;
            string id = "10";

            Abonnement abonnement = new Abonnement(dateCommande, dateFin, idRevue, montant, id);

            Assert.AreEqual(dateCommande, abonnement.DateCommande);
            Assert.AreEqual(dateFin, abonnement.DateFinAbonnement);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
            Assert.AreEqual(montant, abonnement.Montant);
            Assert.AreEqual(id, abonnement.Id);
        }
    }
}
