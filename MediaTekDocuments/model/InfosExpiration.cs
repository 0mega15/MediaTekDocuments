using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class InfosExpiration
    {
        public string Titre { get; set;  }

        public DateTime DateFinAbonnement { get; set; }

        public InfosExpiration(string titre, DateTime dateFinAbonnement ) 
        {
            this.Titre = titre;
            this.DateFinAbonnement = dateFinAbonnement;
        }
    }
}
