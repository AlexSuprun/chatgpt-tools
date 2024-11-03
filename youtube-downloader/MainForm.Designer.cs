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
        DownloadVideoButton = new Button();
        ProcessingProgressBar = new ProgressBar();
        StatusLabel = new Label();
        DownloadAudioButton = new Button();
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
        // DownloadVideoButton
        // 
        DownloadVideoButton.Location = new Point(286, 145);
        DownloadVideoButton.Name = "DownloadVideoButton";
        DownloadVideoButton.Size = new Size(194, 31);
        DownloadVideoButton.TabIndex = 1;
        DownloadVideoButton.Text = "Download Video";
        DownloadVideoButton.UseVisualStyleBackColor = true;
        DownloadVideoButton.Click += DownloadVideoButton_Click;
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
        // DownloadAudioButton
        // 
        DownloadAudioButton.Location = new Point(61, 145);
        DownloadAudioButton.Name = "DownloadAudioButton";
        DownloadAudioButton.Size = new Size(194, 31);
        DownloadAudioButton.TabIndex = 4;
        DownloadAudioButton.Text = "Download Audio";
        DownloadAudioButton.UseVisualStyleBackColor = true;
        DownloadAudioButton.Click += DownloadAudioButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(499, 193);
        Controls.Add(DownloadAudioButton);
        Controls.Add(StatusLabel);
        Controls.Add(ProcessingProgressBar);
        Controls.Add(DownloadVideoButton);
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
    private Button DownloadVideoButton;
    private ProgressBar ProcessingProgressBar;
    private Label StatusLabel;
    private Button DownloadAudioButton;
}