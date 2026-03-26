using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Suivi
    {
        public DateTime DateSuivi { get; set; }
        public string Etat { get; set; }
        public string IdCommandeDocument { get; set; }
        public int NbExemplaire { get; set; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }
        public string IdCommande { get; set; }
        public int Id { get; set; }
        public Suivi(DateTime dateSuivi, string etat, string idCommandeDocument, int nbExemplaire, DateTime dateCommande, double montant, string idCommande, int id)
        {
            this.DateSuivi = dateSuivi;
            this.Etat = etat;
            this.IdCommandeDocument = idCommandeDocument;
            this.NbExemplaire = nbExemplaire;
            this.DateCommande = dateCommande;
            this.Montant = montant;
            this.IdCommande = idCommande;
            this.Id = id;
        }
    }
}
