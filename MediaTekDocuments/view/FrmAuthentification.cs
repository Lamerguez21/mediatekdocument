using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        private List<Utilisateur> leUser = new List<Utilisateur>();
        private readonly FrmMediatekController controller;

        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (!txbLogin.Text.Equals("") && !txbMdp.Text.Equals(""))
            {
                string login = txbLogin.Text;
                string password = txbMdp.Text;
                string idService = controller.GetUtilisateur(login, password);
                if (!idService.Equals(""))
                {
                    switch (idService)
                    {
                        case "3":
                            MessageBox.Show("Vous n'avez pas les droits d'accès à l'application.", "Alerte");
                            Application.Exit();
                            break;
                        case "1":
                        case "0":
                            FrmMediatek frmMediatek = new FrmMediatek();
                            frmMediatek.ShowDialog();
                            this.Close();
                            break;
                        case "2":
                            FrmMediatek FrmMediatek = new FrmMediatek();
                            FrmMediatek.RestreindreAcces();
                            FrmMediatek.ShowDialog();
                            this.Close();
                            break;
                    }
                        
                }
                else
                {
                    MessageBox.Show("Le login ou le mot de passe est incorrect", "Erreur");
                    txbLogin.Text = "";
                    txbMdp.Text = "";
                    txbLogin.Focus();
                }
            }
            else
            {
                MessageBox.Show("Veuillez renseigner un login et un mot de passe", "Erreur");
                txbLogin.Text = "";
                txbMdp.Text = "";
                txbLogin.Focus();
            }
        }



    }
}
