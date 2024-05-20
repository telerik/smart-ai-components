namespace SmartAIComponents
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
            this.radRichTextEditorRuler1 = new Telerik.WinControls.UI.RadRichTextEditorRuler();
            this.radRichTextSmartEditor = new Telerik.WinControls.UI.RadRichTextEditor();
            this.richTextEditorRibbonBar1 = new Telerik.WinControls.UI.RichTextEditorRibbonBar();
            this.radRibbonFormBehavior1 = new Telerik.WinControls.UI.RadRibbonFormBehavior();
            ((System.ComponentModel.ISupportInitialize)this.radRichTextEditorRuler1).BeginInit();
            this.radRichTextEditorRuler1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.radRichTextSmartEditor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.richTextEditorRibbonBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            this.SuspendLayout();
            // 
            // radRichTextEditorRuler1
            // 
            this.radRichTextEditorRuler1.AssociatedRichTextBox = this.radRichTextSmartEditor;
            this.radRichTextEditorRuler1.Controls.Add(this.radRichTextSmartEditor);
            this.radRichTextEditorRuler1.Dock = DockStyle.Fill;
            this.radRichTextEditorRuler1.Location = new Point(0, 225);
            this.radRichTextEditorRuler1.Name = "radRichTextEditorRuler1";
            this.radRichTextEditorRuler1.Size = new Size(768, 475);
            this.radRichTextEditorRuler1.TabIndex = 3;
            // 
            // radRichTextSmartEditor
            // 
            this.radRichTextSmartEditor.Location = new Point(29, 29);
            this.radRichTextSmartEditor.Name = "radRichTextSmartEditor";
            this.radRichTextSmartEditor.Size = new Size(738, 445);
            this.radRichTextSmartEditor.TabIndex = 0;
            // 
            // richTextEditorRibbonBar1
            // 
            this.richTextEditorRibbonBar1.AssociatedRichTextEditor = this.radRichTextSmartEditor;
            this.richTextEditorRibbonBar1.Location = new Point(0, 0);
            this.richTextEditorRibbonBar1.Name = "richTextEditorRibbonBar1";
            this.richTextEditorRibbonBar1.Size = new Size(768, 225);
            this.richTextEditorRibbonBar1.TabIndex = 1;
            this.richTextEditorRibbonBar1.TabStop = false;
            this.richTextEditorRibbonBar1.Text = "MainForm";
            // 
            // radRibbonFormBehavior1
            // 
            this.radRibbonFormBehavior1.Form = this;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new Size(7, 15);
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(768, 700);
            this.Controls.Add(this.radRichTextEditorRuler1);
            this.Controls.Add(this.richTextEditorRibbonBar1);
            this.FormBehavior = this.radRibbonFormBehavior1;
            this.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None;
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)this.radRichTextEditorRuler1).EndInit();
            this.radRichTextEditorRuler1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.radRichTextSmartEditor).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.richTextEditorRibbonBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private Telerik.WinControls.UI.RadRichTextEditorRuler radRichTextEditorRuler1;
        private Telerik.WinControls.UI.RadRichTextEditor radRichTextSmartEditor;
        private Telerik.WinControls.UI.RichTextEditorRibbonBar richTextEditorRibbonBar1;
        private Telerik.WinControls.UI.RadRibbonFormBehavior radRibbonFormBehavior1;
    }
}
