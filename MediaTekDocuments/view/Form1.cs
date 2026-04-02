using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    public partial class Form1 : Form
    {

        private readonly BindingSource bdgAbonnementExpiration = new BindingSource();
        private readonly FrmMediatekController controller;

        public Form1()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemplirRevuesListeCommandeTab(List<InfosExpiration> infos)
        {
            bdgAbonnementExpiration.DataSource = infos;
            dataGridView1.DataSource = bdgAbonnementExpiration;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RemplirRevuesListeCommandeTab(controller.GetAbonnementExpiration());
        }
    }
}
