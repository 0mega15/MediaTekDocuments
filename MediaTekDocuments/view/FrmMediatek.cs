using MediaTekDocuments.controller;
using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        private readonly BindingSource bdgActionsGenres = new BindingSource();
        private readonly BindingSource bdgActionsPublics = new BindingSource();
        private readonly BindingSource bdgActionsRayons = new BindingSource();

        private readonly int LevelId;
        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(int levelId)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            new FrmAvertissement().ShowDialog();
            LevelId = levelId;
        }

        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            Verrouillage();
        }
        public void Verrouillage()
        {
            if (LevelId == 2)
            {
                tabOngletsApplication.TabPages.Remove(tabCommandeDvd);
                tabOngletsApplication.TabPages.Remove(tabCommandeLivre);
                tabOngletsApplication.TabPages.Remove(tabCommandeRevues);
                tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
               
                grpLivresActions.Visible = false;
                grpLivresActions.Enabled = false;
                cbxActionsLivresGenres.Enabled = false;
                cbxActionsLivresPublics.Enabled = false;
                cbxActionsLivresRayons.Enabled = false;
                cbxActionsLivresGenres.Visible = false;
                cbxActionsLivresPublics.Visible = false;
                cbxActionsLivresRayons.Visible = false;

                grpDVDActions.Visible = false;
                grpDVDActions.Enabled = false;
                cbxActionsDvdGenres.Enabled = false;
                cbxActionsDvdPublics.Enabled = false;
                cbxActionsDvdRayons.Enabled = false;
                cbxActionsDvdGenres.Visible = false;
                cbxActionsDvdPublics.Visible = false;
                cbxActionsDvdRayons.Visible = false;

                grpRevuesActions.Visible = false;
                grpRevuesActions.Enabled = false;
                cbxActionsRevuesGenres.Enabled = false;
                cbxActionsRevuesPublics.Enabled = false;
                cbxActionsRevuesRayons.Enabled = false;
                cbxActionsRevuesGenres.Visible = false;
                cbxActionsRevuesPublics.Visible = false;
                cbxActionsRevuesRayons.Visible = false;


            }
        }
        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public static void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Ajoute les champs remplis à la liste des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresActionsAjout_Click(object sender, EventArgs e)
        {
            if (btnLivresActionsModifier.Enabled)
            {
                /// Phase 1 : Indique au code que l'on souhaite ajouter un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                VisibiliteChamps(false, "Ajout");
                txbLivresNumero.Text = (lesLivres.Count + 1).ToString("D5");

            }
            else
            {
                /// Phase 2 : Ajoute les champs remplis à la liste.
                /// 
                var request = MessageBox.Show("Souhaitez-vous ajouter ce livre au catalogue ?", 
                    "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbLivresTitre.Text == null || txbLivresIsbn.Text == null || txbLivresAuteur.Text == null || txbLivresCollection.Text == null
                        || cbxActionsLivresGenres.SelectedIndex == -1 || cbxActionsLivresPublics.SelectedIndex == -1 || cbxActionsLivresRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsLivresGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsLivresPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsLivresRayons.SelectedItem;

                        Livre nvLivre = new Livre(
                            txbLivresNumero.Text,
                            txbLivresTitre.Text,
                            txbLivresImage.Text,
                            txbLivresIsbn.Text,
                            txbLivresAuteur.Text,
                            txbLivresCollection.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle
                            );

                        if (controller.CreerLivre(nvLivre))
                        {
                            VisibiliteChamps(true, "Ajout");

                            MessageBox.Show("Livre ajouté avec succès.");
                            lesLivres = controller.GetAllLivres();
                            RemplirLivresListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChamps(true, "Ajout");
                }
            }
        }
        /// <summary>
        /// Modifie le champ sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresActionsModifier_Click(object sender, EventArgs e)
        {
            /// sauvegarde les anciennes valeurs dans des variables. 
            if (btnLivresActionsAjout.Enabled)
            {

                /// Phase 1 : Indique au code que l'on souhaite modifier un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                if (dgvLivresListe.SelectedRows.Count == 1)
                {
                    VisibiliteChamps(false, "Modifier");
                    cbxActionsLivresGenres.Text = txbLivresGenre.Text;
                    cbxActionsLivresPublics.Text = txbLivresPublic.Text;
                    cbxActionsLivresRayons.Text = txbLivresRayon.Text;
                }
                else
                {
                    MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                /// Phase 2 : Modifie le champ.
                /// 

                var request = MessageBox.Show("Souhaitez-vous modifier ce livre ?",
                 "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbLivresTitre.Text == null || txbLivresIsbn.Text == null || txbLivresAuteur.Text == null || txbLivresCollection.Text == null
                        || cbxActionsLivresGenres.SelectedIndex == -1 || cbxActionsLivresPublics.SelectedIndex == -1 || cbxActionsLivresRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsLivresGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsLivresPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsLivresRayons.SelectedItem;

                        Livre nvLivre = new Livre(
                            txbLivresNumero.Text,
                            txbLivresTitre.Text,
                            txbLivresImage.Text,
                            txbLivresIsbn.Text,
                            txbLivresAuteur.Text,
                            txbLivresCollection.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle
                            );

                        if (controller.ModifierLivre(nvLivre))
                        {
                            VisibiliteChamps(true, "Modifier");

                            MessageBox.Show("Livre modifié avec succès.");
                            lesLivres = controller.GetAllLivres();
                            RemplirLivresListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChamps(true, "Modifier");
                }
            }
        }
        /// <summary>
        /// Supprime le champ sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresActionsSupprimer_Click(object sender, EventArgs e)
        {
            VisibiliteChamps(false, "Supprimer");
                cbxActionsLivresGenres.Text = txbLivresGenre.Text;
                cbxActionsLivresPublics.Text = txbLivresPublic.Text;
                cbxActionsLivresRayons.Text = txbLivresRayon.Text;

                if (dgvLivresListe.SelectedRows.Count == 1)
                {
                    var request = MessageBox.Show("Souhaitez-vous supprimer le livre suivant : " + txbLivresTitre.Text + " ?",
                                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (request == DialogResult.Yes)
                    {
                        Genre selectedGenre = (Genre)cbxActionsLivresGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsLivresPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsLivresRayons.SelectedItem;

                        Livre nvLivre = new Livre(
                            txbLivresNumero.Text,
                            txbLivresTitre.Text,
                            txbLivresImage.Text,
                            txbLivresIsbn.Text,
                            txbLivresAuteur.Text,
                            txbLivresCollection.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle
                            );

                        if (controller.SupprimerLivre(nvLivre))
                        {
                            VisibiliteChamps(true, "Supprimer");

                            MessageBox.Show("Livre supprimé avec succès.");
                            lesLivres = controller.GetAllLivres();
                            RemplirLivresListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        VisibiliteChamps(true, "Supprimer");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        /// <summary>
        /// Change la visibilité des champs pour l'ajout, la modification ou la suppression d'un livre.
        /// </summary>
        /// <param name="result"></param>
        private void VisibiliteChamps(bool result, string bouton)
        {
            if (bouton == "Ajout")
            {
                VideLivresInfos();
                dgvLivresListe.Visible = result;

                btnLivresActionsModifier.Enabled = result;
                btnLivresActionsSupprimer.Enabled = result;

                txbLivresAuteur.ReadOnly = result;
            }
            else if (bouton == "Modifier")
            {
                dgvLivresListe.Enabled = result;

                btnLivresActionsAjout.Enabled = result;
                btnLivresActionsSupprimer.Enabled = result;

                txbLivresAuteur.ReadOnly = result;
                
            }
            else
            {
                dgvLivresListe.Enabled = result;

                btnLivresActionsModifier.Enabled = result;
                btnLivresActionsAjout.Enabled = result;
            }
                txbLivresCollection.ReadOnly = result;
                txbLivresTitre.ReadOnly = result;
                txbLivresImage.ReadOnly = result;
                txbLivresIsbn.ReadOnly = result;
                /// genre
                RemplirComboCategorie(controller.GetAllGenres(), bdgActionsGenres, cbxActionsLivresGenres);
                cbxActionsLivresGenres.Visible = !result;
                /// public
                RemplirComboCategorie(controller.GetAllPublics(), bdgActionsPublics, cbxActionsLivresPublics);
                cbxActionsLivresPublics.Visible = !result;
                /// rayon
                RemplirComboCategorie(controller.GetAllRayons(), bdgActionsRayons, cbxActionsLivresRayons);
                cbxActionsLivresRayons.Visible = !result;
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }
        private void btnDVDActionsAjout_Click(object sender, EventArgs e)
        {
            if (btnDVDActionsModifier.Enabled)
            {
                /// Phase 1 : Indique au code que l'on souhaite ajouter un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                VisibiliteChampsDvd(false, "Ajout");
                txbDvdNumero.Text = "2" + (lesDvd.Count + 1).ToString("D4");

            }
            else
            {
                /// Phase 2 : Ajoute les champs remplis à la liste.
                /// 
                var request = MessageBox.Show("Souhaitez-vous ajouter ce DVD au catalogue ?",
                    "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbDvdTitre.Text == null || txbDvdRealisateur.Text == null || txbDvdSynopsis.Text == null || txbDvdImage.Text == null ||
                        txbDvdDuree.Text == null || txbDvdNumero.Text == null || cbxActionsDvdGenres.SelectedIndex == -1 || cbxActionsDvdPublics.SelectedIndex == -1 || cbxActionsDvdRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsDvdGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsDvdPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsDvdRayons.SelectedItem;

                        Dvd nvDvd = new Dvd(
                            txbDvdNumero.Text, 
                            txbDvdTitre.Text, 
                            txbDvdImage.Text, 
                            int.Parse(txbDvdDuree.Text), 
                            txbDvdRealisateur.Text, 
                            txbDvdSynopsis.Text, 
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle
                            );

                        if (controller.CreerDvd(nvDvd))
                        {
                            VisibiliteChampsDvd(true, "Ajout");

                            MessageBox.Show("Dvd ajouté avec succès.");
                            lesDvd = controller.GetAllDvd();
                            RemplirDvdListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChampsDvd(true, "Ajout");
                }
            }
        }

        private void btnDVDActionsModifier_Click(object sender, EventArgs e)
        {

            /// sauvegarde les anciennes valeurs dans des variables. 
            if (btnDVDActionsAjout.Enabled)
            {

                /// Phase 1 : Indique au code que l'on souhaite modifier un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                if (dgvDvdListe.SelectedRows.Count == 1)
                {
                    VisibiliteChampsDvd(false, "Modifier");
                    cbxActionsDvdGenres.Text = txbDvdGenre.Text;
                    cbxActionsDvdPublics.Text = txbDvdPublic.Text;
                    cbxActionsDvdRayons.Text = txbDvdRayon.Text;
                }
                else
                {
                    MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                /// Phase 2 : Modifie le champ.
                /// 

                var request = MessageBox.Show("Souhaitez-vous modifier ce DVD ?",
                 "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbDvdTitre.Text == null || txbDvdRealisateur.Text == null || txbDvdSynopsis.Text == null || txbDvdImage.Text == null ||
                        txbDvdDuree.Text == null || txbDvdNumero.Text == null || cbxActionsDvdGenres.SelectedIndex == -1 || cbxActionsDvdPublics.SelectedIndex == -1 || cbxActionsDvdRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsDvdGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsDvdPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsDvdRayons.SelectedItem;

                        Dvd nvDvd = new Dvd(
                            txbDvdNumero.Text,
                            txbDvdTitre.Text,
                            txbDvdImage.Text,
                            int.Parse(txbDvdDuree.Text),
                            txbDvdRealisateur.Text,
                            txbDvdSynopsis.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle
                            );

                        if (controller.ModifierDvd(nvDvd))
                        {
                            VisibiliteChampsDvd(true, "Modifier");

                            MessageBox.Show("Dvd modifié avec succès.");
                            lesDvd = controller.GetAllDvd();
                            RemplirDvdListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChampsDvd(true, "Modifier");
                }
            }
        }

        private void btnDVDActionsSupprimer_Click(object sender, EventArgs e)
        {
            VisibiliteChampsDvd(false, "Supprimer");
            cbxActionsDvdGenres.Text = txbDvdGenre.Text;
            cbxActionsDvdPublics.Text = txbDvdPublic.Text;
            cbxActionsDvdRayons.Text = txbDvdRayon.Text;

            if (dgvDvdListe.SelectedRows.Count == 1)
            {
                var request = MessageBox.Show("Souhaitez-vous supprimer le DVD suivant : " + txbDvdTitre.Text + " ?",
                                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (request == DialogResult.Yes)
                {
                    Genre selectedGenre = (Genre)cbxActionsDvdGenres.SelectedItem;
                    Public selectedPublic = (Public)cbxActionsDvdPublics.SelectedItem;
                    Rayon selectedRayon = (Rayon)cbxActionsDvdRayons.SelectedItem;

                    Dvd nvDvd = new Dvd(
                        txbDvdNumero.Text,
                        txbDvdTitre.Text,
                        txbDvdImage.Text,
                        int.Parse(txbDvdDuree.Text),
                        txbDvdRealisateur.Text,
                        txbDvdSynopsis.Text,
                        selectedGenre.Id,
                        selectedGenre.Libelle,
                        selectedPublic.Id,
                        selectedPublic.Libelle,
                        selectedRayon.Id,
                        selectedRayon.Libelle
                        );

                    if (controller.SupprimerDvd(nvDvd))
                    {
                        VisibiliteChampsDvd(true, "Supprimer");

                        MessageBox.Show("Dvd supprimé avec succès.");
                        lesDvd = controller.GetAllDvd();
                        RemplirDvdListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    VisibiliteChampsDvd(true, "Supprimer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        /// <summary>
        /// Change la visibilité des champs pour l'ajout, la modification ou la suppression d'un DVD.
        /// </summary>
        /// <param name="result"></param>
        private void VisibiliteChampsDvd(bool result, string bouton)
        {
            if (bouton == "Ajout")
            {
                VideDvdInfos();
                dgvDvdListe.Visible = result;

                btnDVDActionsModifier.Enabled = result;
                btnDVDActionsSupprimer.Enabled = result;

                txbDvdRealisateur.ReadOnly = result;
            }
            else if (bouton == "Modifier")
            {
                dgvDvdListe.Enabled = result;

                btnDVDActionsAjout.Enabled = result;
                btnDVDActionsSupprimer.Enabled = result;

                txbDvdRealisateur.ReadOnly = result;

            }
            else
            {
                dgvDvdListe.Enabled = result;

                btnDVDActionsModifier.Enabled = result;
                btnDVDActionsAjout.Enabled = result;
            }
            txbDvdSynopsis.ReadOnly = result;
            txbDvdImage.ReadOnly = result;
            txbDvdDuree.ReadOnly = result;
            txbDvdTitre.ReadOnly = result;

            /// genre
            RemplirComboCategorie(controller.GetAllGenres(), bdgActionsGenres, cbxActionsDvdGenres);
            cbxActionsDvdGenres.Visible = !result;
            /// public
            RemplirComboCategorie(controller.GetAllPublics(), bdgActionsPublics, cbxActionsDvdPublics);
            cbxActionsDvdPublics.Visible = !result;
            /// rayon
            RemplirComboCategorie(controller.GetAllRayons(), bdgActionsRayons, cbxActionsDvdRayons);
            cbxActionsDvdRayons.Visible = !result;
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        private void btnRevuesActionsAjout_Click(object sender, EventArgs e)
        {
            if (btnRevuesActionsModifier.Enabled)
            {
                /// Phase 1 : Indique au code que l'on souhaite ajouter un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                VisibiliteChampsRevues(false, "Ajout");
                txbRevuesNumero.Text = "1" + (lesRevues.Count + 1).ToString("D4");

            }
            else
            {
                /// Phase 2 : Ajoute les champs remplis à la liste.
                /// 
                var request = MessageBox.Show("Souhaitez-vous ajouter cette revue au catalogue ?",
                    "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbRevuesNumero.Text == null || txbRevuesTitre.Text == null || txbRevuesImage.Text == null || txbRevuesPeriodicite.Text == null ||
                        txbReceptionRevueDelaiMiseADispo.Text == null || cbxActionsRevuesGenres.SelectedIndex == -1 || cbxActionsRevuesPublics.SelectedIndex == -1 || cbxActionsRevuesRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsRevuesGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsRevuesPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsRevuesRayons.SelectedItem;

                        Revue nvRevue = new Revue(
                            txbRevuesNumero.Text,
                            txbRevuesTitre.Text,
                            txbRevuesImage.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle,
                            txbRevuesPeriodicite.Text,
                            int.Parse(txbRevuesDateMiseADispo.Text)
                            );

                        if (controller.CreerRevue(nvRevue))
                        {
                            VisibiliteChampsRevues(true, "Ajout");

                            MessageBox.Show("Revue ajoutée avec succès.");
                            lesRevues = controller.GetAllRevues();
                            RemplirRevuesListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChampsRevues(true, "Ajout");
                }
            }
        }

        private void btnRevuesActionsModifier_Click(object sender, EventArgs e)
        {
            /// sauvegarde les anciennes valeurs dans des variables. 
            if (btnRevuesActionsAjout.Enabled)
            {

                /// Phase 1 : Indique au code que l'on souhaite modifier un élément.
                /// Prépare le terrain en enablant/disablant tout ce qu'il faut.
                if (dgvRevuesListe.SelectedRows.Count == 1)
                {
                    VisibiliteChampsRevues(false, "Modifier");
                    cbxActionsRevuesGenres.Text = txbRevuesGenre.Text;
                    cbxActionsRevuesPublics.Text = txbRevuesPublic.Text;
                    cbxActionsRevuesRayons.Text = txbRevuesRayon.Text;
                }
                else
                {
                    MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                /// Phase 2 : Modifie le champ.
                /// 

                var request = MessageBox.Show("Souhaitez-vous modifier cette revue ?",
                 "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (request == DialogResult.Yes)
                {
                    /// vérifie si tous les champs sont remplis.
                    if (txbRevuesNumero.Text == null || txbRevuesTitre.Text == null || txbRevuesImage.Text == null || txbRevuesPeriodicite.Text == null ||
                        txbReceptionRevueDelaiMiseADispo.Text == null || cbxActionsRevuesGenres.SelectedIndex == -1 || cbxActionsRevuesPublics.SelectedIndex == -1 || cbxActionsRevuesRayons.SelectedIndex == -1)
                    {
                        MessageBox.Show("Tous les champs ne sont pas remplis. La demande ne peut pas être effectuée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Genre selectedGenre = (Genre)cbxActionsRevuesGenres.SelectedItem;
                        Public selectedPublic = (Public)cbxActionsRevuesPublics.SelectedItem;
                        Rayon selectedRayon = (Rayon)cbxActionsRevuesRayons.SelectedItem;

                        Revue nvRevue = new Revue(
                            txbRevuesNumero.Text,
                            txbRevuesTitre.Text,
                            txbRevuesImage.Text,
                            selectedGenre.Id,
                            selectedGenre.Libelle,
                            selectedPublic.Id,
                            selectedPublic.Libelle,
                            selectedRayon.Id,
                            selectedRayon.Libelle,
                            txbRevuesPeriodicite.Text,
                            int.Parse(txbRevuesDateMiseADispo.Text)
                            );

                        if (controller.ModifierRevue(nvRevue))
                        {
                            VisibiliteChampsRevues(true, "Modifier");

                            MessageBox.Show("Revue modifiée avec succès.");
                            lesRevues = controller.GetAllRevues();
                            RemplirRevuesListeComplete();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la modification.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                if (request == DialogResult.Cancel)
                {
                    VisibiliteChampsRevues(true, "Modifier");
                }
            }
        }

        private void btnRevuesActionsSupprimer_Click(object sender, EventArgs e)
        {
            VisibiliteChampsRevues(false, "Supprimer");
            cbxActionsRevuesGenres.Text = txbRevuesGenre.Text;
            cbxActionsRevuesPublics.Text = txbRevuesPublic.Text;
            cbxActionsRevuesRayons.Text = txbRevuesRayon.Text;

            if (dgvRevuesListe.SelectedRows.Count == 1)
            {
                var request = MessageBox.Show("Souhaitez-vous supprimer la revue suivante : " + txbRevuesTitre.Text + " ?",
                                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (request == DialogResult.Yes)
                {
                    Genre selectedGenre = (Genre)cbxActionsRevuesGenres.SelectedItem;
                    Public selectedPublic = (Public)cbxActionsRevuesPublics.SelectedItem;
                    Rayon selectedRayon = (Rayon)cbxActionsRevuesRayons.SelectedItem;

                    Revue nvRevue = new Revue(
                        txbRevuesNumero.Text,
                        txbRevuesTitre.Text,
                        txbRevuesImage.Text,
                        selectedGenre.Id,
                        selectedGenre.Libelle,
                        selectedPublic.Id,
                        selectedPublic.Libelle,
                        selectedRayon.Id,
                        selectedRayon.Libelle,
                        txbRevuesPeriodicite.Text,
                        int.Parse(txbRevuesDateMiseADispo.Text)
                        );

                    if (controller.SupprimerRevue(nvRevue))
                    {
                        VisibiliteChampsRevues(true, "Supprimer");

                        MessageBox.Show("Revue supprimée avec succès.");
                        lesRevues = controller.GetAllRevues();
                        RemplirRevuesListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    VisibiliteChampsRevues(true, "Supprimer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez ne sélectionner qu'une entrée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        /// <summary>
        /// Change la visibilité des champs pour l'ajout, la modification ou la suppression d'un DVD.
        /// </summary>
        /// <param name="result"></param>
        private void VisibiliteChampsRevues(bool result, string bouton)
        {
            if (bouton == "Ajout")
            {
                VideRevuesInfos();
                dgvRevuesListe.Visible = result;

                btnRevuesActionsModifier.Enabled = result;
                btnRevuesActionsSupprimer.Enabled = result;

            }
            else if (bouton == "Modifier")
            {
                dgvRevuesListe.Enabled = result;

                btnRevuesActionsAjout.Enabled = result;
                btnRevuesActionsSupprimer.Enabled = result;


            }
            else
            {
                dgvRevuesListe.Enabled = result;

                btnRevuesActionsModifier.Enabled = result;
                btnRevuesActionsAjout.Enabled = result;
            }
            txbRevuesPeriodicite.ReadOnly = result;
            txbRevuesImage.ReadOnly = result;
            txbRevuesDateMiseADispo.ReadOnly = result;
            txbRevuesTitre.ReadOnly = result;

            /// genre
            RemplirComboCategorie(controller.GetAllGenres(), bdgActionsGenres, cbxActionsRevuesGenres);
            cbxActionsRevuesGenres.Visible = !result;
            /// public
            RemplirComboCategorie(controller.GetAllPublics(), bdgActionsPublics, cbxActionsRevuesPublics);
            cbxActionsRevuesPublics.Visible = !result;
            /// rayon
            RemplirComboCategorie(controller.GetAllRayons(), bdgActionsRayons, cbxActionsRevuesRayons);
            cbxActionsRevuesRayons.Visible = !result;
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet CommandeLivre
        private readonly BindingSource bdgCommandeListe = new BindingSource();
        private List<Suivi> lesCommandeslivre = new List<Suivi>();
        private bool AjoutCommandelivre = false;
        private bool ModifiCommandelivre = false;
        private void tabCommandeLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxCommandeLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxCommandeLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxCommandeLivresRayons);
            RemplirLivresListeCommandeComplete();
            groupBox1.Enabled = true;
            groupBox2.Visible = true;
            groupBox2.Enabled = true;
            groupBox3.Visible = false;
            groupBox3.Enabled = false;
            ModifiCommandelivre = false;
            AjoutCommandelivre = false;
            cbxEtat.Items.Clear();
            cbxEtat.Items.Add("en cours");
            cbxEtat.Items.Add("livrée");
            cbxEtat.Items.Add("réglée");
            cbxEtat.Items.Add("relancée");
            cbxEtat.SelectedIndex = 0;

        }
        private void RemplirLivresListeCommande(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvCommandeLivresListe.DataSource = bdgLivresListe;
            dgvCommandeLivresListe.Columns["isbn"].Visible = false;
            dgvCommandeLivresListe.Columns["idRayon"].Visible = false;
            dgvCommandeLivresListe.Columns["idGenre"].Visible = false;
            dgvCommandeLivresListe.Columns["idPublic"].Visible = false;
            dgvCommandeLivresListe.Columns["image"].Visible = false;
            dgvCommandeLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCommandeLivresListe.Columns["id"].DisplayIndex = 0;
            dgvCommandeLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        private void RemplirListeCommande(List<Suivi> commandes)
        {
            bdgCommandeListe.DataSource = commandes;
            dgvLivreSuiviCommande.DataSource = bdgCommandeListe;
            dgvLivreSuiviCommande.Columns["idCommandeDocument"].Visible = true;
            dgvLivreSuiviCommande.Columns["idCommande"].Visible = true;
            dgvLivreSuiviCommande.Columns["id"].Visible = true;
            dgvLivreSuiviCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void btnCommandeLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandeLivresNumRecherche.Text.Equals(""))
            {
                txbCommandeLivresTitreRecherche.Text = "";
                cbxCommandeLivresGenres.SelectedIndex = -1;
                cbxCommandeLivresRayons.SelectedIndex = -1;
                cbxCommandeLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbCommandeLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListeCommande(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeCommandeComplete();
                }
            }
            else
            {
                RemplirLivresListeCommandeComplete();
            }
        }
        private void txbCommandeLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbCommandeLivresTitreRecherche.Text.Equals(""))
            {
                cbxCommandeLivresGenres.SelectedIndex = -1;
                cbxCommandeLivresRayons.SelectedIndex = -1;
                cbxCommandeLivresPublics.SelectedIndex = -1;
                txbCommandeLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbCommandeLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListeCommande(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxCommandeLivresGenres.SelectedIndex < 0 && cbxCommandeLivresPublics.SelectedIndex < 0 && cbxCommandeLivresRayons.SelectedIndex < 0
                    && txbCommandeLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeCommandeComplete();
                }
            }
        }
        private void AfficheCommandeLivresInfos(Livre livre)
        {
            txbCommandeLivresAuteur.Text = livre.Auteur;
            txbCommandeLivresCollection.Text = livre.Collection;
            txbCommandeLivresImage.Text = livre.Image;
            txbCommandeLivresIsbn.Text = livre.Isbn;
            txbCommandeLivresNumero.Text = livre.Id;
            txbCommandeLivresGenre.Text = livre.Genre;
            txbCommandeLivresPublic.Text = livre.Public;
            txbCommandeLivresRayon.Text = livre.Rayon;
            txbCommandeLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbCommandeLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeLivresImage.Image = null;
            }
        }
        private void VideLivresCommandeInfos()
        {
            txbCommandeLivresAuteur.Text = "";
            txbCommandeLivresCollection.Text = "";
            txbCommandeLivresImage.Text = "";
            txbCommandeLivresIsbn.Text = "";
            txbCommandeLivresNumero.Text = "";
            txbCommandeLivresGenre.Text = "";
            txbCommandeLivresPublic.Text = "";
            txbCommandeLivresRayon.Text = "";
            txbCommandeLivresTitre.Text = "";
            pcbCommandeLivresImage.Image = null;
        }
        private void RemplirLivresListeCommandeComplete()
        {
            RemplirLivresListeCommande(lesLivres);
            RemplirListeCommande(lesCommandeslivre);
            VideLivresCommandeZones();
        }
        private void cbxCommandeLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeLivresGenres.SelectedIndex >= 0)
            {
                txbCommandeLivresTitreRecherche.Text = "";
                txbCommandeLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxCommandeLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListeCommande(livres);
                cbxCommandeLivresRayons.SelectedIndex = -1;
                cbxCommandeLivresPublics.SelectedIndex = -1;
            }
        }
        private void cbxCommandeLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeLivresPublics.SelectedIndex >= 0)
            {
                txbCommandeLivresTitreRecherche.Text = "";
                txbCommandeLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxCommandeLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListeCommande(livres);
                cbxCommandeLivresRayons.SelectedIndex = -1;
                cbxCommandeLivresGenres.SelectedIndex = -1;
            }
        }
        private void cbxCommandeLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeLivresRayons.SelectedIndex >= 0)
            {
                txbCommandeLivresTitreRecherche.Text = "";
                txbCommandeLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxCommandeLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListeCommande(livres);
                cbxCommandeLivresGenres.SelectedIndex = -1;
                cbxCommandeLivresPublics.SelectedIndex = -1;
            }
        }
        private void dgvCommandeLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandeLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheCommandeLivresInfos(livre);
                    lesCommandeslivre = controller.GetAllSuivis(livre.Id);
                    RemplirListeCommande(lesCommandeslivre);
                    if (lesCommandeslivre.Count < 1)
                    {
                        btnModifCommande.Enabled = false;
                        btnSupprCommande.Enabled = false;
                    }
                    else
                    {
                        btnModifCommande.Enabled = true;
                        btnSupprCommande.Enabled = true;
                    }
                }
                catch
                {
                    VideLivresCommandeZones();
                }
            }
            else
            {
                VideLivresCommandeInfos();
            }
        }
        private void btnCommandeLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeCommandeComplete();
        }
        private void btnCommandeLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeCommandeComplete();
        }
        private void btnCommandeLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeCommandeComplete();
        }
        private void VideLivresCommandeZones()
        {
            cbxCommandeLivresGenres.SelectedIndex = -1;
            cbxCommandeLivresRayons.SelectedIndex = -1;
            cbxCommandeLivresPublics.SelectedIndex = -1;
            txbCommandeLivresNumRecherche.Text = "";
            txbCommandeLivresTitreRecherche.Text = "";
        }
        private void dgvCommandeLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvCommandeLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListeCommande(sortedList);
        }
        private void btnAjoutCommande_Click(object sender, EventArgs e)
        {
            StartAction();
            cbxEtat.Enabled = false;
            AjoutCommandelivre = true;
        }
        private void btnModifCommande_Click(object sender, EventArgs e)
        {
            StartAction();
            cbxEtat.Enabled = true;
            ModifiCommandelivre = true;
            txtNbExemplaire.Text = dgvLivreSuiviCommande.SelectedRows[0].Cells["NbExemplaire"].Value.ToString();
            txtMontant.Text = dgvLivreSuiviCommande.SelectedRows[0].Cells["Montant"].Value.ToString();
            cbxEtat.SelectedItem = dgvLivreSuiviCommande.SelectedRows[0].Cells["Etat"].Value.ToString() ;
        }
        private void btnSupprCommande_Click(object sender, EventArgs e)
        {
            if (dgvLivreSuiviCommande.SelectedRows[0].Cells["Etat"].Value.ToString() == "livrée")
            {
                MessageBox.Show("Une commande livrée ne peut pas être supprimée", "Erreur");
                return;
            }
            if(MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            string idCommande = dgvLivreSuiviCommande.SelectedRows[0].Cells["IdCommande"].Value.ToString();
            if (controller.SupprimerSuivi(idCommande))
            {
                RemplirListeCommande(lesCommandeslivre);
            }
        }
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            EndAction();
        }
        private void btnComfirmer_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            DateTime date = DateTime.Now;
            if (txtNbExemplaire.Text != string.Empty && txtMontant.Text != string.Empty)
            {
                if (AjoutCommandelivre)
                {
                    try
                    {
                        Suivi suivi = new Suivi(date, cbxEtat.SelectedItem.ToString(), livre.Id, Convert.ToInt32(txtNbExemplaire.Text), date, Convert.ToDouble(txtMontant.Text), null, 0);
                        if (controller.CreerSuivi(suivi))
                        {
                            RemplirListeCommande(lesCommandeslivre);
                            EndAction();
                        }
                    } catch 
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
                if (ModifiCommandelivre) 
                {
                    
                    string etatActuel = dgvLivreSuiviCommande.SelectedRows[0].Cells["Etat"].Value.ToString();
                    string etatVoulu = cbxEtat.SelectedItem.ToString();
                    switch (etatActuel)
                    {
                        case "livré" :
                            if (etatVoulu == "en cours" || etatVoulu == "relancée")
                            {
                                MessageBox.Show("Un commande livrée ne peut pas être remise en cours ou relancée", "Erreur");
                                return;
                            }
                            break;
                        case "réglée":
                            MessageBox.Show("Une commande réglée ne peut pas être modifiée", "Erreur");
                            return;
                    }
                    if (etatActuel != "livrée" && etatVoulu == "réglée")
                    {
                        MessageBox.Show("Une commande non livrée ne peut pas être réglée", "Erreur");
                        return;
                    }

                    try
                    {
                        
                        int idSuivi = Convert.ToInt32(dgvLivreSuiviCommande.SelectedRows[0].Cells["Id"].Value.ToString());
                        string idCommande = dgvLivreSuiviCommande.SelectedRows[0].Cells["IdCommande"].Value.ToString();
                        Suivi suivi = new Suivi(date, cbxEtat.SelectedItem.ToString(), livre.Id, Convert.ToInt32(txtNbExemplaire.Text), date, Convert.ToDouble(txtMontant.Text), idCommande, idSuivi);
                        if (controller.ModifiSuivi(suivi))
                        {
                            RemplirListeCommande(lesCommandeslivre);
                            EndAction();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous devez remplir toutes les informations", "Information");
            }
        }
        private void EndAction()
        {
            groupBox1.Enabled = true;
            groupBox2.Visible = true;
            groupBox2.Enabled = true;
            groupBox3.Visible = false;
            groupBox3.Enabled = false;
            cbxEtat.TabIndex = 0;
            txtMontant.Text = string.Empty;
            txtNbExemplaire.Text = string.Empty;
        }
        private void StartAction()
        {
            groupBox1.Enabled = false;
            groupBox2.Visible = false;
            groupBox2.Enabled = false;
            groupBox3.Visible = true;
            groupBox3.Enabled = true;
        }
        #endregion

        #region Onglet CommandeDvd
        private readonly BindingSource bdgCommandeListeDvd = new BindingSource();
        private List<Suivi> lesCommandesdvd = new List<Suivi>();
        private bool AjoutCommandedvd = false;
        private bool ModifiCommandedvd = false;

        private void tabCommandeDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxCommandeDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxCommandeDvdPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxCommandeDvdRayon);
            RemplirDvdListeCommandeComplete();
            groupBox4.Enabled = true;
            groupBox5.Visible = true;
            groupBox5.Enabled = true;
            groupBox6.Visible = false;
            groupBox6.Enabled = false;
            ModifiCommandedvd = false;
            AjoutCommandedvd = false;
            cboEtatDvd.Items.Clear();
            cboEtatDvd.Items.Add("en cours");
            cboEtatDvd.Items.Add("livrée");
            cboEtatDvd.Items.Add("réglée");
            cboEtatDvd.Items.Add("relancée");
            cboEtatDvd.SelectedIndex = 0;

        }

        private void RemplirDvdListeCommande(List<Dvd> dvd)
        {
            bdgCommandeListeDvd.DataSource = dvd;
            dgvCommandeDvdListe.DataSource = bdgCommandeListeDvd;
            dgvCommandeDvdListe.Columns["idRayon"].Visible = false;
            dgvCommandeDvdListe.Columns["idGenre"].Visible = false;
            dgvCommandeDvdListe.Columns["idPublic"].Visible = false;
            dgvCommandeDvdListe.Columns["image"].Visible = false;
            dgvCommandeDvdListe.Columns["synopsis"].Visible = false;
            dgvCommandeDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCommandeDvdListe.Columns["id"].DisplayIndex = 0;
            dgvCommandeDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        private void RemplirListeCommandeDvd(List<Suivi> commandes)
        {
            bdgCommandeListe.DataSource = commandes;
            dgvCommandeDvdSuiviListe.DataSource = bdgCommandeListe;
            dgvCommandeDvdSuiviListe.Columns["idCommandeDocument"].Visible = true;
            dgvCommandeDvdSuiviListe.Columns["idCommande"].Visible = true;
            dgvCommandeDvdSuiviListe.Columns["id"].Visible = true;
            dgvCommandeDvdSuiviListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void AfficheCommandeDvdInfos (Dvd dvd)
        {
            txtCommandeDvdRealisateur.Text = dvd.Realisateur;
            txtCommandeDvdSynopsis.Text = dvd.Synopsis;
            txtCommandeDvdPathImage.Text = dvd.Image;
            txtCommandeDvdDuree.Text = dvd.Duree.ToString();
            txtCommandeDvdNum.Text = dvd.Id;
            txtCommandeDvdGenre.Text = dvd.Genre;
            txtCommandeDvdPublic.Text = dvd.Public;
            txtCommandeDvdRayon.Text = dvd.Rayon;
            txtCommandeDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pictureBoxCommandeDvd.Image = Image.FromFile(image);
            }
            catch
            {
                pictureBoxCommandeDvd.Image = null;
            }
        }

        private void VideDvdCommandeInfos()
        {
            txtCommandeDvdRealisateur.Text = "";
            txtCommandeDvdSynopsis.Text = "";
            txtCommandeDvdPathImage.Text = "";
            txtCommandeDvdDuree.Text = "";
            txtCommandeDvdNum.Text = "";
            txtCommandeDvdGenre.Text = "";
            txtCommandeDvdPublic.Text = "";
            txtCommandeDvdRayon.Text = "";
            txtCommandeDvdTitre.Text = "";
            pictureBoxCommandeDvd.Image = null;
        }

        private void RemplirDvdListeCommandeComplete()
        {
            RemplirDvdListeCommande(lesDvd);
            RemplirListeCommandeDvd(lesCommandesdvd);
            VideDvdCommandeZones();
        }

        private void VideDvdCommandeZones()
        {
            cbxCommandeDvdGenres.SelectedIndex = -1;
            cbxCommandeDvdRayon.SelectedIndex = -1;
            cbxCommandeDvdPublic.SelectedIndex = -1;
            txtCommandeNumDvdRecherche.Text = "";
            txtCommandeDvdRecherche.Text = "";
        }

        private void EndActionDvd()
        {
            groupBox4.Enabled = true;
            groupBox5.Visible = true;
            groupBox5.Enabled = true;
            groupBox6.Visible = false;
            groupBox6.Enabled = false;
            cboEtatDvd.TabIndex = 0;
            txtMontantDvd.Text = string.Empty;
            txtNbExemplaireDvd.Text = string.Empty;
        }

        private void StartActionDvd()
        {
            groupBox4.Enabled = false;
            groupBox5.Visible = false;
            groupBox5.Enabled = false;
            groupBox6.Visible = true;
            groupBox6.Enabled = true;
        }
        private void txtCommandeDvdRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txtCommandeDvdRecherche.Text.Equals(""))
            {
                cbxCommandeDvdGenres.SelectedIndex = -1;
                cbxCommandeDvdRayon.SelectedIndex = -1;
                cbxCommandeDvdPublic.SelectedIndex = -1;
                txtCommandeNumDvdRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txtCommandeDvdRecherche.Text.ToLower()));
                RemplirDvdListeCommande(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxCommandeDvdGenres.SelectedIndex < 0 && cbxCommandeDvdPublic.SelectedIndex < 0 && cbxCommandeDvdRayon.SelectedIndex < 0
                    && txtCommandeNumDvdRecherche.Text.Equals(""))
                {
                    RemplirDvdListeCommandeComplete();
                }
            }
        }

        private void btnCommandeDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txtCommandeNumDvdRecherche.Text.Equals(""))
            {
                txtCommandeDvdRecherche.Text = "";
                cbxCommandeDvdGenres.SelectedIndex = -1;
                cbxCommandeDvdRayon.SelectedIndex = -1;
                cbxCommandeDvdPublic.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtCommandeNumDvdRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> dvds = new List<Dvd>() { dvd };
                    RemplirDvdListeCommande(dvds);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeCommandeComplete();
                }
            }
            else
            {
                RemplirDvdListeCommandeComplete();
            }
        }

        private void cbxCommandeDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeDvdGenres.SelectedIndex >= 0)
            {
                txtCommandeDvdRecherche.Text = "";
                txtCommandeNumDvdRecherche.Text = "";
                Genre genre = (Genre)cbxCommandeDvdGenres.SelectedItem;
                List<Dvd> dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListeCommande(dvd);
                cbxCommandeDvdRayon.SelectedIndex = -1;
                cbxCommandeDvdPublic.SelectedIndex = -1;
            }
        }

        private void cbxCommandeDvdPublic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeDvdPublic.SelectedIndex >= 0)
            {
                txtCommandeDvdTitre.Text = "";
                txtCommandeNumDvdRecherche.Text = "";
                Public lePublic = (Public)cbxCommandeDvdPublic.SelectedItem;
                List<Dvd> dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListeCommande(dvd);
                cbxCommandeDvdRayon.SelectedIndex = -1;
                cbxCommandeDvdGenres.SelectedIndex = -1;
            }
        }

        private void cbxCommandeDvdRayon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCommandeDvdRayon.SelectedIndex >= 0)
            {
                txtCommandeDvdRecherche.Text = "";
                txtCommandeNumDvdRecherche.Text = "";
                Rayon rayon = (Rayon)cbxCommandeDvdRayon.SelectedItem;
                List<Dvd> dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListeCommande(dvd);
                cbxCommandeDvdGenres.SelectedIndex = -1;
                cbxCommandeDvdPublic.SelectedIndex = -1;
            }
        }

        private void btnCommandeDvdAnnulGenre_Click(object sender, EventArgs e)
        {
            RemplirDvdListeCommandeComplete();
        }

        private void btnCommandeDvdAnnulPublic_Click(object sender, EventArgs e)
        {
            RemplirDvdListeCommandeComplete();
        }

        private void btnCommandeDvdAnnulRayon_Click(object sender, EventArgs e)
        {
            RemplirDvdListeCommandeComplete();
        }

        private void btnCommandeDvdAjout_Click(object sender, EventArgs e)
        {
            StartActionDvd();
            cboEtatDvd.Enabled = false;
            AjoutCommandedvd = true;
        }

        private void btnCommandeDvdModifie_Click(object sender, EventArgs e)
        {
            StartActionDvd();
            cboEtatDvd.Enabled = true;
            ModifiCommandedvd = true;
            txtNbExemplaireDvd.Text = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["NbExemplaire"].Value.ToString();
            txtMontantDvd.Text = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["Montant"].Value.ToString();
            cboEtatDvd.SelectedItem = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["Etat"].Value.ToString();
        }

        private void btnCommandeDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["Etat"].Value.ToString() == "livrée")
            {
                MessageBox.Show("Une commande livrée ne peut pas être supprimée", "Erreur");
                return;
            }
            if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            string idCommande = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["IdCommande"].Value.ToString();
            if (controller.SupprimerSuivi(idCommande))
            {
                RemplirListeCommandeDvd(lesCommandesdvd);
            }
        }

        private void dgvCommandeDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandeDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgCommandeListeDvd.List[bdgCommandeListeDvd.Position];
                    AfficheCommandeDvdInfos(dvd);
                    lesCommandesdvd = controller.GetAllSuivis(dvd.Id);
                    RemplirListeCommandeDvd(lesCommandesdvd);
                    if (lesCommandesdvd.Count < 1)
                    {
                        btnCommandeDvdModifie.Enabled = false;
                        btnCommandeDvdSupprimer.Enabled = false;
                    }
                    else
                    {
                        btnCommandeDvdModifie.Enabled = true;
                        btnCommandeDvdSupprimer.Enabled = true;
                    }
                }
                catch
                {
                    VideDvdCommandeZones();
                }
            }
            else
            {
                VideDvdCommandeInfos();
            }
        }

        private void btnComfirmDvd_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgCommandeListeDvd.List[bdgCommandeListeDvd.Position];
            DateTime date = DateTime.Now;
            if (txtNbExemplaireDvd.Text != string.Empty && txtMontantDvd.Text != string.Empty)
            {
                if (AjoutCommandedvd)
                {
                    try
                    {
                        Suivi suivi = new Suivi(date, cboEtatDvd.SelectedItem.ToString(), dvd.Id, Convert.ToInt32(txtNbExemplaireDvd.Text), date, Convert.ToDouble(txtMontantDvd.Text), null, 0);
                        if (controller.CreerSuivi(suivi))
                        {
                            RemplirListeCommandeDvd(lesCommandesdvd);
                            EndActionDvd();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
                if (ModifiCommandedvd)
                {

                    string etatActuel = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["Etat"].Value.ToString();
                    string etatVoulu = cboEtatDvd.SelectedItem.ToString();
                    switch (etatActuel)
                    {
                        case "livré":
                            if (etatVoulu == "en cours" || etatVoulu == "relancée")
                            {
                                MessageBox.Show("Un commande livrée ne peut pas être remise en cours ou relancée", "Erreur");
                                return;
                            }
                            break;
                        case "réglée":
                            MessageBox.Show("Une commande réglée ne peut pas être modifiée", "Erreur");
                            return;
                    }
                    if (etatActuel != "livrée" && etatVoulu == "réglée")
                    {
                        MessageBox.Show("Une commande non livrée ne peut pas être réglée", "Erreur");
                        return;
                    }

                    try
                    {

                        int idSuivi = Convert.ToInt32(dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["Id"].Value.ToString());
                        string idCommande = dgvCommandeDvdSuiviListe.SelectedRows[0].Cells["IdCommande"].Value.ToString();
                        Suivi suivi = new Suivi(date, cboEtatDvd.SelectedItem.ToString(), dvd.Id, Convert.ToInt32(txtNbExemplaireDvd.Text), date, Convert.ToDouble(txtMontantDvd.Text), idCommande, idSuivi);
                        if (controller.ModifiSuivi(suivi))
                        {
                            RemplirListeCommandeDvd(lesCommandesdvd);
                            EndActionDvd();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous devez remplir toutes les informations", "Information");
            }
        }

        private void btnAnnulDvd_Click(object sender, EventArgs e)
        {
            EndActionDvd();
        }
        

        private void dgvCommandeDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdCommandeZones();
            string titreColonne = dgvCommandeDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListeCommande(sortedList);
        }

        #endregion

        #region Onglet CommandeRevue
        private readonly BindingSource bdgCommandeListeRevue = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private bool AjoutCommanderevue = false;
        private bool ModifiCommanderevue = false;
        private void tabCommandeRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cboCommandeRevueGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cboCommandeRevuePublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cboCommandeRevueRayon);
            RemplirCommandeRevuesListeComplete();
            EndActionRevue();
        }
        private void RemplirRevuesListeCommandeTab(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevueListeTabCommande.DataSource = bdgRevuesListe;
            dgvRevueListeTabCommande.Columns["idRayon"].Visible = false;
            dgvRevueListeTabCommande.Columns["idGenre"].Visible = false;
            dgvRevueListeTabCommande.Columns["idPublic"].Visible = false;
            dgvRevueListeTabCommande.Columns["image"].Visible = false;
            dgvRevueListeTabCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevueListeTabCommande.Columns["id"].DisplayIndex = 0;
            dgvRevueListeTabCommande.Columns["titre"].DisplayIndex = 1;
        }
        
        private void RemplirListeCommandeRevue(List<Abonnement> abonnements)            
        {
            bdgCommandeListeRevue.DataSource = abonnements;
            dgvCommandeRevueListe.DataSource = bdgCommandeListeRevue;      
                    
            dgvCommandeRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        
        private void AfficheCommandeRevuesInfos(Revue revue)
        {
            txbCommandeRevuePeriodicite.Text = revue.Periodicite;
            txbCommandeRevuePath.Text = revue.Image;
            txbCommandeRevueDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCommandeRevueNum.Text = revue.Id;
            txbCommandeRevueGenre.Text = revue.Genre;
            txbCommandeRevuePublic.Text = revue.Public;
            txbCommandeRevueRayon.Text = revue.Rayon;
            txbCommandeRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbCommandeRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeRevueImage.Image = null;
            }
        }
        private void VideRevueCommandeInfos()
        {
            txbCommandeRevuePeriodicite.Text = "";
            txbCommandeRevuePath.Text = "";
            txbCommandeRevueDelai.Text = "";
            txbCommandeRevueNum.Text = "";
            txbCommandeRevueGenre.Text = "";
            txbCommandeRevuePublic.Text = "";
            txbCommandeRevueRayon.Text = "";
            txbCommandeRevueTitre.Text = "";
            pcbCommandeRevueImage.Image = null;
        }

        private void RemplirCommandeRevuesListeComplete()
        {
            RemplirRevuesListeCommandeTab(lesRevues);
            RemplirListeCommandeRevue(lesAbonnements);
            VideRevueCommandeZones();
        }

        private void VideRevueCommandeZones()
        {
            cboCommandeRevueGenre.SelectedIndex = -1;
            cboCommandeRevueRayon.SelectedIndex = -1;
            cboCommandeRevuePublic.SelectedIndex = -1;
            txbCommandeRechercheRevueTitre.Text = "";
            txbCommandeRechercheRevueNum.Text = "";
        }
        private void btnCommandeRevueRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandeRechercheRevueNum.Text.Equals(""))
            {
                txtCommandeDvdRecherche.Text = "";
                cbxCommandeDvdGenres.SelectedIndex = -1;
                cbxCommandeDvdRayon.SelectedIndex = -1;
                cbxCommandeDvdPublic.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbCommandeRechercheRevueNum.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListeCommandeTab(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirCommandeRevuesListeComplete();
                }
            }
            else
            {
                RemplirCommandeRevuesListeComplete();
            }
        }

        private void btnCommandeRevueGenreSupp_Click(object sender, EventArgs e)
        {
            RemplirCommandeRevuesListeComplete();
        }

        private void btnCommandeRevuePublicSupp_Click(object sender, EventArgs e)
        {
            RemplirCommandeRevuesListeComplete();
        }

        private void btnCommandeRevueRayonSupp_Click(object sender, EventArgs e)
        {
            RemplirCommandeRevuesListeComplete();
        }

        private void cboCommandeRevueGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbCommandeRechercheRevueTitre.Text = "";
                txbCommandeRechercheRevueNum.Text = "";
                Genre genre = (Genre)cboCommandeRevueGenre.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListeCommandeTab(revues);
                cboCommandeRevueRayon.SelectedIndex = -1;
                cboCommandeRevuePublic.SelectedIndex = -1;
            }
        }

        private void cboCommandeRevuePublic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbCommandeRechercheRevueTitre.Text = "";
                txbCommandeRechercheRevueNum.Text = "";
                Public lePublic = (Public)cboCommandeRevuePublic.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListeCommandeTab(revues);
                cboCommandeRevueRayon.SelectedIndex = -1;
                cboCommandeRevueGenre.SelectedIndex = -1;
            }
        }

        private void cboCommandeRevueRayon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbCommandeRechercheRevueTitre.Text = "";
                txbCommandeRechercheRevueNum.Text = "";
                Rayon rayon = (Rayon)cboCommandeRevueRayon.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListeCommandeTab(revues);
                cboCommandeRevueGenre.SelectedIndex = -1;
                cboCommandeRevuePublic.SelectedIndex = -1;
            }
        }

        private void txbCommandeRechercheRevueTitre_TextChanged(object sender, EventArgs e)
        {
            if (!txbCommandeRechercheRevueTitre.Text.Equals(""))
            {
                cboCommandeRevueGenre.SelectedIndex = -1;
                cboCommandeRevueRayon.SelectedIndex = -1;
                cboCommandeRevuePublic.SelectedIndex = -1;
                txbCommandeRechercheRevueNum.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbCommandeRechercheRevueTitre.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cboCommandeRevueGenre.SelectedIndex < 0 && cboCommandeRevuePublic.SelectedIndex < 0 && cboCommandeRevueRayon.SelectedIndex < 0
                    && txbCommandeRechercheRevueNum.Text.Equals(""))
                {
                    RemplirCommandeRevuesListeComplete();
                }
            }
        }

        private void dgvRevueListeTabCommande_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevueListeTabCommande.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheCommandeRevuesInfos(revue);
                    lesAbonnements = controller.GetAllAbonnements(revue.Id);
                    RemplirListeAbonnements(lesAbonnements);
                    if (lesAbonnements.Count < 1)
                    {
                        btnModifCommandeRevue.Enabled = false;
                        btnSupprCommandeRevue.Enabled = false;
                    }
                    else
                    {
                        btnModifCommandeRevue.Enabled = true;
                        btnSupprCommandeRevue.Enabled = true;
                    }
                }
                catch
                {
                    VideDvdCommandeZones(); // à revoir
                }
            }
            else
            {
                VideRevueCommandeInfos();
            }
        }
        private void RemplirListeAbonnements(List<Abonnement> abonnements)
        {
            bdgCommandeListeRevue.DataSource = abonnements;
            dgvCommandeRevueListe.DataSource = bdgCommandeListeRevue;
            dgvCommandeRevueListe.Columns["idRevue"].Visible = true;
            dgvCommandeRevueListe.Columns["id"].Visible = true;
            dgvCommandeRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnComfirmerCommandeRevue_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            string idAbonnement = dgvCommandeRevueListe.SelectedRows[0].Cells["IdRevue"].Value.ToString();
            DateTime date = DateTime.Now;
            if (textBox1.Text != string.Empty)
            {
                if (AjoutCommanderevue)
                {
                    try
                    {
                        Abonnement abonnement = new Abonnement(date, dateTimePicker1.Value, revue.Id,Convert.ToDouble(textBox1.Text),"-1");
                        if (controller.CreerAbonnement(abonnement))
                        {
                            lesAbonnements = controller.GetAllAbonnements(idAbonnement);
                            RemplirListeCommandeRevue(lesAbonnements);
                            EndActionRevue();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
                if (ModifiCommanderevue)
                {
                    try
                    {

                        string idRevue = dgvCommandeRevueListe.SelectedRows[0].Cells["Id"].Value.ToString();
                        Abonnement abonnement = new Abonnement(date, dateTimePicker1.Value, revue.Id, Convert.ToDouble(textBox1.Text), idRevue);
                        if (controller.ModifiAbonnement(abonnement))
                        {
                            lesAbonnements = controller.GetAllAbonnements(idAbonnement);
                            RemplirListeCommandeRevue(lesAbonnements);
                            EndActionRevue();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Les points ne sont pas autorisé", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous devez remplir toutes les informations", "Information");
            }
        }

        private void btnAnnulerCommandeRevue_Click(object sender, EventArgs e)
        {
            EndActionRevue();
        }
        private void EndActionRevue()
        {
            groupBox7.Enabled = true;
            groupBox8.Visible = true;
            groupBox8.Enabled = true;
            groupBox9.Visible = false;
            groupBox9.Enabled = false;
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Text = string.Empty;
        }

        private void StartActionRevue()
        {
            groupBox7.Enabled = false;
            groupBox8.Visible = false;
            groupBox8.Enabled = false;
            groupBox9.Visible = true;
            groupBox9.Enabled = true;
        }

        private void btnModifCommandeRevue_Click(object sender, EventArgs e)
        {
            StartActionRevue();
            ModifiCommanderevue = true;
            dateTimePicker1.Value = Convert.ToDateTime(dgvCommandeRevueListe.SelectedRows[0].Cells["DateFinAbonnement"].Value);
            textBox1.Text = dgvCommandeRevueListe.SelectedRows[0].Cells["Montant"].Value.ToString();

        }

        private void btnAddCommandeRevue_Click(object sender, EventArgs e)
        {
            StartActionRevue();
            dateTimePicker1.Value = DateTime.Now;
            AjoutCommanderevue = true;
        }

        private void btnSupprCommandeRevue_Click(object sender, EventArgs e)
        {
            string idDocuement = dgvCommandeRevueListe.SelectedRows[0].Cells["IdRevue"].Value.ToString();
            List<Exemplaire> Exemplaires = controller.GetExemplairesRevue(idDocuement);
            DateTime commande = Convert.ToDateTime(dgvCommandeRevueListe.SelectedRows[0].Cells["DateCommande"].Value);
            DateTime fin_abonnement = Convert.ToDateTime(dgvCommandeRevueListe.SelectedRows[0].Cells["DateFinAbonnement"].Value);

            ParutionAbonnement parutionAbonnement = new ParutionAbonnement();

            bool interdit = Exemplaires.Any(ex =>
            parutionAbonnement.ParutionDansAbonnement(commande, fin_abonnement, ex.DateAchat));

            if (interdit)
            {
                MessageBox.Show("Suppression impossible");
                return;
            }
            try
            {
                string idRevue = dgvCommandeRevueListe.SelectedRows[0].Cells["Id"].Value.ToString();
                if (controller.SupprimerAbonnement(idRevue))
                {
                    lesAbonnements = controller.GetAllAbonnements(idDocuement);
                    RemplirListeCommandeRevue(lesAbonnements);
                }
            }
            catch
            {
                MessageBox.Show("Erreur lors de la suppression");
            }
        }

        
    }
    #endregion
}
