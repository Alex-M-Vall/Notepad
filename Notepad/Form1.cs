using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Reflection.Metadata;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace Notepad
{
    public partial class Form1 : Form
    {
        private string fileName = string.Empty;
        private bool unsavedMaterial = false;
        private bool ignoreListenCng = false;  
        public Form1()
        {
            InitializeComponent();
            UpdateTitle();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void UpdateTitle()
        {
            string file;
           
            if(string.IsNullOrEmpty(fileName))
                file = "Unnamed";

            else
                file = Path.GetFileName(fileName);


            if (unsavedMaterial)
            Text = file + "* -  Notepad";
            else
                Text = file + " -  Notepad";
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)

                    fileName = saveFileDialog.FileName;

                else
                    return;

            }
            File.WriteAllText(fileName, textBox1.Text);
            unsavedMaterial = false;
            UpdateTitle();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(unsavedMaterial)
            {
                var usrInput = MessageBox.Show(this, "Would you like to save changes made to " + Path.GetFileName(fileName) + "?", "Notepad", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (usrInput == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFile();
                    textBox1.Text = String.Empty;
                    fileName = "";
                    unsavedMaterial = false;
                    UpdateTitle();
                }
               
            }
            textBox1.Text = String.Empty;
            fileName = "";
            unsavedMaterial = false;
            UpdateTitle();



        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (unsavedMaterial)
            {
                var usrInput = MessageBox.Show(this, "Would you like to save changes made to " + Path.GetFileName(fileName) + "?", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (usrInput == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFile();

                }
            }
           

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(openFileDialog.FileName);
                fileName = openFileDialog.FileName;
                unsavedMaterial = false;
                UpdateTitle();
                ignoreListenCng = true;
            }


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
      

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           if(ignoreListenCng)
            {
                ignoreListenCng = false;
                return;
            }
            unsavedMaterial = true;
            UpdateTitle();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsavedMaterial)
            {
               var usrInput =  MessageBox.Show(this, "Would you like to save changes made to " + Path.GetFileName(fileName) + "?", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (usrInput == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFile();
                } else if (usrInput == System.Windows.Forms.DialogResult.No)
                {
                   
                } else
                {
                    e.Cancel = true;
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (e.Graphics != null)
            {
                e.Graphics.DrawString(textBox1.Text, textBox1.Font, Brushes.Black, 20, 20);
            }
           
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

   
    }
}