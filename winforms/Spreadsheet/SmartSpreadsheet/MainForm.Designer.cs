namespace SmartSpreadsheet
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook2 = new Telerik.Windows.Documents.Spreadsheet.Model.Workbook();
            Telerik.Windows.Documents.Model.DocumentInfo documentInfo2 = new Telerik.Windows.Documents.Model.DocumentInfo();
            this.radSpreadsheetRibbonBar1 = new Telerik.WinControls.UI.RadSpreadsheetRibbonBar();
            this.radSpreadsheetSmart = new Telerik.WinControls.UI.RadSpreadsheet();
            this.radRibbonFormBehavior1 = new Telerik.WinControls.UI.RadRibbonFormBehavior();
            ((System.ComponentModel.ISupportInitialize)this.radSpreadsheetRibbonBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.radSpreadsheetSmart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            this.SuspendLayout();
            // 
            // radSpreadsheetRibbonBar1
            // 
            this.radSpreadsheetRibbonBar1.AssociatedSpreadsheet = this.radSpreadsheetSmart;
            this.radSpreadsheetRibbonBar1.Location = new Point(0, 0);
            this.radSpreadsheetRibbonBar1.Name = "radSpreadsheetRibbonBar1";
            this.radSpreadsheetRibbonBar1.Size = new Size(982, 171);
            this.radSpreadsheetRibbonBar1.TabIndex = 0;
            this.radSpreadsheetRibbonBar1.Text = "Form1";
            // 
            // radSpreadsheetSmart
            // 
            this.radSpreadsheetSmart.Dock = DockStyle.Fill;
            this.radSpreadsheetSmart.Location = new Point(0, 171);
            this.radSpreadsheetSmart.Name = "radSpreadsheetSmart";
            this.radSpreadsheetSmart.Size = new Size(982, 665);
            this.radSpreadsheetSmart.TabIndex = 5;
            workbook2.ActiveTabIndex = -1;
            documentInfo2.Author = null;
            documentInfo2.Description = null;
            documentInfo2.Keywords = null;
            documentInfo2.Subject = null;
            documentInfo2.Title = null;
            workbook2.DocumentInfo = documentInfo2;
            workbook2.Name = "Book1";
            workbook2.WorkbookContentChangedInterval = TimeSpan.Parse("00:00:00.0300000");
            this.radSpreadsheetSmart.Workbook = workbook2;
            // 
            // radRibbonFormBehavior1
            // 
            this.radRibbonFormBehavior1.Form = this;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new Size(7, 15);
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(982, 836);
            this.Controls.Add(this.radSpreadsheetSmart);
            this.Controls.Add(this.radSpreadsheetRibbonBar1);
            this.FormBehavior = this.radRibbonFormBehavior1;
            this.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None;
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)this.radSpreadsheetRibbonBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.radSpreadsheetSmart).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Telerik.WinControls.UI.RadSpreadsheetRibbonBar radSpreadsheetRibbonBar1;
        private Telerik.WinControls.UI.RadRibbonFormBehavior radRibbonFormBehavior1;
        private Telerik.WinControls.UI.RadSpreadsheet radSpreadsheetSmart;
    }
}
