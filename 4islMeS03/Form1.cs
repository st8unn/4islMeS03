using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4islMeS03
{
    public partial class Form1 : Form
    {
        Koeffinder syst = new Koeffinder();
        //GradientSolver syst1 = new GradientSolver();
        grswiki syst1 = new grswiki();
        gradWikiCuted2 syst2 = new gradWikiCuted2();
        public Form1()
        {
            InitializeComponent();
            for (int i = 1; i < 11; i++)
            {
                dataGridView1.Columns.Add("w" + i.ToString(), "w" + i.ToString());
            }
            
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vNumX, vNumY, vNumMax;
            double vEpsil;
            try
            {
                Convert.ToInt32(textBox1.Text);
            }
            catch (Exception)
            {

                textBox1.Text = "4";
            }
            vNumX = Math.Max(2, Convert.ToInt32(textBox1.Text));
            textBox1.Text = vNumX.ToString();

            try
            {
                Convert.ToInt32(textBox2.Text);
            }
            catch (Exception)
            {

                textBox2.Text = "4";
            }
            vNumY = Math.Max(2, Convert.ToInt32(textBox2.Text));
            textBox2.Text = vNumY.ToString();

            try
            {
                Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {

                textBox3.Text = "4";
            }
            vNumMax = Math.Max(1, Convert.ToInt32(textBox3.Text));
            textBox3.Text = vNumMax.ToString();

            try
            {
                Convert.ToDouble(textBox4.Text);
            }
            catch (Exception)
            {

                textBox4.Text = "0,001";
            }
            vEpsil = Math.Min(0.1, Convert.ToDouble(textBox4.Text));
            textBox4.Text = vEpsil.ToString();

            syst.Calculation(vNumX,vNumY,vNumMax,vEpsil);
            label7.Text = syst.vDZel.ToString("G3");
            label8.Text = syst.vDSol.ToString("G3");
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Math.Min(40,vNumX-1); i++)
            {
                dataGridView1.Rows.Add(1);
                for (int j = 0; j < Math.Min(10,vNumY-1); j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = syst.massD2d[i,j].ToString("G3");
                }
                
            }
            this.Text = syst.vIteration.ToString();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void changerToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeToolStripMenuItem.Checked = !changeToolStripMenuItem.Checked;
            bool temp = changeToolStripMenuItem.Checked;
            syst.chang(!temp);
            syst1.chang(!temp);
            syst2.chang(!temp);
        }

        private void run2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vNumX, vNumY, vNumMax;
            double vEpsil;
            try
            {
                Convert.ToInt32(textBox1.Text);
            }
            catch (Exception)
            {

                textBox1.Text = "4";
            }
            vNumX = Math.Max(2, Convert.ToInt32(textBox1.Text));
            textBox1.Text = vNumX.ToString();

            try
            {
                Convert.ToInt32(textBox2.Text);
            }
            catch (Exception)
            {

                textBox2.Text = "4";
            }
            vNumY = Math.Max(2, Convert.ToInt32(textBox2.Text));
            textBox2.Text = vNumY.ToString();

            try
            {
                Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {

                textBox3.Text = "4";
            }
            vNumMax = Math.Max(1, Convert.ToInt32(textBox3.Text));
            textBox3.Text = vNumMax.ToString();

            try
            {
                Convert.ToDouble(textBox4.Text);
            }
            catch (Exception)
            {
                textBox4.Text = "0,001";
            }
            vEpsil = Math.Min(0.1, Convert.ToDouble(textBox4.Text));
            textBox4.Text = vEpsil.ToString();

            syst1.GradSolverWiki(vNumX, vNumY, vNumMax, vEpsil);
            label7.Text = syst1.vDMeth.ToString("G3");
            label8.Text = syst1.vDSol.ToString("G3");
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Math.Min(40, vNumX - 1); i++)
            {
                dataGridView1.Rows.Add(1);
                for (int j = 0; j < Math.Min(10, vNumY - 1); j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = syst1.massD2d[i, j].ToString("G3");
                }

            }
            this.Text = syst1.vIteration.ToString();
        }

        private void run3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vNumX, vNumY, vNumMax;
            double vEpsil;
            try
            {
                Convert.ToInt32(textBox1.Text);
            }
            catch (Exception)
            {

                textBox1.Text = "6";
            }
            vNumX = Math.Max(2, Convert.ToInt32(textBox1.Text));
            textBox1.Text = vNumX.ToString();

            try
            {
                Convert.ToInt32(textBox2.Text);
            }
            catch (Exception)
            {

                textBox2.Text = "6";
            }
            vNumY = Math.Max(2, Convert.ToInt32(textBox2.Text));
            textBox2.Text = vNumY.ToString();

            try
            {
                Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {

                textBox3.Text = "40";
            }
            vNumMax = Math.Max(1, Convert.ToInt32(textBox3.Text));
            textBox3.Text = vNumMax.ToString();

            try
            {
                Convert.ToDouble(textBox4.Text);
            }
            catch (Exception)
            {
                textBox4.Text = "0,001";
            }
            vEpsil = Math.Min(0.1, Convert.ToDouble(textBox4.Text));
            textBox4.Text = vEpsil.ToString();

            syst2.GradSolverWiki2(vNumX, vNumY, vNumMax, vEpsil);
            label7.Text = syst2.vDMeth.ToString("G3");
            label8.Text = syst2.vDSol.ToString("G3");
            dataGridView1.Rows.Clear();
            vNumX = syst2.vNumX;
            vNumY = syst2.vNumY;
            for (int i = 0; i < Math.Min(40, vNumX - 1); i++)
            {
                dataGridView1.Rows.Add(1);
                for (int j = 0; j < Math.Min(10, vNumY - 1); j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = syst2.massD2d[i, j].ToString("G3");
                }
            }
            this.Text = syst2.vIteration.ToString();
        }
    }
}
