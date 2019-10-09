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
    public partial class ReportForm : Form
    {
        string dbFollder, ConnectionString;
        DataSet ds;
        OleDbConnection cn;

        public ReportForm()
        {
            InitializeComponent();
            dbFollder = FindFolder("Data");
            ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + dbFollder + "book.mdb";
            Load += ReportForm_Load;
        }

        void ReportForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bookDataSet.Cbook". При необходимости она может быть перемещена или удалена.
            cn = new OleDbConnection(ConnectionString);
            CbookTableAdapter.Connection = cn;
            this.CbookTableAdapter.Fill(this.bookDataSet.Cbook);
            reportViewer1.RefreshReport();
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
