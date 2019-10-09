using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace books
{
    public partial class CreateForm : Form
    {
        OleDbConnection cn;
        OleDbDataAdapter da;
        DataSet ds;
        string dbFollder, ConnectionString, image;
        int currentBook = BooksForm.curentBook;

        public CreateForm()
        {
            InitializeComponent();
            dbFollder = FindFolder("Data");
            ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + dbFollder + "book.mdb";
            Load += CreateForm_Load;
            label1.MouseEnter += label1_MouseEnter;
            label1.MouseLeave += label1_MouseLeave;
            label8.MouseEnter += label8_MouseEnter;
            label8.MouseLeave += label8_MouseLeave;
            label1.Click += label1_Click;
            pictureBox1.DoubleClick += pictureBox1_DoubleClick;
            label8.Click += label8_Click;
            textBox6.KeyPress += textBox6_KeyPress;
            textBox4.KeyPress += textBox4_KeyPress;
            textBox4.TextChanged += textBox4_TextChanged;
            listBox1.DoubleClick += listBox1_DoubleClick;
        }

        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox4.Text = listBox1.Text;
            listBox1.Visible = false;
        }

        void textBox4_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT * FROM lang where l like '"+textBox4.Text+"%'", cn);
            da.Fill(ds, "lang");
            if (ds.Tables[0].Rows.Count == 0 || textBox4.Text=="")
            { listBox1.Visible = false; return; }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString());
            }
            listBox1.Visible = true;
        }

        void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar == 8)
            { e.Handled = false; return; }
            if (e.KeyChar >= 1040 && e.KeyChar <= 1103)
                e.Handled = false;
        }

        void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            { e.Handled = false; return; }
            if (e.KeyChar < 48 || e.KeyChar >= 59)
                e.Handled = true;
        }

        void CreateForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            cn = new OleDbConnection(ConnectionString);
            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT * FROM genre", cn);
            da.Fill(ds, "genre");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox1.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), false);
            }

            ds = new DataSet();
            da = new OleDbDataAdapter(@"select * from category ", cn);
            da.Fill(ds, "category");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox2.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), false);
            }
            checkedListBox2.Sorted = true;
        }


        void label8_Click(object sender, EventArgs e)
        {
            string numberOfPage = textBox6.Text;
            if (textBox2.Text == "") { MessageBox.Show("Введите назвние книги", "ERROR"); textBox2.Focus(); return; }
            if (textBox1.Text == "") { MessageBox.Show("Введите автора", "ERROR"); textBox1.Focus(); return; }
            CheckedListBox.CheckedItemCollection cic1 = checkedListBox1.CheckedItems;
            if (cic1.Count == 0) { MessageBox.Show("Выберите жанр", "ERROR"); checkedListBox1.Focus(); return; }
            CheckedListBox.CheckedItemCollection cic2 = checkedListBox2.CheckedItems;
            if (cic2.Count == 0) { MessageBox.Show("Выберите категорию", "ERROR"); checkedListBox2.Focus(); return; }
            if (numberOfPage == "") numberOfPage = "null";
            cn.Open();
            string tb1 = "",tb2="",tb3="",tb7="";
            try { tb3= textBox3.Text.Replace("\'", "\'\'"); }
            catch { }
            try { tb2 = textBox2.Text.Replace("\'", "\'\'"); }
            catch { }
            try { tb1= textBox1.Text.Replace("\'", "\'\'"); }
            catch { }
            try { tb7 = textBox7.Text.Replace("\'", "\'\'"); }
            catch { }
            try { image.Replace("\'", "\'\'"); }
            catch { }
            ds = new DataSet();
            da.InsertCommand = cn.CreateCommand();
            da.InsertCommand.CommandText = @"INSERT INTO book (title, author, publishingHouse, releaseDate, sourceLanguage, numberOfPage, annotation, cover)
VALUES ('" + tb2 + "','" + tb1 + "','" + textBox3.Text+ "','" + dateTimePicker1.Value + "','" + textBox4.Text +
          "'," + numberOfPage + ",'" + tb7 + "','" + image + "')";
            da.InsertCommand.ExecuteNonQuery();

            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT * FROM book where title='"+ tb2+ "'", cn);
            da.Fill(ds, "genre");
            int x = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());

            for (int i = 0; i < cic1.Count; i++)
            {
                ds = new DataSet();
                da = new OleDbDataAdapter(@"SELECT * FROM genre where genre='" + cic1[i] + "'", cn);
                da.Fill(ds, "genre");
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = "INSERT INTO bookGenre (id_book, id_genre) VALUES (" + x + "," + ds.Tables[0].Rows[0].ItemArray[0].ToString() + ")";
                da.InsertCommand.ExecuteNonQuery();
            }

            for (int i = 0; i < cic2.Count; i++)
            {
                ds = new DataSet();
                da = new OleDbDataAdapter(@"SELECT * FROM category where category='" + cic2[i] + "'", cn);
                da.Fill(ds, "category");
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = "INSERT INTO bookCategory (id_book, id_category) VALUES (" + x + "," + ds.Tables[0].Rows[0].ItemArray[0].ToString() + ")";
                da.InsertCommand.ExecuteNonQuery();
            }
            cn.Close();
            this.Close();
        }

        void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            da.Fill(ds, "book");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = dbFollder + "img";
            ofd.Filter = ".jpg|*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = ofd.FileName;
                image = ofd.SafeFileName;
            }
        }

        void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void label8_MouseLeave(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Gray;
        }

        void label8_MouseEnter(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Black;
        }

        void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Gray;
        }

        void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
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
