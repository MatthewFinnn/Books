namespace books
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.bookDataSet = new books.bookDataSet();
            this.CbookBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CbookTableAdapter = new books.bookDataSetTableAdapters.CbookTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bookDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbookBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.CbookBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "books.Report2.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(784, 561);
            this.reportViewer1.TabIndex = 0;
            // 
            // bookDataSet
            // 
            this.bookDataSet.DataSetName = "bookDataSet";
            this.bookDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // CbookBindingSource
            // 
            this.CbookBindingSource.DataMember = "Cbook";
            this.CbookBindingSource.DataSource = this.bookDataSet;
            // 
            // CbookTableAdapter
            // 
            this.CbookTableAdapter.ClearBeforeFill = true;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ReportForm";
            this.Text = "Отчет";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bookDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbookBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource CbookBindingSource;
        private bookDataSet bookDataSet;
        private bookDataSetTableAdapters.CbookTableAdapter CbookTableAdapter;


    }
}