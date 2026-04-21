using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Connexion
    {
        public string Login { get; }
        public string Password { get; }
        public int IdService { get; }

        public Connexion(string login, string password, int idService)
        {
            Login = login;
            Password = password;
            IdService = idService;
        }
    }
}
