namespace ChatToMarkdown;

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
        ConvertButton = new Button();
        JsonTextBox = new TextBox();
        openCheckBox = new CheckBox();
        SuspendLayout();
        // 
        // ConvertButton
        // 
        ConvertButton.Location = new Point(1023, 651);
        ConvertButton.Name = "ConvertButton";
        ConvertButton.Size = new Size(153, 37);
        ConvertButton.TabIndex = 0;
        ConvertButton.Text = "Convert to Markdown";
        ConvertButton.UseVisualStyleBackColor = true;
        ConvertButton.Click += ConvertButton_Click;
        // 
        // JsonTextBox
        // 
        JsonTextBox.Location = new Point(29, 32);
        JsonTextBox.MaxLength = 327670000;
        JsonTextBox.Multiline = true;
        JsonTextBox.Name = "JsonTextBox";
        JsonTextBox.ScrollBars = ScrollBars.Vertical;
        JsonTextBox.Size = new Size(1147, 604);
        JsonTextBox.TabIndex = 1;
        // 
        // openCheckBox
        // 
        openCheckBox.AutoSize = true;
        openCheckBox.Location = new Point(902, 661);
        openCheckBox.Name = "openCheckBox";
        openCheckBox.Size = new Size(97, 19);
        openCheckBox.TabIndex = 2;
        openCheckBox.Text = "Open md File";
        openCheckBox.UseVisualStyleBackColor = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1209, 707);
        Controls.Add(openCheckBox);
        Controls.Add(JsonTextBox);
        Controls.Add(ConvertButton);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = "MainForm";
        Text = "ChatGPT Json to Markdown";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button ConvertButton;
    private TextBox JsonTextBox;
    private CheckBox openCheckBox;
}