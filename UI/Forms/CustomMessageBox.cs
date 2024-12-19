namespace AshClicker;

using System;
using System.Diagnostics;
using System.Windows.Forms;

public class CustomMessageBox : Form
{
    private Button btnOk;
    private LinkLabel linkLabel;

    public CustomMessageBox(string message, string url)
    {
        Text = "消息";
        Width = 400;
        Height = 200;
        StartPosition = FormStartPosition.CenterScreen;

        linkLabel = new LinkLabel
        {
            Text = message,
            AutoSize = true,
            Location = new Point(20, 20),
        };
        linkLabel.LinkClicked += (sender, e) => Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });

        btnOk = new Button
        {
            Text = "好的",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(150, 100),
            AutoSize = true
        };

        Controls.Add(linkLabel);
        Controls.Add(btnOk);
    }

    public static void Show(string message, string url)
    {
        using (var form = new CustomMessageBox(message, url))
        {
            form.ShowDialog();
        }
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomMessageBox));
        SuspendLayout();
        // 
        // CustomMessageBox
        // 
        ClientSize = new System.Drawing.Size(282, 253);
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        ResumeLayout(false);
    }
}
