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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        ConvertButton = new Button();
        JsonTextBox = new TextBox();
        OpenCheckBox = new CheckBox();
        SaveJsonCheckBox = new CheckBox();
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
        OpenCheckBox.AutoSize = true;
        OpenCheckBox.Location = new Point(902, 661);
        OpenCheckBox.Name = "OpenCheckBox";
        OpenCheckBox.Size = new Size(97, 19);
        OpenCheckBox.TabIndex = 2;
        OpenCheckBox.Text = "Open md File";
        OpenCheckBox.UseVisualStyleBackColor = true;
        // 
        // SaveJsonCheckBox
        // 
        SaveJsonCheckBox.AutoSize = true;
        SaveJsonCheckBox.Location = new Point(770, 661);
        SaveJsonCheckBox.Name = "SaveJsonCheckBox";
        SaveJsonCheckBox.Size = new Size(81, 19);
        SaveJsonCheckBox.TabIndex = 3;
        SaveJsonCheckBox.Text = "Save JSON";
        SaveJsonCheckBox.UseVisualStyleBackColor = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1209, 707);
        Controls.Add(SaveJsonCheckBox);
        Controls.Add(OpenCheckBox);
        Controls.Add(JsonTextBox);
        Controls.Add(ConvertButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MainForm";
        Text = "ChatGPT Json to Markdown";
        StartPosition = FormStartPosition.CenterScreen;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button ConvertButton;
    private TextBox JsonTextBox;
    private CheckBox OpenCheckBox;
    private CheckBox SaveJsonCheckBox;
}