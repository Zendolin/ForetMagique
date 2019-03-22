namespace ForetMagique
{
    partial class IGForet
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
            this.btnDemarrer = new System.Windows.Forms.Button();
            this.tlpForest = new System.Windows.Forms.TableLayoutPanel();
            this.btnBouger = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDemarrer
            // 
            this.btnDemarrer.Location = new System.Drawing.Point(12, 459);
            this.btnDemarrer.Name = "btnDemarrer";
            this.btnDemarrer.Size = new System.Drawing.Size(98, 23);
            this.btnDemarrer.TabIndex = 0;
            this.btnDemarrer.Text = "Démarrer";
            this.btnDemarrer.UseVisualStyleBackColor = true;
            // 
            // tlpForest
            // 
            this.tlpForest.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpForest.ColumnCount = 3;
            this.tlpForest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpForest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpForest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpForest.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpForest.Location = new System.Drawing.Point(12, 12);
            this.tlpForest.Margin = new System.Windows.Forms.Padding(0);
            this.tlpForest.Name = "tlpForest";
            this.tlpForest.RowCount = 3;
            this.tlpForest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpForest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpForest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpForest.Size = new System.Drawing.Size(507, 444);
            this.tlpForest.TabIndex = 1;
            // 
            // btnBouger
            // 
            this.btnBouger.Location = new System.Drawing.Point(444, 462);
            this.btnBouger.Name = "btnBouger";
            this.btnBouger.Size = new System.Drawing.Size(75, 23);
            this.btnBouger.TabIndex = 2;
            this.btnBouger.Text = "Bouger";
            this.btnBouger.UseVisualStyleBackColor = true;
            // 
            // IGForet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 497);
            this.Controls.Add(this.btnBouger);
            this.Controls.Add(this.tlpForest);
            this.Controls.Add(this.btnDemarrer);
            this.Name = "IGForet";
            this.Text = "Forêt Magique";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDemarrer;
        private System.Windows.Forms.TableLayoutPanel tlpForest;
        private System.Windows.Forms.Button btnBouger;
    }
}

