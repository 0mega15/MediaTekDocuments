using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest.model
{
    public class SuiviTest
    {
        [TestMethod()]
        public void ConstructeurTest()
        {
            DateTime dateSuivi = new DateTime(2026, 04, 29);
            DateTime dateCommande = new DateTime(2026, 04, 01);

            Suivi suivi = new Suivi(dateSuivi, "en cours", "00017", 2, dateCommande, 45.0, "10", 1);

            Assert.AreEqual(dateSuivi, suivi.DateSuivi);
            
        }
    }
}
