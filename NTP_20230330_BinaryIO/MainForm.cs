using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace NTP_20230330_BinaryIO
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        protected static extern void PostQuitMessage(int nExitCode);
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PostQuitMessage(69);
        }

        private void addNewNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumericUpDown nu = new NumericUpDown();
            nu.Size = new Size(40, 10);
            nu.Maximum = decimal.MaxValue;
            nu.Minimum = decimal.MinValue;
            flowLayoutPanel1.Controls.Add(nu);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() is DialogResult.OK)
            {
                SaveToFile(new FileInfo(sfd.FileName));
            }

        }
        /// <summary>
        /// Saves the data to the given file.
        /// </summary>
        /// <param name="file"></param>
        protected void SaveToFile(FileInfo file)
        {
            FileStream fs = file.OpenWrite();
            BinaryWriter bw = new BinaryWriter(fs);
            List<decimal> numbers = (from Control ctl in this.Controls where ctl is NumericUpDown _ select ((NumericUpDown)ctl).Value).ToList();
            foreach (var n in numbers)
            {
                bw.Write(n);
            }
            bw.Flush();
            bw.Close();
            this.Text = $"BinaryIO - {file.FullName}";
        }

        protected void LoadFromFile(FileInfo file)
        {
            FileStream fs = file.OpenRead();
            BinaryReader br = new BinaryReader(fs);
            List<decimal> numbers = new List<decimal>();
            while (fs.Position <= fs.Length)
                numbers.Add(br.ReadDecimal());
            flowLayoutPanel1.Controls.Clear();
            foreach (var n in numbers)
            {
                NumericUpDown nu = new NumericUpDown();
                nu.Size = new Size(40, 10);
                nu.Maximum = decimal.MaxValue;
                nu.Minimum = decimal.MinValue;
                nu.Value = n;
                flowLayoutPanel1.Controls.Add(nu);
            }
            this.Text = $"BinaryIO - {file.FullName}";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() is DialogResult.OK)
            {
                LoadFromFile(new FileInfo(ofd.FileName));
            }

        }
    }
}
