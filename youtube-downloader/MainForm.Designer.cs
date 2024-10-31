namespace YoutubeDownloader;

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
        UrlTextBox = new TextBox();
        DownloadButton = new Button();
        ProcessingProgressBar = new ProgressBar();
        StatusLabel = new Label();
        SuspendLayout();
        // 
        // UrlTextBox
        // 
        UrlTextBox.Location = new Point(22, 25);
        UrlTextBox.Name = "UrlTextBox";
        UrlTextBox.Size = new Size(458, 23);
        UrlTextBox.TabIndex = 0;
        UrlTextBox.Text = "https://www.youtube.com/watch?v=JjyR5ud57mU&t=1s";
        // 
        // DownloadButton
        // 
        DownloadButton.Location = new Point(286, 145);
        DownloadButton.Name = "DownloadButton";
        DownloadButton.Size = new Size(194, 31);
        DownloadButton.TabIndex = 1;
        DownloadButton.Text = "Download";
        DownloadButton.UseVisualStyleBackColor = true;
        DownloadButton.Click += DownloadButton_Click;
        // 
        // ProcessingProgressBar
        // 
        ProcessingProgressBar.Location = new Point(22, 100);
        ProcessingProgressBar.Name = "ProcessingProgressBar";
        ProcessingProgressBar.Size = new Size(458, 23);
        ProcessingProgressBar.Step = 1;
        ProcessingProgressBar.TabIndex = 2;
        // 
        // StatusLabel
        // 
        StatusLabel.AutoSize = true;
        StatusLabel.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        StatusLabel.Location = new Point(22, 72);
        StatusLabel.Name = "StatusLabel";
        StatusLabel.Size = new Size(0, 25);
        StatusLabel.TabIndex = 3;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(499, 193);
        Controls.Add(StatusLabel);
        Controls.Add(ProcessingProgressBar);
        Controls.Add(DownloadButton);
        Controls.Add(UrlTextBox);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Download Youtube Video";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox UrlTextBox;
    private Button DownloadButton;
    private ProgressBar ProcessingProgressBar;
    private Label StatusLabel;
}