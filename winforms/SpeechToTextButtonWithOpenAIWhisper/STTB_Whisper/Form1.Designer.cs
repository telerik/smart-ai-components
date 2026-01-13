namespace SpeechToTextButtonWithOpenAIWhisper
{
    partial class Form1
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
            this.speechToTextButton = new Telerik.WinControls.UI.RadSpeechToTextButton();
            this.textBox1 = new TextBox();
            this.SuspendLayout();
            // 
            // speechToTextButton
            // 
            this.speechToTextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.speechToTextButton.Location = new Point(736, 386);
            this.speechToTextButton.Name = "speechToTextButton";
            this.speechToTextButton.Size = new Size(52, 52);
            this.speechToTextButton.TabIndex = 0;
            this.speechToTextButton.Text = "start";
            this.speechToTextButton.UseVisualStyleBackColor = true;
            this.speechToTextButton.IsContinuousRecognition = false;
            this.speechToTextButton.ErrorOccurred += this.SpeechToTextButton_ErrorOccurred;
            this.speechToTextButton.StateChanged += this.SpeechToTextButton_StateChanged;
            this.speechToTextButton.SpeechRecognized += this.SpeechToTextButton_SpeechRecognized;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBox1.Location = new Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(776, 368);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += this.TextBox1_TextChanged;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(15F, 37F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.speechToTextButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Telerik.WinControls.UI.RadSpeechToTextButton speechToTextButton;
        private TextBox textBox1;
    }
}
