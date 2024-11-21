using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cxapi;
using Microsoft.Win32;

namespace tut_executer
{
    public partial class Form1 : RoundedForm
    {
        Timer time = new Timer();
        public Point mouseLocation;

        public Form1()
        {
            InitializeComponent();
            time.Tick += timertick;
            time.Start();
            Editor.Navigate(new Uri(string.Format("file:///{0}/Monaco/index.html", Directory.GetCurrentDirectory())));
            functions.PopulateListBox(listBox1, "./Scripts", "*.lua");
            functions.PopulateListBox(listBox1, "./Scripts", "*.txt");
            Editor.DocumentCompleted += Editor_DocumentCompleted;
        }

        private void Editor_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // The document is ready, and you can now safely invoke scripts.
        }

        private void timertick(object sender, EventArgs e)
        {
            if (CoreFunctions.IsRobloxOpen())
            {
                robloxopen.Text = "Roblox Open: 👍";
                robloxopen.ForeColor = Color.DarkGreen;  // Change text color to green
            }
            else
            {
                robloxopen.Text = "Roblox Open: ❌";
                robloxopen.ForeColor = Color.DarkRed;  // Change text color to red
            }

            if (CoreFunctions.IsInjected())
            {
                status.Text = "Status: Injected!";
                status.ForeColor = Color.DarkGreen;  // Change text color to green
            }
            else
            {
                status.Text = "Status: Not Injected!";
                status.ForeColor = Color.DarkRed;  // Change text color to red
            }
        }

        private void Inject_Click(object sender, EventArgs e)
        {
            CoreFunctions.Inject();
        }

        private void execute_Click(object sender, EventArgs e)
        {
            string script = Editor.Document.InvokeScript("getValue").ToString();
            CoreFunctions.ExecuteScript(script);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CoreFunctions.SetAutoInject(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CoreFunctions.KillRoblox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Ensure the document is loaded and then invoke JavaScript to clear the editor
            if (Editor.Document != null)
            {
                Editor.Document.InvokeScript("eval", new object[] { "editor.setValue('')" });
            }
            else
            {
                MessageBox.Show("Editor document is not ready yet.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string script = File.ReadAllText("./Scripts/" + listBox1.SelectedItem);
            Editor.Document.InvokeScript("setValue", new object[] { script });
        }

        class functions
        {
            public static void PopulateListBox(System.Windows.Forms.ListBox lsb, string Folder, string FileType)
            {
                DirectoryInfo dinfo = new DirectoryInfo(Folder);
                FileInfo[] Files = dinfo.GetFiles(FileType);
                foreach (FileInfo file in Files)
                {
                    lsb.Items.Add(file.Name);
                }
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void openfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Txt Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string script = File.ReadAllText(dialog.FileName);
                Editor.Document.InvokeScript("setValue", new object[] { script });
            }
        }

        private void savefile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt",
                DefaultExt = "lua",
                Title = "Save Lua or Text File"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Assuming you're retrieving text from your editor through a script method
                string textToSave = (string)Editor.Document.InvokeScript("getValue");

                // Save the file
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(textToSave);
                }
            }
        }


        private int tabCount = 1;

        private void button6_Click_1(object sender, EventArgs e)
        {
            TabPage newTab = new TabPage($"Tab {tabCount + 1}");

            WebBrowser webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill // Fill the tab with the WebBrowser
            };

            webBrowser.Navigate(new Uri(string.Format("file:///{0}/Monaco/index.html", Directory.GetCurrentDirectory())));
            newTab.Controls.Add(webBrowser);
            tabControl1.TabPages.Add(newTab);
            tabCount++;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0 && tabControl1.SelectedTab != null)
            {
                if (tabControl1.TabCount > 1)
                {
                    if (MessageBox.Show("Are you sure you want to close this tab?", "Close Tab", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                        tabCount--;
                    }
                }
                else
                {
                    MessageBox.Show("You cannot close the last tab.", "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void Editor_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}