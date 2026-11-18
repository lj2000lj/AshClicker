using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AshClicker
{
    partial class KeySelectionFrame
    {
        private IContainer components;
        private Label triggerLabel;
        private Button triggerHelpButton;
        private TextBox triggerKeyTextBox;
        private Label modeLabel;
        private ComboBox modeComboBox;
        private Label sequenceLabel;
        private Button sequenceHelpButton;
        private TextBox stepKeyTextBox;
        private ListView stepsListView;
        private Panel stepEditorPanel;
        private Label selectedStepLabel;
        private Button stepHelpButton;
        private Label repeatLabel;
        private NumericUpDown repeatNumeric;
        private Label holdLabel;
        private NumericUpDown holdNumeric;
        private Label delayLabel;
        private NumericUpDown delayNumeric;
        private Button removeStepButton;
        private Button moveStepUpButton;
        private Button moveStepDownButton;
        private Button cancelButton;
        private Button saveButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.triggerLabel = new Label();
            this.triggerHelpButton = new Button();
            this.triggerKeyTextBox = new TextBox();
            this.modeLabel = new Label();
            this.modeComboBox = new ComboBox();
            this.sequenceLabel = new Label();
            this.sequenceHelpButton = new Button();
            this.stepKeyTextBox = new TextBox();
            this.stepsListView = new ListView();
            this.stepEditorPanel = new Panel();
            this.selectedStepLabel = new Label();
            this.stepHelpButton = new Button();
            this.repeatLabel = new Label();
            this.repeatNumeric = new NumericUpDown();
            this.holdLabel = new Label();
            this.holdNumeric = new NumericUpDown();
            this.delayLabel = new Label();
            this.delayNumeric = new NumericUpDown();
            this.removeStepButton = new Button();
            this.moveStepUpButton = new Button();
            this.moveStepDownButton = new Button();
            this.cancelButton = new Button();
            this.saveButton = new Button();
            this.stepEditorPanel.SuspendLayout();
            ((ISupportInitialize)this.repeatNumeric).BeginInit();
            ((ISupportInitialize)this.holdNumeric).BeginInit();
            ((ISupportInitialize)this.delayNumeric).BeginInit();
            this.SuspendLayout();

            this.triggerLabel.AutoSize = true;
            this.triggerLabel.Location = new Point(24, 22);
            this.triggerLabel.Text = "触发按键";
            this.triggerHelpButton.Location = new Point(104, 15);
            this.triggerHelpButton.Size = new Size(34, 34);
            this.triggerHelpButton.TabIndex = 1;

            this.triggerKeyTextBox.BackColor = Color.White;
            this.triggerKeyTextBox.BorderStyle = BorderStyle.FixedSingle;
            this.triggerKeyTextBox.ImeMode = ImeMode.Disable;
            this.triggerKeyTextBox.Location = new Point(24, 53);
            this.triggerKeyTextBox.ReadOnly = true;
            this.triggerKeyTextBox.Size = new Size(454, 27);
            this.triggerKeyTextBox.TabIndex = 0;
            this.triggerKeyTextBox.Text = "—";
            this.triggerKeyTextBox.TextAlign = HorizontalAlignment.Center;
            this.triggerKeyTextBox.KeyDown += this.TriggerKeyTextBox_KeyDown;

            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new Point(498, 22);
            this.modeLabel.Text = "触发方式";
            this.modeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.modeComboBox.Items.AddRange(new object[] { "按住循环", "按键切换" });
            this.modeComboBox.Location = new Point(498, 52);
            this.modeComboBox.Size = new Size(300, 28);
            this.modeComboBox.TabIndex = 2;

            this.sequenceLabel.AutoSize = true;
            this.sequenceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.sequenceLabel.Location = new Point(24, 111);
            this.sequenceLabel.Text = "执行序列";
            this.sequenceHelpButton.Location = new Point(104, 107);
            this.sequenceHelpButton.Size = new Size(34, 34);
            this.sequenceHelpButton.TabIndex = 4;

            this.stepKeyTextBox.BackColor = Color.White;
            this.stepKeyTextBox.BorderStyle = BorderStyle.FixedSingle;
            this.stepKeyTextBox.ImeMode = ImeMode.Disable;
            this.stepKeyTextBox.Location = new Point(24, 148);
            this.stepKeyTextBox.ReadOnly = true;
            this.stepKeyTextBox.Size = new Size(454, 27);
            this.stepKeyTextBox.TabIndex = 3;
            this.stepKeyTextBox.Text = "—";
            this.stepKeyTextBox.TextAlign = HorizontalAlignment.Center;
            this.stepKeyTextBox.KeyDown += this.StepKeyTextBox_KeyDown;

            this.stepsListView.BorderStyle = BorderStyle.FixedSingle;
            this.stepsListView.Columns.AddRange(new[]
            {
                new ColumnHeader { Text = "#", Width = 38 },
                new ColumnHeader { Text = "按键", Width = 100 },
                new ColumnHeader { Text = "次数", Width = 60 },
                new ColumnHeader { Text = "按住 (ms)", Width = 114 },
                new ColumnHeader { Text = "等待 (ms)", Width = 118 }
            });
            this.stepsListView.FullRowSelect = true;
            this.stepsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.stepsListView.HideSelection = false;
            this.stepsListView.Location = new Point(24, 190);
            this.stepsListView.MultiSelect = false;
            this.stepsListView.Size = new Size(454, 236);
            this.stepsListView.TabIndex = 5;
            this.stepsListView.UseCompatibleStateImageBehavior = false;
            this.stepsListView.View = View.Details;
            this.stepsListView.SelectedIndexChanged += this.StepsListView_SelectedIndexChanged;

            this.stepEditorPanel.BackColor = Color.White;
            this.stepEditorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.stepEditorPanel.Location = new Point(498, 112);
            this.stepEditorPanel.Size = new Size(300, 314);
            this.stepEditorPanel.TabIndex = 6;
            this.stepEditorPanel.Controls.AddRange(new Control[]
            {
                this.selectedStepLabel, this.stepHelpButton,
                this.repeatLabel, this.repeatNumeric,
                this.holdLabel, this.holdNumeric,
                this.delayLabel, this.delayNumeric,
                this.moveStepUpButton, this.moveStepDownButton, this.removeStepButton
            });

            this.selectedStepLabel.AutoEllipsis = true;
            this.selectedStepLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.selectedStepLabel.Location = new Point(17, 17);
            this.selectedStepLabel.Size = new Size(223, 28);
            this.selectedStepLabel.Text = "步骤设置";
            this.stepHelpButton.Location = new Point(247, 12);
            this.stepHelpButton.Size = new Size(34, 34);
            this.stepHelpButton.TabIndex = 6;

            this.repeatLabel.AutoSize = true;
            this.repeatLabel.Location = new Point(18, 75);
            this.repeatLabel.Text = "重复次数";
            this.repeatNumeric.Location = new Point(175, 72);
            this.repeatNumeric.Minimum = 1;
            this.repeatNumeric.Maximum = 9999;
            this.repeatNumeric.Value = 1;
            this.repeatNumeric.Size = new Size(106, 27);
            this.repeatNumeric.TabIndex = 0;
            this.repeatNumeric.TextAlign = HorizontalAlignment.Right;
            this.repeatNumeric.ValueChanged += this.StepSetting_ValueChanged;

            this.holdLabel.AutoSize = true;
            this.holdLabel.Location = new Point(18, 120);
            this.holdLabel.Text = "按住时间 (ms)";
            this.holdNumeric.Location = new Point(175, 117);
            this.holdNumeric.Maximum = 600000;
            this.holdNumeric.Size = new Size(106, 27);
            this.holdNumeric.TabIndex = 1;
            this.holdNumeric.TextAlign = HorizontalAlignment.Right;
            this.holdNumeric.ValueChanged += this.StepSetting_ValueChanged;

            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new Point(18, 165);
            this.delayLabel.Text = "步骤后等待 (ms)";
            this.delayNumeric.Location = new Point(175, 162);
            this.delayNumeric.Maximum = 600000;
            this.delayNumeric.Size = new Size(106, 27);
            this.delayNumeric.TabIndex = 2;
            this.delayNumeric.TextAlign = HorizontalAlignment.Right;
            this.delayNumeric.ValueChanged += this.StepSetting_ValueChanged;

            this.moveStepUpButton.Location = new Point(18, 232);
            this.moveStepUpButton.Size = new Size(40, 38);
            this.moveStepUpButton.TabIndex = 3;
            this.moveStepUpButton.Click += this.MoveStepUpButton_Click;
            this.moveStepDownButton.Location = new Point(66, 232);
            this.moveStepDownButton.Size = new Size(40, 38);
            this.moveStepDownButton.TabIndex = 4;
            this.moveStepDownButton.Click += this.MoveStepDownButton_Click;
            this.removeStepButton.Location = new Point(114, 232);
            this.removeStepButton.Size = new Size(40, 38);
            this.removeStepButton.TabIndex = 5;
            this.removeStepButton.Click += this.RemoveStepButton_Click;

            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = new Point(700, 448);
            this.cancelButton.Size = new Size(44, 42);
            this.cancelButton.TabIndex = 7;

            this.saveButton.BackColor = Color.FromArgb(77, 98, 203);
            this.saveButton.FlatAppearance.BorderSize = 0;
            this.saveButton.FlatStyle = FlatStyle.Flat;
            this.saveButton.ForeColor = Color.White;
            this.saveButton.Location = new Point(754, 448);
            this.saveButton.Size = new Size(44, 42);
            this.saveButton.TabIndex = 8;
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += this.SaveButton_Click;

            this.AcceptButton = this.saveButton;
            this.CancelButton = this.cancelButton;
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.ForeColor = Color.FromArgb(39, 45, 62);
            this.ClientSize = new Size(822, 514);
            this.Controls.AddRange(new Control[]
            {
                this.triggerLabel, this.triggerHelpButton, this.triggerKeyTextBox,
                this.modeLabel, this.modeComboBox, this.sequenceLabel, this.sequenceHelpButton,
                this.stepKeyTextBox, this.stepsListView, this.stepEditorPanel,
                this.cancelButton, this.saveButton
            });
            this.Font = new Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = Properties.Resources.ashlogo;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "新建按键序列";
            this.Load += this.KeySelectionFrame_Load;
            ConfigureIcons();
            this.stepEditorPanel.ResumeLayout(false);
            this.stepEditorPanel.PerformLayout();
            ((ISupportInitialize)this.repeatNumeric).EndInit();
            ((ISupportInitialize)this.holdNumeric).EndInit();
            ((ISupportInitialize)this.delayNumeric).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
