using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view
{
    public partial class FrmAlerteFinAbonnement : Form
    {
        private readonly FrmMediatekController controller;
        public FrmAlerteFinAbonnement()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        private readonly BindingSource bdgEcheanceRevue = new BindingSource();
        private List<Abonnement> lesEcheances = new List<Abonnement>();

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void FrmAlerteFinAbonnement_Load(object sender, EventArgs e)
        {
            lesEcheances = controller.GetAllEcheanceAbonnement();
            RemplirEcheanceRevueListe(lesEcheances);
        }
        private void RemplirEcheanceRevueListe(List<Abonnement> lesEcheances)
        {
            if (lesEcheances != null)
            {
                bdgEcheanceRevue.DataSource = lesEcheances;
                dgvFinAbonnement.DataSource = bdgEcheanceRevue;
                dgvFinAbonnement.Columns["id"].Visible = false;
                dgvFinAbonnement.Columns["dateCommande"].Visible = false;
                dgvFinAbonnement.Columns["montant"].Visible = false;
                dgvFinAbonnement.Columns["idRevue"].Visible = false;
                dgvFinAbonnement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                bdgEcheanceRevue.DataSource = null;
            }
        }
    }
}
