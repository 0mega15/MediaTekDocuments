using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public sealed class FrmMediatekTest
    {
        private ParutionAbonnement parution = new ParutionAbonnement();

        [TestMethod]
        public void ParutionApresInterval()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime fin_abonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2026, 1, 1);

            bool result = parution.ParutionDansAbonnement(dateCommande, fin_abonnement, dateParution);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParutionAvantInterval()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime fin_abonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2024, 12, 31);
            bool result = parution.ParutionDansAbonnement(dateCommande, fin_abonnement, dateParution);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParutionDansInterval()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime fin_abonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2025, 6, 1);
            bool result = parution.ParutionDansAbonnement(dateCommande, fin_abonnement, dateParution);
            Assert.IsTrue(result);
        }
    }
}

