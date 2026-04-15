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
            textBoxControl = new Telerik.WinControls.UI.RadTextBoxControl();
            speechToTextButton = new Telerik.WinControls.UI.RadSpeechToTextButton();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)textBoxControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)speechToTextButton).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxControl
            // 
            textBoxControl.AcceptsReturn = true;
            textBoxControl.Dock = DockStyle.Fill;
            textBoxControl.Location = new Point(0, 0);
            textBoxControl.Margin = new Padding(0);
            textBoxControl.Name = "textBoxControl";
            textBoxControl.Size = new Size(512, 232);
            textBoxControl.TabIndex = 0;
            textBoxControl.SelectionChanged += TextBoxControl_SelectionChanged;
            // 
            // speechToTextButton
            // 
            speechToTextButton.Dock = DockStyle.Right;
            speechToTextButton.Location = new Point(444, 0);
            speechToTextButton.Margin = new Padding(0);
            speechToTextButton.Name = "speechToTextButton";
            speechToTextButton.IsContinuousRecognition = false;
            speechToTextButton.RootElement.UseDefaultDisabledPaint = false;
            speechToTextButton.Size = new Size(68, 47);
            speechToTextButton.TabIndex = 0;
            speechToTextButton.ErrorOccurred += SpeechToTextButton_ErrorOccurred;
            speechToTextButton.SpeechRecognized += SpeechToTextButton_SpeechRecognized;
            speechToTextButton.StateChanged += SpeechToTextButton_StateChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(textBoxControl);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(10, 10);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 0, 4);
            panel1.Size = new Size(512, 236);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(speechToTextButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(10, 246);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(512, 47);
            panel2.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(532, 303);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Margin = new Padding(2);
            Name = "Form1";
            Padding = new Padding(10);
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)textBoxControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)speechToTextButton).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadTextBoxControl textBoxControl;
        private Telerik.WinControls.UI.RadSpeechToTextButton speechToTextButton;
        private Panel panel2;
        private Panel panel1;
    }
}
