using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTest.model
{
    [TestClass]
    internal class ConnexionTest
    {
        [TestMethod]
        public void ConstructeurTest()
        {
            string login = "admin";
            string password = "1234";
            int idService = 2;

            Connexion connexion = new Connexion(login, password, idService);

            Assert.AreEqual(login, connexion.Login);
            Assert.AreEqual(password, connexion.Password);
            Assert.AreEqual(idService, connexion.IdService);
        }
    }
}
