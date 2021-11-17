using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpySnake_in_CSharp
{
    public partial class Form1 : Form
    {
        bool oben, unten, rechts, links;

        int merkeZeile, merkeSpalte, zufallsZeile, zufallsSpalte;

        Panel ApfelPanel = new Panel();

        Random zufall = new Random();

        List<Panel> Schlange = new List<Panel>();

        List<TableLayoutPanelCellPosition> Zellen = new List<TableLayoutPanelCellPosition>();

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (!rechts && Feld.GetRow(KopfPanel) != merkeZeile)
                    {
                        oben = unten = rechts = false;
                        links = true;
                    }
                    break;


                case Keys.Right:
                    if (!links && Feld.GetRow(KopfPanel) != merkeZeile)
                    {
                        links = oben = unten = false;
                        rechts = true;
                    }
                    break;

                case Keys.Up:
                    if (!unten && Feld.GetColumn(KopfPanel) != merkeSpalte)
                    {
                        unten = rechts = links = false;  
                        oben = true;
                    }
                    break;


                case Keys.Down:
                    if (!oben && Feld.GetColumn(KopfPanel) != merkeSpalte)
                    {
                        oben = rechts = links = false;
                        unten = true;
                    }
                    break;

                case Keys.Space:
                    if (!button1.Enabled)
                    {
                        timer1.Enabled = !timer1.Enabled;
                    }
                    break;

                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApfelPanel.BackColor = Color.Red;
            ApfelPanel.Size = new Size(13, 13);
            Feld.Controls.Add(ApfelPanel, zufall.Next(0, Feld.ColumnCount), zufall.Next(0, Feld.RowCount));

            if (radioButton1.Checked == true)
                timer1.Interval = 700;

            else if (radioButton2.Checked == true)
                timer1.Interval = 350;

            else if (radioButton2.Checked == true)
                timer1.Interval = 100;

            groupBox1.Enabled = false;
            button1.Enabled = false;

            timer1.Enabled = true;

            Schlange.Add(KopfPanel);
            Zellen.Add(Feld.GetCellPosition(KopfPanel));

            rechts = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Zellen[0] = Feld.GetCellPosition(KopfPanel);

            merkeZeile = Feld.GetRow(KopfPanel);
            merkeSpalte = Feld.GetColumn(KopfPanel);

            if (unten)
                Feld.SetCellPosition(KopfPanel, new TableLayoutPanelCellPosition(Feld.GetColumn(KopfPanel), Feld.GetRow(KopfPanel) + 1));

            if (oben)
                Feld.SetCellPosition(KopfPanel, new TableLayoutPanelCellPosition(Feld.GetColumn(KopfPanel), Feld.GetRow(KopfPanel) - 1));

            if (rechts)
                Feld.SetCellPosition(KopfPanel, new TableLayoutPanelCellPosition(Feld.GetColumn(KopfPanel) + 1, Feld.GetRow(KopfPanel)));

            if (links)
                Feld.SetCellPosition(KopfPanel, new TableLayoutPanelCellPosition(Feld.GetColumn(KopfPanel) - 1, Feld.GetRow(KopfPanel)));

            for (int i = 1; i < Schlange.Count; i++)
            {
                Zellen[i] = Feld.GetCellPosition(Schlange[i]);
                Feld.SetCellPosition(Schlange[i], Zellen[i - 1]);
            }

            if (Feld.GetCellPosition(KopfPanel).Equals(Feld.GetCellPosition(ApfelPanel)))
            {
                AktuellZahl.Text = Schlange.Count + "";
                Panel PlusPanel = new Panel();
                PlusPanel.BackColor = Color.Purple;
                PlusPanel.Size = new Size(13, 13);

                Schlange.Add(PlusPanel);
                Zellen.Add(new TableLayoutPanelCellPosition(Zellen[Zellen.Count - 1].Column, Zellen[Zellen.Count - 1].Row));
                Feld.Controls.Add(PlusPanel, Zellen[Zellen.Count - 1].Column, Zellen[Zellen.Count - 1].Row);

                do
                {
                    zufallsSpalte = zufall.Next(0, Feld.ColumnCount);
                    zufallsZeile = zufall.Next(0, Feld.RowCount);
                }
                while (Zellen.Exists(x => x.Column == zufallsSpalte && x.Row == zufallsZeile));

                Feld.SetCellPosition(ApfelPanel, new TableLayoutPanelCellPosition(zufallsSpalte, zufallsZeile));

            }

            if (Feld.GetColumn(KopfPanel) > Feld.ColumnCount - 1 || Feld.GetColumn(KopfPanel) < 0 ||
                Feld.GetRow(KopfPanel) > Feld.RowCount - 1 || Feld.GetRow(KopfPanel) < 0 ||
                Zellen.Contains(Feld.GetCellPosition(KopfPanel)))

            {
                RekordZahl.Text = "" + Math.Max(Convert.ToInt32(AktuellZahl.Text), Convert.ToInt32(RekordZahl.Text));
                timer1.Enabled = false;
                KopfPanel.Visible = false;

                oben = unten = rechts = links = false;

                DialogResult dr = MessageBox.Show("Game Over ! \nTry Again?", "Info", MessageBoxButtons.YesNo);

                if (dr == DialogResult.No)
                {
                    Application.Exit();
                }

                if (dr == DialogResult.Yes)
                {
                    Feld.Controls.Remove(ApfelPanel);

                    foreach (Panel p in Schlange)
                        Feld.Controls.Remove(p);

                    Schlange.Clear();
                    Zellen.Clear();

                    Feld.Controls.Add(KopfPanel);

                    Feld.SetCellPosition(KopfPanel, new TableLayoutPanelCellPosition(5, 5));

                    KopfPanel.Visible = true;

                    AktuellZahl.Text = 0 + "";
                    groupBox1.Enabled = true;
                    button1.Enabled = true;

                }
            }
        }
    }
}

