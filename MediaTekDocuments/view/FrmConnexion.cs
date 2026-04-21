using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public partial class FrmConnexion : Form
    {
        private readonly FrmConnexionController controller;

        internal FrmConnexion()
        {
            InitializeComponent();
            this.controller = new FrmConnexionController();
        }

        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (txbLogin.Text != string.Empty && txbPassword.Text != string.Empty)
            {
                var data = new
                {
                    login = txbLogin.Text,
                    password = txbPassword.Text
                };
                List<Connexion> x = controller.GetConnexion(data);
                if (x.Count > 0)
                {
                    if (x[0].IdService == 3)
                    {
                        MessageBox.Show("Cette application n'est pas accessible pour vous");
                    }
                    else 
                    {
                        FrmMediatek frmMediatek = new FrmMediatek(x[0].IdService);
                        frmMediatek.ShowDialog();
                    }
                        
                }
                else
                {
                    MessageBox.Show("Login ou mot de passe incorrect");
                }
            }
        }
    }
}
