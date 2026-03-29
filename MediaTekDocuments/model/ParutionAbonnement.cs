using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class ParutionAbonnement
    {
        public bool ParutionDansAbonnement(DateTime commande, DateTime fin_abonnement, DateTime date_parution)
        {
            if (date_parution >= commande && date_parution <= fin_abonnement)
            {
                return true;
            }
            return false;
        }
    }
}
