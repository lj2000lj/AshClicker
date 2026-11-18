using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AshClicker
{
    partial class AshForm
    {
        private IContainer components;
        private Panel sidebarPanel;
        private Label appTitleLabel;
        private Label versionLabel;
        private Label statusDotLabel;
        private Label statusValueLabel;
        private Button toggleButton;
        private Label driverSectionLabel;
        private Button installDriverButton;
        private Button uninstallDriverButton;
        private Button loadDdDriverButton;
        private Button useInterceptionButton;
        private Button githubButton;
        private Button voiceLicenseButton;
        private Panel contentPanel;
        private GroupBox timingGroupBox;
        private Button timingHelpButton;
        private Label pressDelayLabel;
        private Label pressDelayUnitLabel;
        private NumericUpDown pressDelayNumeric;
        private Label releaseDelayLabel;
        private Label releaseDelayUnitLabel;
        private NumericUpDown releaseDelayNumeric;
        private CheckBox preciseDelayCheckBox;
        private CheckBox soundEffectCheckBox;
        private CheckBox highPerformanceCheckBox;
        private Label mappingsTitleLabel;
        private Label mappingCountLabel;
        private ListView mappingListView;
        private ColumnHeader triggerKeyColumn;
        private ColumnHeader outputKeyColumn;
        private ColumnHeader modeColumn;
        private Button addMappingButton;
        private Button editMappingButton;
        private Button deleteMappingButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.sidebarPanel = new Panel();
            this.voiceLicenseButton = new Button();
            this.githubButton = new Button();
            this.useInterceptionButton = new Button();
            this.loadDdDriverButton = new Button();
            this.uninstallDriverButton = new Button();
            this.installDriverButton = new Button();
            this.driverSectionLabel = new Label();
            this.toggleButton = new Button();
            this.statusValueLabel = new Label();
            this.statusDotLabel = new Label();
            this.versionLabel = new Label();
            this.appTitleLabel = new Label();
            this.contentPanel = new Panel();
            this.deleteMappingButton = new Button();
            this.editMappingButton = new Button();
            this.addMappingButton = new Button();
            this.mappingListView = new ListView();
            this.triggerKeyColumn = new ColumnHeader();
            this.outputKeyColumn = new ColumnHeader();
            this.modeColumn = new ColumnHeader();
            this.mappingCountLabel = new Label();
            this.mappingsTitleLabel = new Label();
            this.timingGroupBox = new GroupBox();
            this.timingHelpButton = new Button();
            this.soundEffectCheckBox = new CheckBox();
            this.preciseDelayCheckBox = new CheckBox();
            this.highPerformanceCheckBox = new CheckBox();
            this.releaseDelayUnitLabel = new Label();
            this.releaseDelayNumeric = new NumericUpDown();
            this.releaseDelayLabel = new Label();
            this.pressDelayUnitLabel = new Label();
            this.pressDelayNumeric = new NumericUpDown();
            this.pressDelayLabel = new Label();
            this.sidebarPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.timingGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // sidebarPanel
            //
            this.sidebarPanel.BackColor = Color.FromArgb(34, 39, 54);
            this.sidebarPanel.Controls.Add(this.voiceLicenseButton);
            this.sidebarPanel.Controls.Add(this.githubButton);
            this.sidebarPanel.Controls.Add(this.useInterceptionButton);
            this.sidebarPanel.Controls.Add(this.loadDdDriverButton);
            this.sidebarPanel.Controls.Add(this.uninstallDriverButton);
            this.sidebarPanel.Controls.Add(this.installDriverButton);
            this.sidebarPanel.Controls.Add(this.driverSectionLabel);
            this.sidebarPanel.Controls.Add(this.toggleButton);
            this.sidebarPanel.Controls.Add(this.statusValueLabel);
            this.sidebarPanel.Controls.Add(this.statusDotLabel);
            this.sidebarPanel.Controls.Add(this.versionLabel);
            this.sidebarPanel.Controls.Add(this.appTitleLabel);
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.Location = new Point(0, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new Size(190, 522);
            this.sidebarPanel.TabIndex = 0;
            //
            // appTitleLabel
            //
            this.appTitleLabel.AutoSize = true;
            this.appTitleLabel.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.appTitleLabel.ForeColor = Color.White;
            this.appTitleLabel.Location = new Point(22, 25);
            this.appTitleLabel.Name = "appTitleLabel";
            this.appTitleLabel.Size = new Size(148, 42);
            this.appTitleLabel.TabIndex = 0;
            this.appTitleLabel.Text = "岚尘按键";
            //
            // versionLabel
            //
            this.versionLabel.AutoSize = true;
            this.versionLabel.ForeColor = Color.FromArgb(153, 163, 184);
            this.versionLabel.Location = new Point(24, 480);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new Size(75, 20);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "dev";
            //
            // statusDotLabel
            //
            this.statusDotLabel.AutoSize = true;
            this.statusDotLabel.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.statusDotLabel.ForeColor = Color.FromArgb(142, 151, 172);
            this.statusDotLabel.Location = new Point(21, 81);
            this.statusDotLabel.Name = "statusDotLabel";
            this.statusDotLabel.Size = new Size(21, 26);
            this.statusDotLabel.TabIndex = 4;
            this.statusDotLabel.Text = "●";
            //
            // statusValueLabel
            //
            this.statusValueLabel.AutoSize = true;
            this.statusValueLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.statusValueLabel.ForeColor = Color.White;
            this.statusValueLabel.Location = new Point(45, 81);
            this.statusValueLabel.Name = "statusValueLabel";
            this.statusValueLabel.Size = new Size(61, 24);
            this.statusValueLabel.TabIndex = 5;
            this.statusValueLabel.Text = "已停止";
            //
            // toggleButton
            //
            this.toggleButton.BackColor = Color.FromArgb(77, 98, 203);
            this.toggleButton.Cursor = Cursors.Hand;
            this.toggleButton.FlatAppearance.BorderSize = 0;
            this.toggleButton.FlatStyle = FlatStyle.Flat;
            this.toggleButton.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.toggleButton.ForeColor = Color.White;
            this.toggleButton.Location = new Point(24, 116);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new Size(142, 44);
            this.toggleButton.TabIndex = 0;
            this.toggleButton.UseVisualStyleBackColor = false;
            this.toggleButton.Click += new System.EventHandler(this.ToggleButton_Click);
            //
            // driverSectionLabel
            //
            this.driverSectionLabel.AutoSize = true;
            this.driverSectionLabel.ForeColor = Color.FromArgb(153, 163, 184);
            this.driverSectionLabel.Location = new Point(24, 182);
            this.driverSectionLabel.Name = "driverSectionLabel";
            this.driverSectionLabel.Size = new Size(73, 20);
            this.driverSectionLabel.TabIndex = 7;
            this.driverSectionLabel.Text = "驱动管理";
            //
            // installDriverButton
            //
            this.installDriverButton.BackColor = Color.FromArgb(49, 56, 75);
            this.installDriverButton.Cursor = Cursors.Hand;
            this.installDriverButton.FlatAppearance.BorderSize = 0;
            this.installDriverButton.FlatStyle = FlatStyle.Flat;
            this.installDriverButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.installDriverButton.Location = new Point(24, 212);
            this.installDriverButton.Name = "installDriverButton";
            this.installDriverButton.Size = new Size(40, 38);
            this.installDriverButton.TabIndex = 1;
            this.installDriverButton.UseVisualStyleBackColor = false;
            this.installDriverButton.Click += new System.EventHandler(this.InstallDriverButton_Click);
            //
            // uninstallDriverButton
            //
            this.uninstallDriverButton.BackColor = Color.FromArgb(49, 56, 75);
            this.uninstallDriverButton.Cursor = Cursors.Hand;
            this.uninstallDriverButton.FlatAppearance.BorderSize = 0;
            this.uninstallDriverButton.FlatStyle = FlatStyle.Flat;
            this.uninstallDriverButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.uninstallDriverButton.Location = new Point(72, 212);
            this.uninstallDriverButton.Name = "uninstallDriverButton";
            this.uninstallDriverButton.Size = new Size(40, 38);
            this.uninstallDriverButton.TabIndex = 2;
            this.uninstallDriverButton.UseVisualStyleBackColor = false;
            this.uninstallDriverButton.Click += new System.EventHandler(this.UninstallDriverButton_Click);
            //
            // loadDdDriverButton
            //
            this.loadDdDriverButton.BackColor = Color.FromArgb(49, 56, 75);
            this.loadDdDriverButton.Cursor = Cursors.Hand;
            this.loadDdDriverButton.FlatAppearance.BorderSize = 0;
            this.loadDdDriverButton.FlatStyle = FlatStyle.Flat;
            this.loadDdDriverButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.loadDdDriverButton.Location = new Point(24, 258);
            this.loadDdDriverButton.Name = "loadDdDriverButton";
            this.loadDdDriverButton.Size = new Size(40, 38);
            this.loadDdDriverButton.TabIndex = 3;
            this.loadDdDriverButton.UseVisualStyleBackColor = false;
            this.loadDdDriverButton.Visible = false;
            this.loadDdDriverButton.Click += new System.EventHandler(this.LoadDdDriverButton_Click);
            //
            // useInterceptionButton
            //
            this.useInterceptionButton.BackColor = Color.FromArgb(49, 56, 75);
            this.useInterceptionButton.FlatAppearance.BorderSize = 0;
            this.useInterceptionButton.FlatStyle = FlatStyle.Flat;
            this.useInterceptionButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.useInterceptionButton.Location = new Point(72, 258);
            this.useInterceptionButton.Name = "useInterceptionButton";
            this.useInterceptionButton.Size = new Size(40, 38);
            this.useInterceptionButton.TabIndex = 4;
            this.useInterceptionButton.UseVisualStyleBackColor = false;
            this.useInterceptionButton.Visible = false;
            this.useInterceptionButton.Click += new System.EventHandler(this.UseInterceptionButton_Click);
            //
            // githubButton
            //
            this.githubButton.BackColor = Color.FromArgb(49, 56, 75);
            this.githubButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.githubButton.FlatAppearance.BorderSize = 0;
            this.githubButton.FlatStyle = FlatStyle.Flat;
            this.githubButton.Location = new Point(24, 428);
            this.githubButton.Name = "githubButton";
            this.githubButton.Size = new Size(40, 36);
            this.githubButton.TabIndex = 5;
            this.githubButton.UseVisualStyleBackColor = false;
            this.githubButton.Click += this.GithubButton_Click;
            //
            // voiceLicenseButton
            //
            this.voiceLicenseButton.BackColor = Color.FromArgb(49, 56, 75);
            this.voiceLicenseButton.ForeColor = Color.FromArgb(225, 229, 239);
            this.voiceLicenseButton.FlatAppearance.BorderSize = 0;
            this.voiceLicenseButton.FlatStyle = FlatStyle.Flat;
            this.voiceLicenseButton.Location = new Point(72, 428);
            this.voiceLicenseButton.Name = "voiceLicenseButton";
            this.voiceLicenseButton.Size = new Size(40, 36);
            this.voiceLicenseButton.TabIndex = 6;
            this.voiceLicenseButton.UseVisualStyleBackColor = false;
            this.voiceLicenseButton.Click += this.VoiceLicenseButton_Click;
            //
            // contentPanel
            //
            this.contentPanel.BackColor = Color.FromArgb(245, 247, 252);
            this.contentPanel.Controls.Add(this.deleteMappingButton);
            this.contentPanel.Controls.Add(this.editMappingButton);
            this.contentPanel.Controls.Add(this.addMappingButton);
            this.contentPanel.Controls.Add(this.mappingListView);
            this.contentPanel.Controls.Add(this.mappingCountLabel);
            this.contentPanel.Controls.Add(this.mappingsTitleLabel);
            this.contentPanel.Controls.Add(this.timingGroupBox);
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.Location = new Point(190, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new Size(662, 522);
            this.contentPanel.TabIndex = 1;
            //
            // timingGroupBox
            //
            this.timingGroupBox.Controls.Add(this.timingHelpButton);
            this.timingGroupBox.Controls.Add(this.soundEffectCheckBox);
            this.timingGroupBox.Controls.Add(this.preciseDelayCheckBox);
            this.timingGroupBox.Controls.Add(this.highPerformanceCheckBox);
            this.timingGroupBox.Controls.Add(this.releaseDelayUnitLabel);
            this.timingGroupBox.Controls.Add(this.releaseDelayNumeric);
            this.timingGroupBox.Controls.Add(this.releaseDelayLabel);
            this.timingGroupBox.Controls.Add(this.pressDelayUnitLabel);
            this.timingGroupBox.Controls.Add(this.pressDelayNumeric);
            this.timingGroupBox.Controls.Add(this.pressDelayLabel);
            this.timingGroupBox.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.timingGroupBox.ForeColor = Color.FromArgb(58, 65, 84);
            this.timingGroupBox.Location = new Point(24, 360);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new Size(614, 138);
            this.timingGroupBox.TabIndex = 4;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "节奏设置";
            //
            // timingHelpButton
            //
            this.timingHelpButton.Location = new Point(558, 78);
            this.timingHelpButton.Name = "timingHelpButton";
            this.timingHelpButton.Size = new Size(36, 38);
            this.timingHelpButton.TabIndex = 5;
            //
            // pressDelayLabel
            //
            this.pressDelayLabel.AutoSize = true;
            this.pressDelayLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.pressDelayLabel.Location = new Point(20, 37);
            this.pressDelayLabel.Name = "pressDelayLabel";
            this.pressDelayLabel.Size = new Size(73, 20);
            this.pressDelayLabel.TabIndex = 0;
            this.pressDelayLabel.Text = "连发间隔";
            //
            // pressDelayNumeric
            //
            this.pressDelayNumeric.BorderStyle = BorderStyle.FixedSingle;
            this.pressDelayNumeric.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.pressDelayNumeric.Location = new Point(103, 33);
            this.pressDelayNumeric.Name = "pressDelayNumeric";
            this.pressDelayNumeric.Size = new Size(84, 27);
            this.pressDelayNumeric.TabIndex = 0;
            this.pressDelayNumeric.Minimum = 1;
            this.pressDelayNumeric.Maximum = int.MaxValue;
            this.pressDelayNumeric.Value = 15;
            this.pressDelayNumeric.TextAlign = HorizontalAlignment.Right;
            this.pressDelayNumeric.ValueChanged += new System.EventHandler(this.PressDelayNumeric_ValueChanged);
            //
            // pressDelayUnitLabel
            //
            this.pressDelayUnitLabel.AutoSize = true;
            this.pressDelayUnitLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.pressDelayUnitLabel.ForeColor = Color.FromArgb(112, 121, 143);
            this.pressDelayUnitLabel.Location = new Point(194, 37);
            this.pressDelayUnitLabel.Name = "pressDelayUnitLabel";
            this.pressDelayUnitLabel.Size = new Size(39, 20);
            this.pressDelayUnitLabel.TabIndex = 2;
            this.pressDelayUnitLabel.Text = "毫秒";
            //
            // releaseDelayLabel
            //
            this.releaseDelayLabel.AutoSize = true;
            this.releaseDelayLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.releaseDelayLabel.Location = new Point(300, 37);
            this.releaseDelayLabel.Name = "releaseDelayLabel";
            this.releaseDelayLabel.Size = new Size(73, 20);
            this.releaseDelayLabel.TabIndex = 3;
            this.releaseDelayLabel.Text = "按住时间";
            //
            // releaseDelayNumeric
            //
            this.releaseDelayNumeric.BorderStyle = BorderStyle.FixedSingle;
            this.releaseDelayNumeric.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.releaseDelayNumeric.Location = new Point(383, 33);
            this.releaseDelayNumeric.Name = "releaseDelayNumeric";
            this.releaseDelayNumeric.Size = new Size(84, 27);
            this.releaseDelayNumeric.TabIndex = 1;
            this.releaseDelayNumeric.Minimum = 1;
            this.releaseDelayNumeric.Maximum = int.MaxValue;
            this.releaseDelayNumeric.Value = 1;
            this.releaseDelayNumeric.TextAlign = HorizontalAlignment.Right;
            this.releaseDelayNumeric.ValueChanged += new System.EventHandler(this.ReleaseDelayNumeric_ValueChanged);
            //
            // releaseDelayUnitLabel
            //
            this.releaseDelayUnitLabel.AutoSize = true;
            this.releaseDelayUnitLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.releaseDelayUnitLabel.ForeColor = Color.FromArgb(112, 121, 143);
            this.releaseDelayUnitLabel.Location = new Point(474, 37);
            this.releaseDelayUnitLabel.Name = "releaseDelayUnitLabel";
            this.releaseDelayUnitLabel.Size = new Size(39, 20);
            this.releaseDelayUnitLabel.TabIndex = 5;
            this.releaseDelayUnitLabel.Text = "毫秒";
            //
            // preciseDelayCheckBox
            //
            this.preciseDelayCheckBox.AutoSize = false;
            this.preciseDelayCheckBox.Appearance = Appearance.Button;
            this.preciseDelayCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            this.preciseDelayCheckBox.BackColor = Color.White;
            this.preciseDelayCheckBox.FlatStyle = FlatStyle.Flat;
            this.preciseDelayCheckBox.FlatAppearance.BorderColor = Color.FromArgb(207, 213, 226);
            this.preciseDelayCheckBox.FlatAppearance.CheckedBackColor = Color.FromArgb(219, 227, 255);
            this.preciseDelayCheckBox.UseVisualStyleBackColor = false;
            this.preciseDelayCheckBox.Checked = true;
            this.preciseDelayCheckBox.CheckState = CheckState.Checked;
            this.preciseDelayCheckBox.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.preciseDelayCheckBox.Location = new Point(20, 78);
            this.preciseDelayCheckBox.Name = "preciseDelayCheckBox";
            this.preciseDelayCheckBox.Size = new Size(40, 38);
            this.preciseDelayCheckBox.TabIndex = 2;
            this.preciseDelayCheckBox.UseVisualStyleBackColor = true;
            //
            // soundEffectCheckBox
            //
            this.soundEffectCheckBox.AutoSize = false;
            this.soundEffectCheckBox.Appearance = Appearance.Button;
            this.soundEffectCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            this.soundEffectCheckBox.BackColor = Color.White;
            this.soundEffectCheckBox.FlatStyle = FlatStyle.Flat;
            this.soundEffectCheckBox.FlatAppearance.BorderColor = Color.FromArgb(207, 213, 226);
            this.soundEffectCheckBox.FlatAppearance.CheckedBackColor = Color.FromArgb(219, 227, 255);
            this.soundEffectCheckBox.UseVisualStyleBackColor = false;
            this.soundEffectCheckBox.Checked = true;
            this.soundEffectCheckBox.CheckState = CheckState.Checked;
            this.soundEffectCheckBox.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.soundEffectCheckBox.Location = new Point(68, 78);
            this.soundEffectCheckBox.Name = "soundEffectCheckBox";
            this.soundEffectCheckBox.Size = new Size(40, 38);
            this.soundEffectCheckBox.TabIndex = 3;
            this.soundEffectCheckBox.UseVisualStyleBackColor = true;
            //
            // highPerformanceCheckBox
            //
            this.highPerformanceCheckBox.AutoSize = false;
            this.highPerformanceCheckBox.Appearance = Appearance.Button;
            this.highPerformanceCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            this.highPerformanceCheckBox.BackColor = Color.White;
            this.highPerformanceCheckBox.FlatStyle = FlatStyle.Flat;
            this.highPerformanceCheckBox.FlatAppearance.BorderColor = Color.FromArgb(207, 213, 226);
            this.highPerformanceCheckBox.FlatAppearance.CheckedBackColor = Color.FromArgb(219, 227, 255);
            this.highPerformanceCheckBox.UseVisualStyleBackColor = false;
            this.highPerformanceCheckBox.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.highPerformanceCheckBox.Location = new Point(116, 78);
            this.highPerformanceCheckBox.Name = "highPerformanceCheckBox";
            this.highPerformanceCheckBox.Size = new Size(40, 38);
            this.highPerformanceCheckBox.TabIndex = 4;
            this.highPerformanceCheckBox.UseVisualStyleBackColor = true;
            this.highPerformanceCheckBox.CheckedChanged += new System.EventHandler(this.HighPerformanceCheckBox_CheckedChanged);
            //
            // mappingsTitleLabel
            //
            this.mappingsTitleLabel.AutoSize = true;
            this.mappingsTitleLabel.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.mappingsTitleLabel.ForeColor = Color.FromArgb(39, 45, 62);
            this.mappingsTitleLabel.Location = new Point(24, 24);
            this.mappingsTitleLabel.Name = "mappingsTitleLabel";
            this.mappingsTitleLabel.Size = new Size(108, 27);
            this.mappingsTitleLabel.TabIndex = 3;
            this.mappingsTitleLabel.Text = "按键映射";
            //
            // mappingCountLabel
            //
            this.mappingCountLabel.ForeColor = Color.FromArgb(112, 121, 143);
            this.mappingCountLabel.Location = new Point(143, 31);
            this.mappingCountLabel.Name = "mappingCountLabel";
            this.mappingCountLabel.Size = new Size(110, 20);
            this.mappingCountLabel.TabIndex = 4;
            this.mappingCountLabel.Text = "0 条";
            this.mappingCountLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // mappingListView
            //
            this.mappingListView.BackColor = Color.White;
            this.mappingListView.BorderStyle = BorderStyle.FixedSingle;
            this.mappingListView.Columns.AddRange(new ColumnHeader[] {
                this.triggerKeyColumn,
                this.outputKeyColumn,
                this.modeColumn});
            this.mappingListView.FullRowSelect = true;
            this.mappingListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.mappingListView.HideSelection = false;
            this.mappingListView.Location = new Point(24, 72);
            this.mappingListView.MultiSelect = false;
            this.mappingListView.Name = "mappingListView";
            this.mappingListView.Size = new Size(614, 270);
            this.mappingListView.TabIndex = 3;
            this.mappingListView.UseCompatibleStateImageBehavior = false;
            this.mappingListView.View = View.Details;
            this.mappingListView.SelectedIndexChanged += this.MappingListView_SelectedIndexChanged;
            this.mappingListView.DoubleClick += new System.EventHandler(this.EditMappingButton_Click);
            //
            // triggerKeyColumn
            //
            this.triggerKeyColumn.Text = "触发按键";
            this.triggerKeyColumn.Width = 140;
            //
            // outputKeyColumn
            //
            this.outputKeyColumn.Text = "输出序列";
            this.outputKeyColumn.Width = 300;
            //
            // modeColumn
            //
            this.modeColumn.Text = "触发模式";
            this.modeColumn.Width = 152;
            //
            // addMappingButton
            //
            this.addMappingButton.BackColor = Color.FromArgb(77, 98, 203);
            this.addMappingButton.Cursor = Cursors.Hand;
            this.addMappingButton.FlatAppearance.BorderSize = 0;
            this.addMappingButton.FlatStyle = FlatStyle.Flat;
            this.addMappingButton.ForeColor = Color.White;
            this.addMappingButton.Location = new Point(502, 20);
            this.addMappingButton.Name = "addMappingButton";
            this.addMappingButton.Size = new Size(40, 38);
            this.addMappingButton.TabIndex = 0;
            this.addMappingButton.UseVisualStyleBackColor = false;
            this.addMappingButton.Click += new System.EventHandler(this.AddMappingButton_Click);
            //
            // editMappingButton
            //
            this.editMappingButton.BackColor = Color.White;
            this.editMappingButton.Cursor = Cursors.Hand;
            this.editMappingButton.FlatAppearance.BorderColor = Color.FromArgb(207, 213, 226);
            this.editMappingButton.FlatStyle = FlatStyle.Flat;
            this.editMappingButton.ForeColor = Color.FromArgb(78, 86, 106);
            this.editMappingButton.Location = new Point(550, 20);
            this.editMappingButton.Name = "editMappingButton";
            this.editMappingButton.Size = new Size(40, 38);
            this.editMappingButton.TabIndex = 1;
            this.editMappingButton.UseVisualStyleBackColor = false;
            this.editMappingButton.Click += new System.EventHandler(this.EditMappingButton_Click);
            //
            // deleteMappingButton
            //
            this.deleteMappingButton.BackColor = Color.White;
            this.deleteMappingButton.Cursor = Cursors.Hand;
            this.deleteMappingButton.FlatAppearance.BorderColor = Color.FromArgb(207, 213, 226);
            this.deleteMappingButton.FlatStyle = FlatStyle.Flat;
            this.deleteMappingButton.ForeColor = Color.FromArgb(78, 86, 106);
            this.deleteMappingButton.Location = new Point(598, 20);
            this.deleteMappingButton.Name = "deleteMappingButton";
            this.deleteMappingButton.Size = new Size(40, 38);
            this.deleteMappingButton.TabIndex = 2;
            this.deleteMappingButton.UseVisualStyleBackColor = false;
            this.deleteMappingButton.Click += new System.EventHandler(this.DeleteMappingButton_Click);
            //
            // AshForm
            //
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.ClientSize = new Size(852, 522);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = Properties.Resources.ashlogo;
            this.MaximizeBox = false;
            this.Name = "AshForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "岚尘按键";
            this.Load += new System.EventHandler(this.AshForm_Load);
            ConfigureIcons();
            this.sidebarPanel.ResumeLayout(false);
            this.sidebarPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.timingGroupBox.ResumeLayout(false);
            this.timingGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
