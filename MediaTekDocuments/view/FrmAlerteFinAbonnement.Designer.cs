
namespace MediaTekDocuments.view
{
    partial class FrmAlerteFinAbonnement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvFinAbonnement = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnement)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFinAbonnement
            // 
            this.dgvFinAbonnement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinAbonnement.Location = new System.Drawing.Point(107, 74);
            this.dgvFinAbonnement.Name = "dgvFinAbonnement";
            this.dgvFinAbonnement.ReadOnly = true;
            this.dgvFinAbonnement.RowHeadersVisible = false;
            this.dgvFinAbonnement.RowHeadersWidth = 62;
            this.dgvFinAbonnement.RowTemplate.Height = 28;
            this.dgvFinAbonnement.Size = new System.Drawing.Size(519, 226);
            this.dgvFinAbonnement.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(484, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Liste des abonnements qui expirent dans moins de 30 jours";
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(626, 336);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(121, 41);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmAlerteFinAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 410);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvFinAbonnement);
            this.Name = "FrmAlerteFinAbonnement";
            this.Text = "Abonnements qui arrivent à échéance";
            this.Load += new System.EventHandler(this.FrmAlerteFinAbonnement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFinAbonnement;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
    }
}