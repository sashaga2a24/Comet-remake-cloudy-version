using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tut_executer
{
    public partial class Key : RoundedForm
    {

        Timer time = new Timer();
        public Point mouseLocation;
        public Key()
        {
            InitializeComponent();
            this.Show();
        }

        private void submit_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void submit_Click_1(object sender, EventArgs e)
        {
            if (keytext.Text == "madebytacocatt")
            {
                Form1 f1 = new Form1();
                f1.Show(); // Show the main form
                this.Hide(); // Hide the key form after success
            }
            else
            {
                MessageBox.Show("Wrong Key Stupid, Try Again!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                keytext.Clear();
                keytext.Focus();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                // Replace with your Discord invite link
                string LinkvertiseLink = "https://link-hub.net/1256539/tacokeysystemi";

                // Open the Discord invite link in the default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = LinkvertiseLink,
                    UseShellExecute = true
                });
            }
        }
    }

}