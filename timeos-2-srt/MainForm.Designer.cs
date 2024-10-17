namespace TimeOs2Srt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            TranscriptTextBox = new TextBox();
            ConvertButton = new Button();
            OpenCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // TranscriptTextBox
            // 
            TranscriptTextBox.Location = new Point(31, 24);
            TranscriptTextBox.MaxLength = 327670000;
            TranscriptTextBox.Multiline = true;
            TranscriptTextBox.Name = "TranscriptTextBox";
            TranscriptTextBox.ScrollBars = ScrollBars.Vertical;
            TranscriptTextBox.Size = new Size(1147, 604);
            TranscriptTextBox.TabIndex = 3;
            // 
            // ConvertButton
            // 
            ConvertButton.Location = new Point(1025, 644);
            ConvertButton.Name = "ConvertButton";
            ConvertButton.Size = new Size(153, 37);
            ConvertButton.TabIndex = 2;
            ConvertButton.Text = "Convert to .srt";
            ConvertButton.UseVisualStyleBackColor = true;
            ConvertButton.Click += ConvertToSrt_Click;
            // 
            // OpenCheckBox
            // 
            OpenCheckBox.AutoSize = true;
            OpenCheckBox.Location = new Point(911, 654);
            OpenCheckBox.Name = "OpenCheckBox";
            OpenCheckBox.Size = new Size(76, 19);
            OpenCheckBox.TabIndex = 4;
            OpenCheckBox.Text = "Open Video";
            OpenCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1209, 707);
            Controls.Add(OpenCheckBox);
            Controls.Add(TranscriptTextBox);
            Controls.Add(ConvertButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Convert TimeOS transcript to .srt";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TranscriptTextBox;
        private Button ConvertButton;
        private CheckBox OpenCheckBox;
    }
}
