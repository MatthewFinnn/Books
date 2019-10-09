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
    public partial class RedaktForm : Form
    {
        OleDbConnection cn;
        OleDbDataAdapter da;
        DataSet ds, dsl;
        string dbFollder, ConnectionString, image;
        int currentBook = BooksForm.curentBook;

        public RedaktForm()
        {
            InitializeComponent();
            dbFollder = FindFolder("Data");
            ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + dbFollder + "book.mdb";
            Load += RedaktForm_Load;
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
            dsl = new DataSet();
            da = new OleDbDataAdapter(@"SELECT * FROM lang where l like '" + textBox4.Text + "%'", cn);
            da.Fill(dsl, "lang");
            if (dsl.Tables[0].Rows.Count == 0 || textBox4.Text == "")
            { listBox1.Visible = false; return; }
            for (int i = 0; i < dsl.Tables[0].Rows.Count; i++)
            {
                listBox1.Items.Add(dsl.Tables[0].Rows[i].ItemArray[1].ToString());
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
            da.UpdateCommand = cn.CreateCommand();
            da.UpdateCommand.CommandText = "UPDATE book SET cover ='" + image.Replace("\'", "\'\'") + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET title ='" + textBox2.Text.Replace("\'", "\'\'") + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET author ='" + textBox1.Text.Replace("\'", "\'\'") + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET publishingHouse ='" + textBox3.Text.Replace("\'", "\'\'") + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET releaseDate ='" + dateTimePicker1.Value + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET sourceLanguage ='" + textBox4.Text + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET numberOfPage ='" + numberOfPage + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.CommandText = "UPDATE book SET annotation ='" + textBox7.Text.Replace("\'","\'\'") + "' WHERE id_book=" + currentBook;
            da.UpdateCommand.ExecuteNonQuery();
            da.DeleteCommand = cn.CreateCommand();
            da.DeleteCommand.CommandText = "DELETE FROM bookGenre WHERE id_book=" + currentBook;
            da.DeleteCommand.ExecuteNonQuery();
            CheckedListBox.CheckedItemCollection cic = checkedListBox1.CheckedItems;
            for (int i = 0; i < cic.Count; i++)
            {
                ds = new DataSet();
                da = new OleDbDataAdapter(@"SELECT * FROM genre where genre='" + cic[i] + "'", cn);
                da.Fill(ds, "genre");
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = "INSERT INTO bookGenre (id_book, id_genre) VALUES (" + currentBook + "," + ds.Tables[0].Rows[0].ItemArray[0].ToString() + ")";
                da.InsertCommand.ExecuteNonQuery();
            }
            da.DeleteCommand = cn.CreateCommand();
            da.DeleteCommand.CommandText = "DELETE FROM bookCategory WHERE id_book=" + currentBook;
            da.DeleteCommand.ExecuteNonQuery();
            cic = checkedListBox2.CheckedItems;
            for (int i = 0; i < cic.Count; i++)
            {
                ds = new DataSet();
                da = new OleDbDataAdapter(@"SELECT * FROM category where category='" + cic[i] + "'", cn);
                da.Fill(ds, "category");
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = "INSERT INTO bookCategory (id_book, id_category) VALUES (" + currentBook + "," + ds.Tables[0].Rows[0].ItemArray[0].ToString() + ")";
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

        void RedaktForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now;
            cn = new OleDbConnection(ConnectionString);
            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT b.* FROM book b where id_book=" + currentBook, cn);
            da.Fill(ds, "book");
            textBox1.Text = ds.Tables[0].Rows[0].ItemArray[2].ToString();
            textBox2.Text = ds.Tables[0].Rows[0].ItemArray[1].ToString();
            textBox3.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
            dateTimePicker1.Value = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[4]);
            textBox4.Text = ds.Tables[0].Rows[0].ItemArray[5].ToString();
            listBox1.Visible = false;
            textBox6.Text = ds.Tables[0].Rows[0].ItemArray[6].ToString();
            textBox7.Text = ds.Tables[0].Rows[0].ItemArray[7].ToString();
            pictureBox1.ImageLocation = dbFollder + "img/" + ds.Tables[0].Rows[0].ItemArray[8].ToString();
            image = ds.Tables[0].Rows[0].ItemArray[8].ToString();

            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT g.* FROM genre g inner join bookGenre bg on g.id_genre=bg.id_genre where id_book=" + currentBook, cn);
            da.Fill(ds, "genre");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox1.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), true);
            }
            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT * FROM genre where id_genre <>ANY
(SELECT g.id_genre FROM genre g left join bookGenre bg on g.id_genre=bg.id_genre where id_book is null or id_book=" + currentBook + ")", cn);
            da.Fill(ds, "genre");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox1.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), false);
            }

            ds = new DataSet();
            da = new OleDbDataAdapter(@"SELECT c.* FROM category c inner join bookCategory bc on c.id_category=bc.id_category where id_book=" + currentBook, cn);
            da.Fill(ds, "category");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox2.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), true);
            }

            ds = new DataSet();
            da = new OleDbDataAdapter(@"select * from category where id_category <>ANY
(SELECT c.id_category FROM category c inner join bookCategory bc on c.id_category=bc.id_category where id_book=" + currentBook + ")", cn);
            da.Fill(ds, "category");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i].ItemArray[1].ToString();
                checkedListBox2.Items.Add(ds.Tables[0].Rows[i].ItemArray[1].ToString(), false);
            }
            checkedListBox2.Sorted = true;
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
