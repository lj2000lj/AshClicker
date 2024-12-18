using System.ComponentModel;

namespace AshClicker;

partial class KeySelectionFrame
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeySelectionFrame));
        textBox1 = new System.Windows.Forms.TextBox();
        textBox2 = new System.Windows.Forms.TextBox();
        comboBox1 = new System.Windows.Forms.ComboBox();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        button1 = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
        textBox1.Location = new System.Drawing.Point(12, 51);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(89, 27);
        textBox1.TabIndex = 0;
        textBox1.Text = "（按下选择）";
        textBox1.KeyDown += textBox_KeyUp;
        textBox1.KeyUp += textBox_KeyUp;
        // 
        // textBox2
        // 
        textBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
        textBox2.Location = new System.Drawing.Point(107, 51);
        textBox2.Name = "textBox2";
        textBox2.Size = new System.Drawing.Size(89, 27);
        textBox2.TabIndex = 1;
        textBox2.Text = "（按下选择）";
        textBox2.KeyDown += textBox_KeyUp;
        textBox2.KeyUp += textBox_KeyUp;
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Items.AddRange(new object[] { "长按连发", "按键切换" });
        comboBox1.Location = new System.Drawing.Point(202, 50);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new System.Drawing.Size(96, 28);
        comboBox1.TabIndex = 2;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(12, 29);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(89, 19);
        label1.TabIndex = 3;
        label1.Text = "按下的键";
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(107, 29);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(89, 23);
        label2.TabIndex = 4;
        label2.Text = "连发的按键";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(202, 29);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(82, 23);
        label3.TabIndex = 5;
        label3.Text = "按键模式";
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(124, 84);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(174, 44);
        button1.TabIndex = 6;
        button1.Text = "添加按键";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // KeySelectionFrame
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(310, 140);
        Controls.Add(button1);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(comboBox1);
        Controls.Add(textBox2);
        Controls.Add(textBox1);
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        Text = "按键选择";
        Load += KeySelectionFrame_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;

    #endregion
}