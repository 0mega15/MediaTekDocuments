using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    class FrmConnexionController
    {
        private readonly Access access;

        public FrmConnexionController()
        {
            access = Access.GetInstance();
        }

        public List<Connexion> GetConnexion(object data)
        {
            return access.GetConnexion(data);
        }
    }
}
