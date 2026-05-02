using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using MediaTekDocuments.view;
using Serilog;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = ConfigurationManager.AppSettings["UriApi"];
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/errorlog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            String authenticationString;
            try
            {
                authenticationString = ConfigurationManager.AppSettings["ApiKeyVal"];
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Access.Access() - Erreur lors de la tentative de connexion à l'API : {0}", e.Message);
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        public List<Connexion> GetConnexion(object data)
        {
            String jsonData = ConvertToJson("data", data);
            List<Connexion> connexions = TraitementRecup<Connexion>(GET, "connexion/" + jsonData, null);
            return connexions;
        }
        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }
        /// <summary>
        /// Retourne tous les suivis d'un document à partir de la BDD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Suivi> lessuivis = TraitementRecup<Suivi>(GET, "suivi/" + jsonIdDocument, null);
            return lessuivis;
        }
        /// <summary>
        /// Retourne tous les abonnements d'une revue à partir de la BDD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Abonnement> GetAllAbonnements(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdDocument, null);
            return lesAbonnements;
        }
        /// <summary>
        /// Récupère la liste de tous les exemplaires d'un type donné : 0 = Livre / 1 = Revue / 2 = DVD
        /// </summary>
        /// <param name="idExemplaire"></param>
        /// <returns></returns>
        public List<Exemplaire> GetAllExemplairesType(string idExemplaire)
        {
            String jsonIdDocument = ConvertToJson("id", idExemplaire);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaireglobal/" + jsonIdDocument, null);
            return lesExemplaires;
        }
        /// <summary>
        /// Récupère la liste de tous les Etats (neuf, usagé, etc.)
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            List<Etat> lesEtats = TraitementRecup<Etat>(GET, "etat", null);
            return lesEtats;
        }
        /// <summary>
        /// Retourne la liste de tous les abonnements arrivant à expiration dans les 30 prochains jours
        /// </summary>
        /// <returns></returns>
        public List<InfosExpiration> GetAbonnementExpiration()
        {
            List<InfosExpiration> lesAbonnementExpirations = TraitementRecup<InfosExpiration>(GET, "finAbonnement", null);
            return lesAbonnementExpirations;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = ConvertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex,"Access.CreerExemplaire - Erreur lors de la création d'un exemplaire : {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modifie un exemplaire global
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns></returns>
        public bool ModifierExemplaire(Exemplaire exemplaire)
        {
            string jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(PUT, "exemplaireglobal", "id=null&champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime un exemplaire global
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns></returns>
        public bool SupprimerExemplaire(Exemplaire exemplaire)
        {
            string jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(DELETE, "exemplaireglobal", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Ajoute le livre dans la base de données
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool CreerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
            Console.WriteLine(jsonLivre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre", "champs=" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.CreerLivre - Erreur lors de la création d'un livre : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Modifie un livre sélectionné dans la base de données
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
        public bool ModifierLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre, new CustomDateTimeConverter());
            Console.WriteLine(jsonLivre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre", "champs=" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.ModifierLivre - Erreur lors de la modification d'un livre : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime un livre sélectionné dans la base de données
        /// </summary>
        /// <param name="livre"></param>
        /// <returns></returns>
    public bool SupprimerLivre(Livre livre)
    {
        try
        {
            var Id = livre.Id;
            var livreData = new { Id };
            string jsonLivre = JsonConvert.SerializeObject(livreData, new CustomDateTimeConverter());

            JObject retour = api.RecupDistant(DELETE, "livre", "champs=" + jsonLivre);

            string code = (string)retour["code"];
            string message = (string)retour["message"];
            Console.WriteLine($"Code : {code}, Message : {message}");

            return code.Equals("200");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nStack Trace : {ex.StackTrace}");
            MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Access.SupprimerLivre - Erreur lors de la suppresion d'un livre : {0}", ex.Message);
            return false;
        }
    }
        /// <summary>
        /// Ajoute le dvd dans la base de données
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool CreerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd, new CustomDateTimeConverter());
            Console.WriteLine(jsonDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd", "champs=" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.CreerExemplaire - Erreur lors de la création d'un exemplaire : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Modifie un dvd sélectionné dans la base de données
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool ModifierDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd, new CustomDateTimeConverter());
            Console.WriteLine(jsonDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd", "champs=" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.ModifierDvd - Erreur lors de la modification d'un dvd : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime un dvd sélectionné dans la base de données
        /// </summary>
        /// <param name="dvd"></param>
        /// <returns></returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            try
            {
                var Id = dvd.Id;
                var dvdData = new { Id };
                string jsonDvd = JsonConvert.SerializeObject(dvdData, new CustomDateTimeConverter());

                JObject retour = api.RecupDistant(DELETE, "dvd", "champs=" + jsonDvd);

                string code = (string)retour["code"];
                string message = (string)retour["message"];
                Console.WriteLine($"Code : {code}, Message : {message}");

                return code.Equals("200");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStack Trace : {ex.StackTrace}");
                Log.Error(ex, "Access.SupprimerDvd - Erreur lors de la suppresion d'un dvd : {0}", ex.Message);
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        /// <summary>
        /// Ajoute le Revue dans la base de données
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool CreerRevue(Revue revue)
        {
            String jsonrevue = JsonConvert.SerializeObject(revue, new CustomDateTimeConverter());
            Console.WriteLine(jsonrevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue", "champs=" + jsonrevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.CreerRevue - Erreur lors de la création d'une revue : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Modifie un Revue sélectionné dans la base de données
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool ModifierRevue(Revue revue)
        {
            String jsonrevue = JsonConvert.SerializeObject(revue, new CustomDateTimeConverter());
            Console.WriteLine(jsonrevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue", "champs=" + jsonrevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.ModifierRevue - Erreur lors de la création d'une revue : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime un dvd sélectionné dans la base de données
        /// </summary>
        /// <param name="revue"></param>
        /// <returns></returns>
        public bool SupprimerRevue(Revue revue)
        {
            try
            {
                var Id = revue.Id;
                var revueData = new { Id };
                string jsonrevue = JsonConvert.SerializeObject(revueData, new CustomDateTimeConverter());

                JObject retour = api.RecupDistant(DELETE, "revue", "champs=" + jsonrevue);

                string code = (string)retour["code"];
                string message = (string)retour["message"];
                Console.WriteLine($"Code : {code}, Message : {message}");

                return code.Equals("200");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nStack Trace : {ex.StackTrace}");
                Log.Error(ex, "Access.SupprimerRevue - Erreur lors de la suppression d'une revue : {0}", ex.Message);
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// Appelle l'API pour créer un suivi d'un document dans la base de données
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public bool CreerSuivi(Suivi suivi)
        {
            String jsonSuivi = JsonConvert.SerializeObject(suivi, new CustomDateTimeConverter());
            try
            {
                List<Suivi> liste = TraitementRecup<Suivi>(POST, "suivi", "champs=" + jsonSuivi);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.CreerSuivi - Erreur lors de la création d'un suivi : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Appelle l'API pour modifier un suivi d'un document dans la base de données
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public bool ModifiSuivi(Suivi suivi)
        {
            String jsonSuivi = JsonConvert.SerializeObject(suivi, new CustomDateTimeConverter());
            try
            {
                List<Suivi> liste = TraitementRecup<Suivi>(PUT, "suivi", "id=null&champs=" + jsonSuivi);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Appelle l'API pour supprimer un suivi d'un document dans la base de données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerSuivi(string id)
        {
            try
            {
                String jsonIdSuivi = ConvertToJson("id", id);
                List<Suivi> liste = TraitementRecup<Suivi>(DELETE, "suivi/"+jsonIdSuivi, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.SupprimerSuivi - Erreur lors de la suppression d'un suivi : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Appelle l'API pour créer un abonnement d'une revue dans la base de données
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.CreerAbonnement - Erreur lors de la création d'un abonnement : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Appelle l'API pour modifier un abonnement d'une revue dans la base de données
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool ModifiAbonnement(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(PUT, "abonnement", "id=null&champs=" + jsonAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.ModifAbonnement - Erreur lors de la modification d'un abonnement : {0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Appelle l'API pour supprimer un abonnement d'une revue dans la base de données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerAbonnement(string id)
        {
            try
            {
                String jsonIdAbonnement = ConvertToJson("id", id);
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "abonnement/" + jsonIdAbonnement, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex, "Access.SupprimerAbonnement - Erreur lors de la suppression d'un abonnement : {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Log.Error("Access.TraitementRecup - Le code de retour de l'API doit être 200: code={0}, message={1}", code, (String)retour["message"]);
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Log.Fatal(e, "Access.TraitementRecup - Erreur lors de la tentative d'accès à l'API: {0}", e.Message);
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private static String ConvertToJson(Object nom, Object valeur)
        {
            var dictionary = new Dictionary<object, object>
            {
                { nom, valeur }
            };
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }


    }
}
