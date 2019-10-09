using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace books
{
    public partial class MainForm : Form
    {
        string dbFollder;
        int ialbum=0;
        string[] files;
        Timer t;

        public MainForm()
        {
            InitializeComponent();
            dbFollder = FindFolder("Data");
            InitTimer();
            t.Tick += t_Tick;
            label1.MouseEnter += label1_MouseEnter;
            label1.MouseLeave += label1_MouseLeave;
            label3.MouseEnter += label3_MouseEnter;
            label3.MouseLeave += label3_MouseLeave;
            label4.MouseEnter += label4_MouseEnter;
            label4.MouseLeave += label4_MouseLeave;
            label5.MouseEnter += label5_MouseEnter;
            label5.MouseLeave += label5_MouseLeave;
            label5.Click += label5_Click;
            label4.Click += label4_Click;
            label3.Click += label3_Click;
            label1.Click += label1_Click;
        }

        #region Label
        void label1_Click(object sender, EventArgs e)
        {
            BooksForm form = new BooksForm();
            form.Show();
            this.Visible = false;
            form.FormClosed += form_FormClosed;
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;
        }

        void label3_Click(object sender, EventArgs e)
        {
            ReportForm rf = new ReportForm();
            rf.ShowDialog();

        }

        void label4_Click(object sender, EventArgs e)
        {
            Process SysInfo = new Process();
            SysInfo.StartInfo.ErrorDialog = true;
            SysInfo.StartInfo.FileName = FindFolder("Data") + "1.chm";
            SysInfo.Start();
        }

        void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Gray;
        }

        void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.ForeColor = Color.White;
        }

        void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Gray;
        }

        void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.White;
        }

        void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Gray;
        }

        void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
        }

        void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Gray;
        }

        void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.White;
        }
        #endregion

        void t_Tick(object sender, EventArgs e)
        {
            if (ialbum == files.Length) { ialbum = 0; }
            pictureBox1.ImageLocation = files[ialbum];
            ialbum++;
        }

        void InitTimer()
        {
            files = Directory.GetFiles(dbFollder + "album", "*.jpg", SearchOption.TopDirectoryOnly);
            pictureBox1.ImageLocation = files[ialbum];
            ialbum++;
            t = new Timer();
            t.Interval = 5000;
            t.Start();
        }

        public static string FindFolder(string name)
        {
            string dir = Application.StartupPath;
            for (char slash = '\\'; dir != null; dir = Path.GetDirectoryName(dir))
            {
                string res = dir.TrimEnd(slash) + slash + name;
                if (Directory.Exists(res))
                    return res + slash;
            }
            return null;
        }
    }
}
