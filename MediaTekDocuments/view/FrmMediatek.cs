﻿using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

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

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Restreins l'accès aux onglets en fonction de l'utilisateur
        /// </summary>
        public void RestreindreAcces()
        {
            tabOngletsApplication.TabPages.Remove(tabCommandesLivres);
            tabOngletsApplication.TabPages.Remove(tabCommandesDVD);
            tabOngletsApplication.TabPages.Remove(tabCommandesRevue);
            tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            tabOngletsApplication.Enter -= tabOngletsApplication_Enter;
        }


        /// <summary>
        /// Ouvre la fenêtre qui affiche la liste des abonnemnts qui expirent dans moins de 30 jours
        /// </summary>
        private void tabOngletsApplication_Enter(object sender, EventArgs e)
        {
            FrmAlerteFinAbonnement frmAlerte = new FrmAlerteFinAbonnement();
            frmAlerte.ShowDialog();
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
        #endregion

        #region Onglet Parutions
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

        #region Commandes de livres

        private List<CommandeDocument> lesCommandesLivre = new List<CommandeDocument>();

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCommandes_Click(Object sender, EventArgs e)
        {
            if (!txbRechercheDoc.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbRechercheDoc.Text));
                lesCommandesLivre = controller.GetAllCommandes(txbRechercheDoc.Text);
                if (livre != null)
                {
                    AfficheLivresCommandeInfos(livre);
                    RemplirCommandesLivresListe(lesCommandesLivre);
                    gbInfosCommande.Enabled = true;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresCommandeInfos(Livre livre)
        {
            txbCoLivresAuteur.Text = livre.Auteur;
            txbCoLivresCollection.Text = livre.Collection;
            txbCoLivresImage.Text = livre.Image;
            txbCoLivresIsbn.Text = livre.Isbn;
            txbCoLivresGenre.Text = livre.Genre;
            txbCoLivresPublic.Text = livre.Public;
            txbCoLivresRayon.Text = livre.Rayon;
            txbCoLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pictureBox1.Image = Image.FromFile(image);
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }

        private readonly BindingSource bdgCommandesLivre = new BindingSource();

        private void RemplirCommandesLivresListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bdgCommandesLivre.DataSource = lesCommandesDocument;
                dgvCommandesLivre.DataSource = bdgCommandesLivre;
                dgvCommandesLivre.Columns["id"].Visible = false;
                dgvCommandesLivre.Columns["idLivreDvd"].Visible = false;
                dgvCommandesLivre.Columns["idSuivi"].Visible = false;
                dgvCommandesLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                bdgCommandesLivre.DataSource = null;
            }
        }

        private void dgvCommandesLivre_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandesLivre.CurrentCell != null)
            {
                CommandeDocument commandeDoc = (CommandeDocument)bdgCommandesLivre.List[bdgCommandesLivre.Position];
                AfficheCommandeInfos(commandeDoc);
                btnEnregistrerCommande.Visible = false;
                videInfosSuivi();
            }
        }

        private void AfficheCommandeInfos(CommandeDocument commande)
        {
            txbNumeroCo.Text = commande.id;
            txbNbExemplaire.Text = commande.nbExemplaire.ToString();
            txbMontant.Text = commande.montant.ToString();
            dtpCommande.Value = commande.dateCommande;
        }

        private void videInfosLivres()
        {
            txbCoLivresAuteur.Text = "";
            txbCoLivresCollection.Text = "";
            txbCoLivresImage.Text = "";
            txbCoLivresIsbn.Text = "";
            txbCoLivresGenre.Text = "";
            txbCoLivresPublic.Text = "";
            txbCoLivresRayon.Text = "";
            txbCoLivresTitre.Text = "";
            pictureBox1.Image = null;
        }

        private void txbRechercheDoc_TextChanged(object sender, EventArgs e)
        {
            EffaceToutInfos();
        }

        private void EffaceToutInfos()
        {
            videInfosLivres();
            videInfosCommandes();
            gbInfosCommande.Enabled = false;
            btnEnregistrerCommande.Visible = false;
            videInfosSuivi();
            RemplirCommandesLivresListe(null);
        }

        private void videInfosCommandes()
        {
            txbNumeroCo.Text = "";
            txbNbExemplaire.Text = "";
            txbMontant.Text = "";
            dtpCommande.Value = DateTime.Now;
        }

        private void btnAjoutCommande_Click(object sender, EventArgs e)
        {
            videInfosCommandes();
            videInfosSuivi();
            btnEnregistrerCommande.Visible = true;
        }

        private void btnEnregistrerCommande_Click(object sender, EventArgs e)
        {
            if (!txbNumeroCo.Text.Equals("") && !txbNbExemplaire.Text.Equals("") && !txbMontant.Text.Equals(""))
            {
                try
                {
                    string id = txbNumeroCo.Text;
                    int nbExemplaire = int.Parse(txbNbExemplaire.Text);
                    double montant = double.Parse(txbMontant.Text);
                    DateTime dateCommande = dtpCommande.Value;
                    string idLivreDvd = txbRechercheDoc.Text;
                    string idSuivi = "01";
                    Commande commande = new Commande(id, dateCommande, montant);

                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                        lesCommandesLivre = controller.GetAllCommandes(txbRechercheDoc.Text);
                        RemplirCommandesLivresListe(lesCommandesLivre);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                    else
                    {
                        MessageBox.Show("le numéro de commande existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("les informations saisies ne sont pas corrects", "Information");
                    videInfosCommandes();
                    txbNumeroCo.Focus();
                }
            }
            else
            {
                MessageBox.Show("tous les champs sont obligatoires", "Information");
            }
        }

        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            txbRechercheDoc.Text = "";
            EffaceToutInfos();
        }

        private void dgvCommandesLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesLivre.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesCommandesLivre.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesCommandesLivre.OrderBy(o => o.montant).Reverse().ToList();
                    break;
                case "nbExemplaire":
                    sortedList = lesCommandesLivre.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "etapeSuivi":
                    sortedList = lesCommandesLivre.OrderBy(o => o.etapeSuivi).ToList();
                    break;
            }
            RemplirCommandesLivresListe(sortedList);
        }

        private void videInfosSuivi()
        {
            txbSuivi.Text = "";
            btnEnregistrerSuivi.Visible = false;
            cbxSuivi.Visible = false;
            gbSuivi.Enabled = false;
        }

        private void btnSuivi_Click(object sender, EventArgs e)
        {
            gbSuivi.Enabled = true;
            CommandeDocument commande = (CommandeDocument)bdgCommandesLivre.List[bdgCommandesLivre.Position];
            txbSuivi.Text = commande.etapeSuivi;

            cbxSuivi.Items.Clear();
            switch (commande.etapeSuivi)
            {
                case "en cours":
                    cbxSuivi.Text = "";
                    cbxSuivi.Items.Add("relancée");
                    cbxSuivi.Items.Add("livrée");
                    break;
                case "relancée":
                    cbxSuivi.Text = "";
                    cbxSuivi.Items.Add("en cours");
                    cbxSuivi.Items.Add("livrée");
                    break;
                case "livrée":
                    cbxSuivi.Text = "";
                    cbxSuivi.Items.Add("réglée");
                    break;

            }
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            videInfosSuivi();
        }

        private void btnModifierSuivi_Click(object sender, EventArgs e)
        {
            cbxSuivi.Visible = true;
            btnEnregistrerSuivi.Visible = true;
        }

        private string convertitIdSuivi(string etapeSuivi)
        {
            string idSuivi;
            switch (etapeSuivi)
            {
                case "en cours":
                    idSuivi = "01";
                    break;
                case "relancée":
                    idSuivi = "02";
                    break;
                case "livrée":
                    idSuivi = "03";
                    break;
                case "réglée":
                    idSuivi = "04";
                    break;
                default:
                    idSuivi = "";
                    break;
            }
            return idSuivi;
        }


        private void btnEnregistrerSuivi_Click(object sender, EventArgs e)
        {
            if (!cbxSuivi.Text.Equals(""))
            {
                try
                {
                    string idSuivi = convertitIdSuivi(cbxSuivi.Text);
                    CommandeDocument commande = (CommandeDocument)bdgCommandesLivre.List[bdgCommandesLivre.Position];
                    controller.ModifierSuiviCommandeDocument(commande.id, idSuivi);
                    lesCommandesLivre = controller.GetAllCommandes(txbRechercheDoc.Text);
                    RemplirCommandesLivresListe(lesCommandesLivre);
                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }

            }
            else
            {
                MessageBox.Show("Veuillez saisir une nouvelle étape de suivi", "Erreur");
            }
        }

        private void btnSupprCommande_Click(object sender, EventArgs e)
        {
            CommandeDocument commande = (CommandeDocument)bdgCommandesLivre.List[bdgCommandesLivre.Position];
            if ((commande.etapeSuivi == "en cours" || commande.etapeSuivi == "relancée")
                && MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + commande.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {
                    controller.supprimerCommande(commande.id);
                    lesCommandesLivre = controller.GetAllCommandes(txbRechercheDoc.Text);
                    RemplirCommandesLivresListe(lesCommandesLivre);

                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cette commande ne peut pas être supprimée", "Erreur");
            }

        }
        #endregion

        #region Abonnements de revues

        private List<Abonnement> lesAbonnements = new List<Abonnement>();

        private void btnRechercheCoRevues_Click(object sender, EventArgs e)
        {
            if (!txbRechecheCommandesRevue.Text.Equals(""))
            {
                lesRevues = controller.GetAllRevues();
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRechecheCommandesRevue.Text));
                lesAbonnements = controller.GetAllCommandesRevue(txbRechecheCommandesRevue.Text);
                if (revue != null)
                {
                    AfficheRevueCommandeInfos(revue);
                    RemplirCommandesRevueListe(lesAbonnements);
                    gbInfosCommandeRevue.Enabled = true;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        private void AfficheRevueCommandeInfos(Revue revue)
        {
            txbCoTitreRevue.Text = revue.Titre;
            txbCoPeriodicite.Text = revue.Periodicite;
            txbCoDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCoGenreRevue.Text = revue.Genre;
            txbCoPublicRevue.Text = revue.Public;
            txbCoRayonRevue.Text = revue.Rayon;
            txbCoCheImageRevue.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pbxCoRevue.Image = Image.FromFile(image);
            }
            catch
            {
                pbxCoRevue.Image = null;
            }
        }

        private readonly BindingSource bdgCommandesRevue = new BindingSource();

        private void RemplirCommandesRevueListe(List<Abonnement> lesAbonnements)
        {
            if (lesAbonnements != null)
            {
                bdgCommandesRevue.DataSource = lesAbonnements;
                dgvCommandesRevue.DataSource = bdgCommandesRevue;
                dgvCommandesRevue.Columns["id"].Visible = false;
                dgvCommandesRevue.Columns["idRevue"].Visible = false;
                dgvCommandesRevue.Columns["titre"].Visible = false;
                dgvCommandesRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                bdgCommandesRevue.DataSource = null;
            }
        }

        private void txbRechecheCommandesRevue_TextChanged(object sender, EventArgs e)
        {
            effaceTout();
        }

        private void effaceTout()
        {
            txbCoTitreRevue.Text = "";
            txbCoPeriodicite.Text = "";
            txbCoDelai.Text = "";
            txbCoGenreRevue.Text = "";
            txbCoPublicRevue.Text = "";
            txbCoRayonRevue.Text = "";
            txbCoCheImageRevue.Text = "";
            pbxCoRevue.Image = null;
            RemplirCommandesRevueListe(null);
            gbInfosCommandeRevue.Enabled = false;
            effaceInfosCommandeRevue();
            btnEnregistrerCommandeRevue.Visible = false;

        }

        private void btnAjoutCommandeRevue_Click(object sender, EventArgs e)
        {
            btnEnregistrerCommandeRevue.Visible = true;
            effaceInfosCommandeRevue();
        }

        private void effaceInfosCommandeRevue()
        {
            txbMontantRevue.Text = "";
            txbNumeroCommande.Text = "";
            dtpDateFinCommande.Value = DateTime.Now;
            dtpDateCommande.Value = DateTime.Now;
        }

        private void dgvCommandesRevue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandesRevue.CurrentCell != null)
            {
                Abonnement abonnement = (Abonnement)bdgCommandesRevue.List[bdgCommandesRevue.Position];
                txbNumeroCommande.Text = abonnement.id;
                dtpDateCommande.Value = abonnement.dateCommande;
                dtpDateFinCommande.Value = abonnement.dateFinAbonnement;
                txbMontantRevue.Text = abonnement.montant.ToString();
                btnEnregistrerCommandeRevue.Visible = false;
            }

        }

        private void btnEnregistrerCommandeRevue_Click(object sender, EventArgs e)
        {
            if (!txbNumeroCommande.Text.Equals("") && !txbMontantRevue.Text.Equals(""))
            {
                try
                {
                    string id = txbNumeroCommande.Text;
                    double montant = double.Parse(txbMontantRevue.Text);
                    DateTime dateCommande = dtpDateCommande.Value;
                    Commande commande = new Commande(id, dateCommande, montant);

                    string idRevue = txbRechecheCommandesRevue.Text;
                    DateTime dateFinAbonnement = dtpDateFinCommande.Value;

                    if (controller.CreerCommande(commande) && controller.CreerAbonnement(id, dateFinAbonnement, idRevue))
                    {
                        lesAbonnements = controller.GetAllCommandesRevue(txbRechecheCommandesRevue.Text);
                        RemplirCommandesRevueListe(lesAbonnements);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                    else
                    {
                        MessageBox.Show("le numéro de commande existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("les informations saisies ne sont pas corrects", "Information");
                    effaceInfosCommandeRevue();
                    txbNumeroCommande.Focus();
                }
            }
            else
            {
                MessageBox.Show("tous les champs sont obligatoires", "Information");
            }
        }

        private void btnSupprCoRevue_Click(object sender, EventArgs e)
        {
            Abonnement abonnement = (Abonnement)bdgCommandesRevue.List[bdgCommandesRevue.Position];
            DateTime dateFinAbonnement = abonnement.dateFinAbonnement;
            DateTime dateCommande = abonnement.dateCommande;
            lesExemplaires = controller.GetExemplairesRevue(txbRechecheCommandesRevue.Text);

            if (MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + abonnement.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes
                && Exemplaire(lesExemplaires, dateFinAbonnement, dateCommande))
            {
                try
                {
                    controller.supprimerCommande(abonnement.id);
                    lesAbonnements = controller.GetAllCommandesRevue(txbRechecheCommandesRevue.Text);
                    RemplirCommandesRevueListe(lesAbonnements);

                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cet abonnement ne peut pas être supprimé car il contient des exemplaires", "Erreur");
            }

        }

        private void tabCommandesRevue_Enter(object sender, EventArgs e)
        {
            txbRechecheCommandesRevue.Text = "";
        }

        private bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        private bool Exemplaire(List<Exemplaire> lesExemplaires, DateTime dateFinAbonnement, DateTime dateCommande)
        {
            foreach (var exemplaire in lesExemplaires)
            {
                if (ParutionDansAbonnement(dateCommande, dateFinAbonnement, exemplaire.DateAchat))
                {
                    return false;
                }
            }
            return true;
        }

        private void dgvCommandesRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesRevue.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesAbonnements.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "dateFinAbonnement":
                    sortedList = lesAbonnements.OrderBy(o => o.dateFinAbonnement).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesAbonnements.OrderBy(o => o.montant).ToList();
                    break;
            }
            RemplirCommandesRevueListe(sortedList);
        }

        #endregion

        #region Commandes de Dvd
        private List<CommandeDocument> lesCommandesDVD = new List<CommandeDocument>();

        private void btnRechercheCommandesDVD_Click(object sender, EventArgs e)
        {
            if (!txbRechercheCommandesDVD.Text.Equals(""))
            {
                lesDvd = controller.GetAllDvd();
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbRechercheCommandesDVD.Text));
                lesCommandesDVD = controller.GetAllCommandes(txbRechercheCommandesDVD.Text);
                if (dvd != null)
                {
                    AfficheDvdCommandeInfos(dvd);
                    RemplirCommandesDvdListe(lesCommandesDVD);
                    gbxInfosCommandeDVD.Enabled = true;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        private void AfficheDvdCommandeInfos(Dvd dvd)
        {
            txbTitreDVD.Text = dvd.Titre;
            txbCoSynopsis.Text = dvd.Synopsis;
            txbRealisateur.Text = dvd.Realisateur;
            txbGenreDVD.Text = dvd.Genre;
            txbPublicDVD.Text = dvd.Public;
            txbCoLivresPublic.Text = dvd.Public;
            txbRayonDVD.Text = dvd.Rayon;
            txbDuree.Text = dvd.Duree.ToString();
            string image = dvd.Image;
            try
            {
                pictureBox2.Image = Image.FromFile(image);
            }
            catch
            {
                pictureBox2.Image = null;
            }
        }

        private readonly BindingSource bdgCommandesDVD = new BindingSource();

        private void RemplirCommandesDvdListe(List<CommandeDocument> lesCommandesDVD)
        {
            if (lesCommandesDVD != null)
            {
                bdgCommandesDVD.DataSource = lesCommandesDVD;
                dgvCommandesDVD.DataSource = bdgCommandesDVD;
                dgvCommandesDVD.Columns["id"].Visible = false;
                dgvCommandesDVD.Columns["idLivreDvd"].Visible = false;
                dgvCommandesDVD.Columns["idSuivi"].Visible = false;
                dgvCommandesDVD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                bdgCommandesDVD.DataSource = null;
            }
        }

        private void dgvCommandesDVD_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandesDVD.CurrentCell != null)
            {
                CommandeDocument commandeDoc = (CommandeDocument)bdgCommandesDVD.List[bdgCommandesDVD.Position];
                AfficheCommandeDvdInfos(commandeDoc);
                btnEnregistrerCommandeDVD.Visible = false;
                videInfosDvdSuivi();
            }
        }

        private void AfficheCommandeDvdInfos(CommandeDocument commande)
        {
            txbNumCommandeDVD.Text = commande.id;
            txbNbExemplairesCoDVD.Text = commande.nbExemplaire.ToString();
            txbMontantDVD.Text = commande.montant.ToString();
            dtpCommandeDVD.Value = commande.dateCommande;
        }

        private void videInfosDVD()
        {
            txbTitreDVD.Text = "";
            txbCoSynopsis.Text = "";
            txbRealisateur.Text = "";
            txbGenreDVD.Text = "";
            txbPublicDVD.Text = "";
            txbCoLivresPublic.Text = "";
            txbRayonDVD.Text = "";
            txbDuree.Text = "";
            pictureBox2.Image = null;
        }

        private void videInfosDvdCommande()
        {
            txbNumCommandeDVD.Text = "";
            txbNbExemplairesCoDVD.Text = "";
            txbMontantDVD.Text = "";
            dtpCommandeDVD.Value = DateTime.Now;
        }

        private void btnAjoutCommandeDVD_Click(object sender, EventArgs e)
        {
            videInfosDvdCommande();
            videInfosDvdSuivi();
            btnEnregistrerCommandeDVD.Visible = true;
        }

        private void btnEnregistrerCommandeDVD_Click(object sender, EventArgs e)
        {
            if (!txbNumCommandeDVD.Text.Equals("") && !txbNbExemplairesCoDVD.Text.Equals("") && !txbMontantDVD.Text.Equals(""))
            {
                try
                {
                    string id = txbNumCommandeDVD.Text;
                    int nbExemplaire = int.Parse(txbNbExemplairesCoDVD.Text);
                    double montant = double.Parse(txbMontantDVD.Text);
                    DateTime dateCommande = dtpCommandeDVD.Value;
                    string idLivreDvd = txbRechercheCommandesDVD.Text;
                    string idSuivi = "01";
                    Commande commande = new Commande(id, dateCommande, montant);

                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                        lesCommandesDVD = controller.GetAllCommandes(txbRechercheCommandesDVD.Text);
                        RemplirCommandesDvdListe(lesCommandesDVD);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                    else
                    {
                        MessageBox.Show("le numéro de commande existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("les informations saisies ne sont pas corrects", "Information");
                    videInfosDvdCommande();
                    txbNumCommandeDVD.Focus();
                }
            }
            else
            {
                MessageBox.Show("tous les champs sont obligatoires", "Information");
            }
        }

        private void videInfosDvdSuivi()
        {
            txbSuiviDVD.Text = "";
            cbxSuiviDvd.Visible = false;
            btnEnregistrerModificationSuiviDvd.Visible = false;
            gbInfosSuiviDVD.Enabled = false;
        }

        private void btnSuiviDVD_Click(object sender, EventArgs e)
        {
            gbInfosSuiviDVD.Enabled = true;
            CommandeDocument commande = (CommandeDocument)bdgCommandesDVD.List[bdgCommandesDVD.Position];
            txbSuiviDVD.Text = commande.etapeSuivi;

            cbxSuiviDvd.Items.Clear();
            switch (commande.etapeSuivi)
            {
                case "en cours":
                    cbxSuiviDvd.Text = "";
                    cbxSuiviDvd.Items.Add("relancée");
                    cbxSuiviDvd.Items.Add("livrée");
                    break;
                case "relancée":
                    cbxSuiviDvd.Text = "";
                    cbxSuiviDvd.Items.Add("en cours");
                    cbxSuiviDvd.Items.Add("livrée");
                    break;
                case "livrée":
                    cbxSuiviDvd.Text = "";
                    cbxSuiviDvd.Items.Add("réglée");
                    break;

            }
        }

        private void btnModifierSuiviDvd_Click(object sender, EventArgs e)
        {
            cbxSuiviDvd.Visible = true;
            btnEnregistrerModificationSuiviDvd.Visible = true;
        }

        private void btnEnregistrerModificationSuiviDvd_Click(object sender, EventArgs e)
        {
            if (!cbxSuiviDvd.Text.Equals(""))
            {
                try
                {
                    string idSuivi = convertitIdSuivi(cbxSuiviDvd.Text);
                    CommandeDocument commande = (CommandeDocument)bdgCommandesDVD.List[bdgCommandesDVD.Position];
                    controller.ModifierSuiviCommandeDocument(commande.id, idSuivi);
                    lesCommandesDVD = controller.GetAllCommandes(txbRechercheCommandesDVD.Text);
                    RemplirCommandesDvdListe(lesCommandesDVD);
                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }

            }
            else
            {
                MessageBox.Show("Veuillez saisir une nouvelle étape de suivi", "Erreur");
            }
        }

        private void dgvCommandesDVD_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesDVD.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesCommandesDVD.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesCommandesDVD.OrderBy(o => o.montant).Reverse().ToList();
                    break;
                case "nbExemplaire":
                    sortedList = lesCommandesDVD.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "etapeSuivi":
                    sortedList = lesCommandesDVD.OrderBy(o => o.etapeSuivi).ToList();
                    break;
            }
            RemplirCommandesDvdListe(sortedList);
        }

        private void txbRechercheCommandesDVD_TextChanged(object sender, EventArgs e)
        {
            effaceToutInfosDvd();
        }

        private void effaceToutInfosDvd()
        {
            videInfosDVD();
            videInfosDvdCommande();
            videInfosDvdSuivi();
            gbxInfosCommandeDVD.Enabled = false;
            btnEnregistrerCommandeDVD.Visible = false;
            RemplirCommandesDvdListe(null);
        }

        private void tabCommandesDVD_Enter(object sender, EventArgs e)
        {
            txbRechercheCommandesDVD.Text = "";
            effaceToutInfosDvd();
        }

        private void btnRetourDvd_Click(object sender, EventArgs e)
        {
            videInfosDvdSuivi();
        }

        private void btnSupprimerCommandeDVD_Click(object sender, EventArgs e)
        {
            CommandeDocument commande = (CommandeDocument)bdgCommandesDVD.List[bdgCommandesDVD.Position];
            if ((commande.etapeSuivi == "en cours" || commande.etapeSuivi == "relancée")
                && MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + commande.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {
                    controller.supprimerCommande(commande.id);
                    lesCommandesDVD = controller.GetAllCommandes(txbRechercheCommandesDVD.Text);
                    RemplirCommandesDvdListe(lesCommandesDVD);

                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cette commande ne peut pas être supprimée", "Erreur");
            }
        }
        #endregion
    }
}
