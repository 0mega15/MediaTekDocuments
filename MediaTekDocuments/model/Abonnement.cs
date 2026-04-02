using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Abonnement
    {
        public DateTime DateCommande { get; set; }
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }
        public double Montant { get; set; }
        public string Id { get; set; }

        public Abonnement(DateTime dateCommande, DateTime dateFinAbonnement, string idRevue, double montant, string id)
        {
            this.DateCommande = dateCommande;
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
            this.Montant = montant;
            this.Id = id;
        }
    }
}
