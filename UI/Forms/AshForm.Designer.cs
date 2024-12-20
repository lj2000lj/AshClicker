using System.ComponentModel;

namespace AshClicker
{

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
            this.buttonLoadDriver = new System.Windows.Forms.Button();
            this.buttonSwitchEnable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.按下的键 = new System.Windows.Forms.ColumnHeader();
            this.连发的键 = new System.Windows.Forms.ColumnHeader();
            this.连发模式 = new System.Windows.Forms.ColumnHeader();
            this.buttonDeleteKey = new System.Windows.Forms.Button();
            this.buttonAddKey = new System.Windows.Forms.Button();
            this.preciseDelay = new System.Windows.Forms.CheckBox();
            this.buttonDriverless = new System.Windows.Forms.Button();
            this.buttonInstallInterception = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLoadDriver
            // 
            this.buttonLoadDriver.Location = new System.Drawing.Point(235, 117);
            this.buttonLoadDriver.Name = "buttonLoadDriver";
            this.buttonLoadDriver.Size = new System.Drawing.Size(154, 34);
            this.buttonLoadDriver.TabIndex = 0;
            this.buttonLoadDriver.Text = "加载 DD 驱动";
            this.buttonLoadDriver.UseVisualStyleBackColor = true;
            this.buttonLoadDriver.Visible = false;
            this.buttonLoadDriver.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSwitchEnable
            // 
            this.buttonSwitchEnable.Location = new System.Drawing.Point(235, 186);
            this.buttonSwitchEnable.Name = "buttonSwitchEnable";
            this.buttonSwitchEnable.Size = new System.Drawing.Size(154, 32);
            this.buttonSwitchEnable.TabIndex = 1;
            this.buttonSwitchEnable.Text = "按键开启";
            this.buttonSwitchEnable.UseVisualStyleBackColor = true;
            this.buttonSwitchEnable.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 105);
            this.label1.TabIndex = 2;
            this.label1.Text = "真不联网的公告：\r\n  - 当前版本 1.1.1\r\n  - 能用就行，有问题请前来 GitHub 提 Issue";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(372, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "当前状态：";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(98, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "关闭";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 136);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(154, 25);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "15";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(19, 191);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(154, 25);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "1";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 22);
            this.label4.TabIndex = 7;
            this.label4.Text = "连发间隔（毫秒）";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 22);
            this.label5.TabIndex = 8;
            this.label5.Text = "抬起间隔（毫秒）";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.按下的键, this.连发的键, this.连发模式 });
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(19, 224);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(372, 171);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // 按下的键
            // 
            this.按下的键.Name = "按下的键";
            this.按下的键.Text = "按下的键";
            this.按下的键.Width = 104;
            // 
            // 连发的键
            // 
            this.连发的键.Name = "连发的键";
            this.连发的键.Text = "连发的键";
            this.连发的键.Width = 100;
            // 
            // 连发模式
            // 
            this.连发模式.Name = "连发模式";
            this.连发模式.Text = "连发模式";
            this.连发模式.Width = 160;
            // 
            // buttonDeleteKey
            // 
            this.buttonDeleteKey.Location = new System.Drawing.Point(237, 401);
            this.buttonDeleteKey.Name = "buttonDeleteKey";
            this.buttonDeleteKey.Size = new System.Drawing.Size(154, 53);
            this.buttonDeleteKey.TabIndex = 10;
            this.buttonDeleteKey.Text = "删除键位";
            this.buttonDeleteKey.UseVisualStyleBackColor = true;
            this.buttonDeleteKey.Click += new System.EventHandler(this.buttonDeleteKey_Click);
            // 
            // buttonAddKey
            // 
            this.buttonAddKey.Location = new System.Drawing.Point(19, 401);
            this.buttonAddKey.Name = "buttonAddKey";
            this.buttonAddKey.Size = new System.Drawing.Size(154, 53);
            this.buttonAddKey.TabIndex = 11;
            this.buttonAddKey.Text = "添加键位";
            this.buttonAddKey.UseVisualStyleBackColor = true;
            this.buttonAddKey.Click += new System.EventHandler(this.buttonAddKey_Click);
            // 
            // preciseDelay
            // 
            this.preciseDelay.Checked = true;
            this.preciseDelay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.preciseDelay.Location = new System.Drawing.Point(237, 95);
            this.preciseDelay.Name = "preciseDelay";
            this.preciseDelay.Size = new System.Drawing.Size(104, 24);
            this.preciseDelay.TabIndex = 12;
            this.preciseDelay.Text = "精确延迟";
            this.preciseDelay.UseVisualStyleBackColor = true;
            // 
            // buttonDriverless
            // 
            this.buttonDriverless.Location = new System.Drawing.Point(235, 151);
            this.buttonDriverless.Name = "buttonDriverless";
            this.buttonDriverless.Size = new System.Drawing.Size(154, 34);
            this.buttonDriverless.TabIndex = 13;
            this.buttonDriverless.Text = "无驱动模式";
            this.buttonDriverless.UseVisualStyleBackColor = true;
            this.buttonDriverless.Visible = false;
            this.buttonDriverless.Click += new System.EventHandler(this.buttonDriverless_Click);
            // 
            // buttonInstallInterception
            // 
            this.buttonInstallInterception.Location = new System.Drawing.Point(235, 117);
            this.buttonInstallInterception.Name = "buttonInstallInterception";
            this.buttonInstallInterception.Size = new System.Drawing.Size(154, 34);
            this.buttonInstallInterception.TabIndex = 14;
            this.buttonInstallInterception.Text = "安装驱动";
            this.buttonInstallInterception.UseVisualStyleBackColor = true;
            this.buttonInstallInterception.Click += new System.EventHandler(this.buttonInstallInterception_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(235, 151);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 34);
            this.button1.TabIndex = 15;
            this.button1.Text = "卸载驱动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // AshForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 466);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonInstallInterception);
            this.Controls.Add(this.preciseDelay);
            this.Controls.Add(this.buttonAddKey);
            this.Controls.Add(this.buttonDeleteKey);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSwitchEnable);
            this.Controls.Add(this.buttonDriverless);
            this.Controls.Add(this.buttonLoadDriver);
            this.Icon = global::AshClicker.Properties.Resources.ashlogo;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "AshForm";
            this.Text = "岚尘按键 | 能用就行";
            this.Load += new System.EventHandler(this.AshForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Button buttonInstallInterception;

        private System.Windows.Forms.Button buttonDriverless;

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
}