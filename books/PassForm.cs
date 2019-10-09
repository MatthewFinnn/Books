using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace books
{
    public partial class PassForm : Form
    {
        string pass = "pass";
        Timer tm;
        public PassForm()
        {
            InitializeComponent();
            tm = new Timer();
            tm.Interval = 500;
            textBox1.KeyDown += textBox1_KeyDown;
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyData == Keys.Enter)
            {
                if (textBox1.Text == pass)
                {
                    BooksForm.blnPass = true;
                    this.Close();
                }
                else
                {
                    textBox1.Text = "";
                    textBox1.BackColor = Color.Red;
                    tm.Start();
                    tm.Tick += tm_Tick;
                }
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            tm.Stop();
        }

    }
}
