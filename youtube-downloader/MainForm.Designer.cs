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
        textBox1 = new TextBox();
        button1 = new Button();
        ProcessingProgressBar = new ProgressBar();
        StatusLabel = new Label();
        TranscribeButton = new Button();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(101, 52);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(402, 23);
        textBox1.TabIndex = 0;
        textBox1.Text = "https://www.youtube.com/watch?v=JjyR5ud57mU&t=1s";
        // 
        // button1
        // 
        button1.Location = new Point(541, 291);
        button1.Name = "button1";
        button1.Size = new Size(194, 23);
        button1.TabIndex = 1;
        button1.Text = "Save with AI Subtitles";
        button1.UseVisualStyleBackColor = true;
        button1.Click += DownloadButton_Click;
        // 
        // ProcessingProgressBar
        // 
        ProcessingProgressBar.Location = new Point(133, 223);
        ProcessingProgressBar.Name = "ProcessingProgressBar";
        ProcessingProgressBar.Size = new Size(483, 23);
        ProcessingProgressBar.Step = 1;
        ProcessingProgressBar.TabIndex = 2;
        // 
        // StatusLabel
        // 
        StatusLabel.AutoSize = true;
        StatusLabel.Location = new Point(118, 184);
        StatusLabel.Name = "StatusLabel";
        StatusLabel.Size = new Size(38, 15);
        StatusLabel.TabIndex = 3;
        StatusLabel.Text = "label1";
        // 
        // TranscribeButton
        // 
        TranscribeButton.Location = new Point(551, 326);
        TranscribeButton.Name = "TranscribeButton";
        TranscribeButton.Size = new Size(184, 23);
        TranscribeButton.TabIndex = 4;
        TranscribeButton.Text = "Transcribe";
        TranscribeButton.UseVisualStyleBackColor = true;
        TranscribeButton.Click += TranscribeButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(TranscribeButton);
        Controls.Add(StatusLabel);
        Controls.Add(ProcessingProgressBar);
        Controls.Add(button1);
        Controls.Add(textBox1);
        Name = "MainForm";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox1;
    private Button button1;
    private ProgressBar ProcessingProgressBar;
    private Label StatusLabel;
    private Button TranscribeButton;
}