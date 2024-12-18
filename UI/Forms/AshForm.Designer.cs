using System.ComponentModel;

namespace AshClicker;

partial class AshForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AshForm));
        buttonLoadDriver = new System.Windows.Forms.Button();
        buttonSwitchEnable = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        textBox1 = new System.Windows.Forms.TextBox();
        textBox2 = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        label5 = new System.Windows.Forms.Label();
        listView1 = new System.Windows.Forms.ListView();
        按下的键 = new System.Windows.Forms.ColumnHeader();
        连发的键 = new System.Windows.Forms.ColumnHeader();
        连发模式 = new System.Windows.Forms.ColumnHeader();
        buttonDeleteKey = new System.Windows.Forms.Button();
        buttonAddKey = new System.Windows.Forms.Button();
        preciseDelay = new System.Windows.Forms.CheckBox();
        SuspendLayout();
        // 
        // buttonLoadDriver
        // 
        buttonLoadDriver.Location = new System.Drawing.Point(237, 136);
        buttonLoadDriver.Name = "buttonLoadDriver";
        buttonLoadDriver.Size = new System.Drawing.Size(154, 39);
        buttonLoadDriver.TabIndex = 0;
        buttonLoadDriver.Text = "加载 DD 驱动";
        buttonLoadDriver.UseVisualStyleBackColor = true;
        buttonLoadDriver.Click += button1_Click;
        // 
        // buttonSwitchEnable
        // 
        buttonSwitchEnable.Location = new System.Drawing.Point(237, 181);
        buttonSwitchEnable.Name = "buttonSwitchEnable";
        buttonSwitchEnable.Size = new System.Drawing.Size(154, 37);
        buttonSwitchEnable.TabIndex = 1;
        buttonSwitchEnable.Text = "按键开启";
        buttonSwitchEnable.UseVisualStyleBackColor = true;
        buttonSwitchEnable.Click += button2_Click;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(19, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(370, 105);
        label1.TabIndex = 2;
        label1.Text = ("真不联网的公告：\r\n  - 当前版本 Alpha 0.0.1\r\n  - 能用就行，有问题请前来 GitHub 提 Issue\r\n  - 需要自行安装 DD 驱动并" + "加载对应的 Dll");
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(19, 95);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(372, 19);
        label2.TabIndex = 3;
        label2.Text = "当前状态：";
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(98, 95);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(75, 19);
        label3.TabIndex = 4;
        label3.Text = "关闭";
        // 
        // textBox1
        // 
        textBox1.Location = new System.Drawing.Point(19, 136);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(154, 27);
        textBox1.TabIndex = 5;
        textBox1.Text = "15";
        textBox1.TextChanged += textBox1_TextChanged;
        // 
        // textBox2
        // 
        textBox2.Location = new System.Drawing.Point(19, 191);
        textBox2.Name = "textBox2";
        textBox2.Size = new System.Drawing.Size(154, 27);
        textBox2.TabIndex = 6;
        textBox2.Text = "1";
        textBox2.TextChanged += textBox2_TextChanged;
        // 
        // label4
        // 
        label4.Location = new System.Drawing.Point(19, 114);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(134, 22);
        label4.TabIndex = 7;
        label4.Text = "连发间隔（毫秒）";
        // 
        // label5
        // 
        label5.Location = new System.Drawing.Point(19, 166);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(134, 22);
        label5.TabIndex = 8;
        label5.Text = "抬起间隔（毫秒）";
        // 
        // listView1
        // 
        listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { 按下的键, 连发的键, 连发模式 });
        listView1.FullRowSelect = true;
        listView1.GridLines = true;
        listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        listView1.Location = new System.Drawing.Point(19, 224);
        listView1.MultiSelect = false;
        listView1.Name = "listView1";
        listView1.Size = new System.Drawing.Size(372, 171);
        listView1.TabIndex = 9;
        listView1.UseCompatibleStateImageBehavior = false;
        listView1.View = System.Windows.Forms.View.Details;
        // 
        // 按下的键
        // 
        按下的键.Name = "按下的键";
        按下的键.Text = "按下的键";
        按下的键.Width = 104;
        // 
        // 连发的键
        // 
        连发的键.Name = "连发的键";
        连发的键.Text = "连发的键";
        连发的键.Width = 100;
        // 
        // 连发模式
        // 
        连发模式.Name = "连发模式";
        连发模式.Text = "连发模式";
        连发模式.Width = 160;
        // 
        // buttonDeleteKey
        // 
        buttonDeleteKey.Location = new System.Drawing.Point(237, 401);
        buttonDeleteKey.Name = "buttonDeleteKey";
        buttonDeleteKey.Size = new System.Drawing.Size(154, 53);
        buttonDeleteKey.TabIndex = 10;
        buttonDeleteKey.Text = "删除键位";
        buttonDeleteKey.UseVisualStyleBackColor = true;
        buttonDeleteKey.Click += buttonDeleteKey_Click;
        // 
        // buttonAddKey
        // 
        buttonAddKey.Location = new System.Drawing.Point(19, 401);
        buttonAddKey.Name = "buttonAddKey";
        buttonAddKey.Size = new System.Drawing.Size(154, 53);
        buttonAddKey.TabIndex = 11;
        buttonAddKey.Text = "添加键位";
        buttonAddKey.UseVisualStyleBackColor = true;
        buttonAddKey.Click += buttonAddKey_Click;
        // 
        // preciseDelay
        // 
        preciseDelay.Checked = true;
        preciseDelay.CheckState = System.Windows.Forms.CheckState.Checked;
        preciseDelay.Location = new System.Drawing.Point(237, 111);
        preciseDelay.Name = "preciseDelay";
        preciseDelay.Size = new System.Drawing.Size(104, 24);
        preciseDelay.TabIndex = 12;
        preciseDelay.Text = "精确延迟";
        preciseDelay.UseVisualStyleBackColor = true;
        // 
        // AshForm
        // 
        AllowDrop = true;
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(405, 466);
        Controls.Add(preciseDelay);
        Controls.Add(buttonAddKey);
        Controls.Add(buttonDeleteKey);
        Controls.Add(listView1);
        Controls.Add(label5);
        Controls.Add(label4);
        Controls.Add(textBox2);
        Controls.Add(textBox1);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(buttonLoadDriver);
        Controls.Add(buttonSwitchEnable);
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        MaximizeBox = false;
        Text = "岚尘按键（内部测试版）";
        Load += AshForm_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.CheckBox preciseDelay;

    private System.Windows.Forms.Button buttonAddKey;

    private System.Windows.Forms.ColumnHeader 按下的键;
    private System.Windows.Forms.ColumnHeader 连发的键;

    private System.Windows.Forms.ListView listView1;

    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.TextBox textBox2;

    private System.Windows.Forms.TextBox textBox1;

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.Button buttonSwitchEnable;

    private System.Windows.Forms.Button buttonLoadDriver;

    #endregion

    private System.Windows.Forms.ColumnHeader 连发模式;
    private System.Windows.Forms.Button buttonDeleteKey;
}