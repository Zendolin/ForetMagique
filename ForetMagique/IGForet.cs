using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForetMagique
{
    public partial class IGForet : Form
    {
        public static readonly int NB_CASE_LIGNE = 3;
        public static readonly int NB_CASE_COLONNE = 3;

        private PictureBox[,] pbs;
        private int performance = 0;

        private int widthPB;
        private int heightPB;
        public ForetEnvironnement foret;

        public IGForet()
        {
            InitializeComponent();

            InitializePanel();
        }

        public void CalculSizePB(int rows, int columns)
        {
            widthPB = tlpForest.Width / columns;
            heightPB = tlpForest.Height / rows;
        }

        public void InitializePanel()
        {
            SetPanel(NB_CASE_LIGNE, NB_CASE_COLONNE);
        }

        public void UpdatePanel(int rows, int columns)
        {
            SetPanel(rows, columns);
        }


        public void SetPanelThreadSafe(int rows, int columns)
        {
            tlpForest.Invoke(new MethodInvoker(delegate
            {
                SetPanel(rows, columns);
            }));
            chckBoxAuto.Invoke(new MethodInvoker(delegate
            {
                chckBoxAuto.Checked = false;
            }));

        }

        public void SetPanel(int rows, int columns)
        {
            tlpForest.RowCount = rows;
            tlpForest.ColumnCount = columns;
            tlpForest.RowStyles.Clear();
            tlpForest.ColumnStyles.Clear();

            for (int i = 0; i < rows; i++)
            {
                tlpForest.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            }
            for (int i = 0; i < columns; i++)
            {
                tlpForest.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            CalculSizePB(rows, columns);
            pbs = new PictureBox[rows, columns];
            tlpForest.Controls.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    PictureBox pb = new PictureBox();

                    pb.Width = widthPB;
                    pb.Height = heightPB;

                    pb.SizeMode = PictureBoxSizeMode.StretchImage;

                    pbs[i, j] = pb;
                    tlpForest.Controls.Add(pb);
                }
            }
        }

        public void SetZone(Zone zone)
        {
            try
            {


                int x = zone.coordX;
                int y = zone.coordY;

                Image img = null;
                if (zone.estFrontiere) pbs[y, x].BackColor = Color.Red;
                else if (zone.visité) pbs[y, x].BackColor = Color.White;
                else if (checkBox1.Checked) pbs[y, x].BackColor = Color.DarkGray;
                else pbs[y, x].BackColor = Color.Black;

                if (zone.contenu.Count == 0)
                {
                    pbs[y, x].Image = null;
                    return;
                }

                if (checkBox1.Checked)
                {
                    switch (zone.contenu[0])
                    {
                        case "monstre_mort":
                            img = Properties.Resources.monstre_mort;
                            break;
                        case "monstre":
                            img = Properties.Resources.monstre;
                            break;
                        case "odeur":
                            img = Properties.Resources.odeur;
                            break;
                        case "crevasse":
                            img = Properties.Resources.crevasse;
                            break;
                        case "vent":
                            img = Properties.Resources.vent;
                            break;
                        case "portail":
                            img = Properties.Resources.portail;
                            break;
                        default:
                            img = null;
                            break;
                    }
                }
                else if (zone.visité)
                {
                    switch (zone.contenu[0])
                    {
                        case "monstre_mort":
                            img = Properties.Resources.monstre_mort;
                            break;
                        case "monstre":
                            img = Properties.Resources.monstre;
                            break;
                        case "odeur":
                            img = Properties.Resources.odeur;
                            break;
                        case "crevasse":
                            img = Properties.Resources.crevasse;
                            break;
                        case "vent":
                            img = Properties.Resources.vent;
                            break;
                        case "portail":
                            img = Properties.Resources.portail;
                            break;
                        default:
                            img = null;
                            break;
                    }
                }
                if (zone.contenu[0] == "agent") img = Properties.Resources.agent;
                pbs[y, x].Image = img;
            }
            catch(Exception e) { }
        }

        public void UpdatePerf(int perf)
        {
            performance = perf;
            try
            {
                label1.Invoke(new MethodInvoker(delegate
                {
                    label1.Text = "performance : " + performance;
                }));
            }
            catch (Exception e) { }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void btnDemarrer_Click(object sender, EventArgs e)
        {
            foret.agent.peutAvancer = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foret.DessinerForet();
        }

        private void chckBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            foret.agent.modeAuto = chckBoxAuto.Checked;
        }
    }
}

