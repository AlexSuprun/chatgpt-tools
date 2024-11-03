namespace SubtitlesGenerator
{
    partial class MainForm
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
            GenerateSubtitlesButton = new Button();
            SuspendLayout();
            // 
            // GenerateSubtitlesButton
            // 
            GenerateSubtitlesButton.Location = new Point(366, 143);
            GenerateSubtitlesButton.Name = "GenerateSubtitlesButton";
            GenerateSubtitlesButton.Size = new Size(75, 23);
            GenerateSubtitlesButton.TabIndex = 0;
            GenerateSubtitlesButton.Text = "Generate Subtitles";
            GenerateSubtitlesButton.UseVisualStyleBackColor = true;
            GenerateSubtitlesButton.Click += GenerateSubtitles_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(499, 193);
            Controls.Add(GenerateSubtitlesButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Subtitles Generator";
            ResumeLayout(false);
        }

        #endregion

        private Button GenerateSubtitlesButton;
    }
}