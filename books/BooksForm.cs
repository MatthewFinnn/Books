using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace books
{
    public partial class BooksForm : Form
    {
        public static bool blnPass = false;
        public static int curentBook;
        int ind = 0;
        string dbFollder, ConnectionString;
        OleDbConnection cn;
        DataSet ds;
        BindingSource bs;
        OleDbDataAdapter daBook;
        bool blnSortAscCategory = false;

        public BooksForm()
        {
            InitializeComponent();
            dbFollder = FindFolder("Data");
            ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + dbFollder + "book.mdb";
            Load += BooksForm_Load;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            label1.Click += label1_Click;
            textBox2.TextChanged += textBox2_TextChanged;
            listBox2.DoubleClick += listBox2_DoubleClick;
            label10.MouseEnter += label10_MouseEnter;
            label10.MouseLeave += label10_MouseLeave;
            label10.Click += label10_Click;
            label11.MouseEnter += label11_MouseEnter;
            label11.MouseLeave += label11_MouseLeave;
            label11.Click += label11_Click;
            label12.MouseEnter += label12_MouseEnter;
            label12.MouseLeave += label12_MouseLeave;
            label12.Click += label12_Click;
        }

        void label12_Click(object sender, EventArgs e)
        {
            if (!blnPass)
            {
                PassForm pf = new PassForm();
                pf.ShowDialog();
            }
            if (blnPass)
            {
                var msg = MessageBox.Show("Для удаления записи нажмите кнопку \"Да\". Отменение удаления будет невозможно. \nУдалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (msg == DialogResult.Yes)
                {
                    cn.Open();
                    daBook.DeleteCommand = cn.CreateCommand();
                    daBook.DeleteCommand.CommandText = "delete from bookGenre where id_book=" + curentBook;
                    daBook.DeleteCommand.ExecuteNonQuery();
                    daBook.DeleteCommand.CommandText = "delete from bookCategory where id_book=" + curentBook;
                    daBook.DeleteCommand.ExecuteNonQuery();
                    daBook.DeleteCommand.CommandText = "DELETE FROM book WHERE id_book=" + curentBook;
                    daBook.DeleteCommand.ExecuteNonQuery();
                    ConnectAndRead();
                    cn.Close();
                }
            }
        }

        void label12_MouseLeave(object sender, EventArgs e)
        {
            label12.ForeColor = Color.Gray;
        }

        void label12_MouseEnter(object sender, EventArgs e)
        {
            label12.ForeColor = Color.White;
        }

        void label11_Click(object sender, EventArgs e)
        {
            if (!blnPass)
            {
                PassForm pf = new PassForm();
                pf.ShowDialog();
            }
            if (blnPass)
            {
                CreateForm cf = new CreateForm();
                cf.Show();
                this.Visible = false;
                cf.FormClosed += rf_FormClosed;
            }
        }

        void label11_MouseLeave(object sender, EventArgs e)
        {
            label11.ForeColor = Color.Gray;
        }

        void label11_MouseEnter(object sender, EventArgs e)
        {
            label11.ForeColor = Color.White;
        }

        void label10_Click(object sender, EventArgs e)
        {
            blnPass = false;
            this.Close();
        }

        void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Gray;
        }

        void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.ForeColor = Color.White;
        }

        void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (!blnPass)
            {
                PassForm pf = new PassForm();
                pf.ShowDialog();
            }
            if (listBox2.Items.Count != 0 && blnPass)
            {
                ind = listBox2.SelectedIndex;
                RedaktForm rf = new RedaktForm();
                rf.Show();
                this.Visible = false;
                rf.FormClosed += rf_FormClosed;
            }
        }

        void rf_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                listBox1.SetSelected(listBox1.SelectedIndex, true);
                listBox2.SelectedIndex = ind;
            }
            catch { }
            ConnectAndRead();
            this.Visible = true;
        }

        void textBox2_TextChanged(object sender, EventArgs e)
        {
            string lb2 = "";
            try { lb2 = textBox2.Text.Replace("\'", "\'\'"); }
            catch { }
            cn.Open();
            ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"SELECT distinct b.* FROM (book b inner join bookCategory bc on b.id_book=bc.id_book) 
inner join category c on bc.id_category=c.id_category where title like '%" + lb2 + "%' and category like '" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "book");
            listBox2.DataSource = ds.Tables[0];
            listBox2.DisplayMember = "title";
            cn.Close();
        }


        void label1_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            OleDbDataAdapter daCategory;
            if (blnSortAscCategory)
                daCategory = new OleDbDataAdapter(@"SELECT distinct category FROM category c inner join bookCategory bc on c.id_category=bc.id_category Order by category", cn);
            else
                daCategory = new OleDbDataAdapter(@"SELECT distinct category FROM category c inner join bookCategory bc on c.id_category=bc.id_category Order by category desc", cn);
            blnSortAscCategory = !blnSortAscCategory;
            daCategory.Fill(ds, "category");
            listBox1.DataSource = ds.Tables[0];
            listBox1.DisplayMember = "category";
        }

        void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lb2 = "";
            try { lb2 = listBox2.Text.Replace("\'", "\'\'"); }
            catch { }
            ds = new DataSet();
            daBook = new OleDbDataAdapter(@"SELECT b.* FROM book b where title='" + lb2 + "'", cn);
            daBook.Fill(ds, "book");
            try { curentBook = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString()); }
            catch { curentBook = 0; }
            label2.Text = "Название: " + listBox2.Text;
            label3.Text = "Автор: " + ds.Tables[0].Rows[0].ItemArray[2];
            label4.Text = "Издательство: " + ds.Tables[0].Rows[0].ItemArray[3];
            string dt = ds.Tables[0].Rows[0].ItemArray[4].ToString();
            label5.Text = "Дата выхода: " + Convert.ToDateTime(dt).Year;
            label6.Text = "Язык: " + ds.Tables[0].Rows[0].ItemArray[5];
            label7.Text = "Количество страниц: " + ds.Tables[0].Rows[0].ItemArray[6];
            textBox1.Text = ds.Tables[0].Rows[0].ItemArray[7].ToString();
            pictureBox1.ImageLocation = dbFollder + "img/" + ds.Tables[0].Rows[0].ItemArray[8];

            ds = new DataSet();
            daBook = new OleDbDataAdapter(@"SELECT g.* FROM genre g inner join bookGenre bg on g.id_genre=bg.id_genre where id_book=" + curentBook, cn);
            daBook.Fill(ds, "genre");
            string[] str = new string[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                str[i] = ds.Tables[0].Rows[i].ItemArray[1].ToString();
            textBox3.Lines = str;
        }


        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ds = new DataSet();
            OleDbDataAdapter daBook = new OleDbDataAdapter(@"SELECT distinct b.* FROM (book b inner join bookCategory bc on b.id_book=bc.id_book) 
inner join category c on bc.id_category=c.id_category where category='" + listBox1.Text + "'", cn);
            daBook.Fill(ds, "book");
            listBox2.DataSource = ds.Tables[0];
            listBox2.DisplayMember = "title";
        }

        void BooksForm_Load(object sender, EventArgs e)
        {
            try
            {
                cn = new OleDbConnection(ConnectionString);
                bs = new BindingSource();
                ConnectAndRead();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text);
                Application.Exit();
            }
        }

        void ConnectAndRead()
        {
            ds = new DataSet();
            OleDbDataAdapter daCategory = new OleDbDataAdapter(@"SELECT distinct category FROM category c inner join bookCategory bc on c.id_category=bc.id_category Order by category", cn);
            daCategory.Fill(ds, "category");
            listBox1.DataSource = ds.Tables[0];
            listBox1.DisplayMember = "category";
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
