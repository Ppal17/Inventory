using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT__Inventory
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Writeoff f4 = new Writeoff();

            // Show Scanin as a dialog
            f4.ShowDialog();

            // Dispose the Scanin instance
            f4.Dispose();

            // Show Main again
            this.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Stock_In f6 = new Stock_In();

            // Show Scanin as a dialog
            f6.ShowDialog();

            // Dispose the Scanin instance
            f6.Dispose();

            // Show Main again
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Stock_All f5 = new Stock_All();

            // Show Scanin as a dialog
            f5.ShowDialog();

            // Dispose the Scanin instance
            f5.Dispose();

            // Show Main again
            this.Show();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Scanin f2 = new Scanin();

            // Show Scanin as a dialog
             f2.ShowDialog();

            // Dispose the Scanin instance
            f2.Dispose();

            // Show Main again
            this.Show();

        }


        private void button2_Click(object sender, EventArgs e)
        { // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Scanout f3 = new Scanout();

            // Show Scanin as a dialog
            f3.ShowDialog();

            // Dispose the Scanin instance
            f3.Dispose();

            // Show Main again
            this.Show();


        }

        private void button10_Click(object sender, EventArgs e)
        {
            {
                // Hide Main
                this.Hide();

                // Create an instance of Scanin
                Stock_Out f7 = new Stock_Out();

                // Show Scanin as a dialog
                f7.ShowDialog();

                // Dispose the Scanin instance
                f7.Dispose();

                // Show Main again
                this.Show();

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            {
                // Hide Main
                this.Hide();

                // Create an instance of Scanin
                Scanin_report f8 = new Scanin_report();

                // Show Scanin as a dialog
                f8.ShowDialog();

                // Dispose the Scanin instance
                f8.Dispose();

                // Show Main again
                this.Show();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Hide Main
            this.Hide();

            // Create an instance of Scanin
            Scanout_Report f9 = new Scanout_Report();

            // Show Scanin as a dialog
            f9.ShowDialog();

            // Dispose the Scanin instance
            f9.Dispose();

            // Show Main again
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            {
                // Hide Main
                this.Hide();

                // Create an instance of Scanin
                Writeoff_report f10 = new Writeoff_report();

                // Show Scanin as a dialog
                f10.ShowDialog();

                // Dispose the Scanin instance
                f10.Dispose();

                // Show Main again
                this.Show();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            {
                // Hide Main
                this.Hide();

                // Create an instance of Scanin
                Form1 f11 = new Form1();

                // Show Scanin as a dialog
                f11.ShowDialog();

                // Dispose the Scanin instance
                f11.Dispose();

                // Show Main again
                this.Show();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
                // Hide Main
                this.Hide();

                // Create an instance of Scanin
                Form2 f12 = new Form2();

                // Show Scanin as a dialog
                f12.ShowDialog();

                // Dispose the Scanin instance
                f12.Dispose();

                // Show Main again
                this.Show();
        }
    }
}
    
