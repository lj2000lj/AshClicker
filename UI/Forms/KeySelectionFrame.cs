using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AshClicker
{
    public partial class KeySelectionFrame : Form
    {
        private readonly List<AshSequenceStep> _steps = new List<AshSequenceStep>();
        private IconToolTips _iconToolTips;
        private AshKey _triggerKey;
        private int _selectedStepIndex = -1;
        private bool _renderingSteps;
        private bool _updatingStepEditor;

        public AshKeyMapping Mapping { get; private set; }

        public KeySelectionFrame(AshKeyMapping mapping = null)
        {
            InitializeComponent();
            if (mapping != null) LoadMapping(mapping);
        }

        private void ConfigureIcons()
        {
            _iconToolTips = new IconToolTips(components);
            const string triggerTip = "点一下输入框，然后按下要用于触发的键。";
            const string sequenceTip = "在列表上方的输入框按键，按下的键会依次加入序列。";
            _iconToolTips.SetHelp(triggerHelpButton, "触发键说明", triggerTip);
            _iconToolTips.SetHelp(sequenceHelpButton, "执行序列说明", sequenceTip);
            _iconToolTips.SetHelp(stepHelpButton, "步骤设置说明",
                "选中列表中的步骤即可调整。按住时间填 0 时，使用主窗口的按住时间。");
            _iconToolTips.Describe(triggerKeyTextBox, "触发按键", triggerTip);
            _iconToolTips.Describe(stepKeyTextBox, "录制序列步骤", sequenceTip);
            _iconToolTips.Describe(modeComboBox, "触发方式",
                "按住循环在松手时停止。按键切换按一次开始，再按一次停止。");
            _iconToolTips.Describe(stepsListView, "执行序列");
            _iconToolTips.Describe(repeatNumeric, "重复次数");
            _iconToolTips.Describe(holdNumeric, "按住时间（毫秒）", "填 0 使用主窗口的按住时间。");
            _iconToolTips.Describe(delayNumeric, "步骤后等待（毫秒）");
            _iconToolTips.SetIcon(moveStepUpButton, "\uE74A", "↑", "上移所选步骤");
            _iconToolTips.SetIcon(moveStepDownButton, "\uE74B", "↓", "下移所选步骤");
            _iconToolTips.SetIcon(removeStepButton, "\uE74D", "×", "删除所选步骤");
            _iconToolTips.SetIcon(cancelButton, "\uE711", "×", "取消");
            _iconToolTips.SetIcon(saveButton, "\uE74E", "✓", "保存序列");
        }

        private void KeySelectionFrame_Load(object sender, EventArgs e)
        {
            if (modeComboBox.SelectedIndex < 0) modeComboBox.SelectedIndex = 0;
            RenderSteps();
            triggerKeyTextBox.Select();
        }

        private void TriggerKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var key = AshKeyMap.Get(e.KeyValue);
            if (key.VKey != 0xFFFF)
            {
                _triggerKey = key;
                triggerKeyTextBox.Text = key.Description;
                UpdateStepEditor();
                stepKeyTextBox.Select();
            }

            e.SuppressKeyPress = true;
        }

        private void StepKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var key = AshKeyMap.Get(e.KeyValue);
            if (key.VKey != 0xFFFF)
            {
                _steps.Add(new AshSequenceStep { KeyCode = key.VKey, RepeatCount = 1 });
                _selectedStepIndex = _steps.Count - 1;
                RenderSteps();
                stepKeyTextBox.Text = key.Description;
            }

            e.SuppressKeyPress = true;
        }

        private void StepSetting_ValueChanged(object sender, EventArgs e)
        {
            if (_updatingStepEditor || _selectedStepIndex < 0) return;

            var step = _steps[_selectedStepIndex];
            step.RepeatCount = (int)repeatNumeric.Value;
            step.HoldMilliseconds = (int)holdNumeric.Value;
            step.DelayAfterMilliseconds = (int)delayNumeric.Value;
            var row = stepsListView.Items[_selectedStepIndex];
            row.SubItems[2].Text = step.RepeatCount.ToString();
            row.SubItems[3].Text = FormatHoldTime(step.HoldMilliseconds);
            row.SubItems[4].Text = step.DelayAfterMilliseconds.ToString();
        }

        private void RemoveStepButton_Click(object sender, EventArgs e)
        {
            if (_selectedStepIndex < 0) return;

            _steps.RemoveAt(_selectedStepIndex);
            _selectedStepIndex = Math.Min(_selectedStepIndex, _steps.Count - 1);
            RenderSteps();
        }

        private void MoveStepUpButton_Click(object sender, EventArgs e)
        {
            MoveSelectedStep(-1);
        }

        private void MoveStepDownButton_Click(object sender, EventArgs e)
        {
            MoveSelectedStep(1);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_triggerKey == null)
            {
                MessageBox.Show("请先按下一个触发键。", "缺少触发键", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                triggerKeyTextBox.Select();
                return;
            }

            if (_steps.Count == 0)
            {
                MessageBox.Show("请在执行序列上方的输入框按下至少一个按键。", "序列为空", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                stepKeyTextBox.Select();
                return;
            }

            Mapping = new AshKeyMapping
            {
                TriggerKeyCode = _triggerKey.VKey,
                TriggerMode = (AshTriggerMode)modeComboBox.SelectedIndex,
                Steps = _steps.Select(step => step.Clone()).ToList()
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadMapping(AshKeyMapping mapping)
        {
            _triggerKey = AshKeyMap.Get(mapping.TriggerKeyCode);
            triggerKeyTextBox.Text = _triggerKey.Description;
            modeComboBox.SelectedIndex = (int)mapping.TriggerMode;
            _steps.AddRange(mapping.Steps.Select(step => step.Clone()));
            _selectedStepIndex = _steps.Count > 0 ? 0 : -1;
            Text = "编辑按键序列";
            _iconToolTips.Describe(saveButton, "保存修改");
        }

        private void StepsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_renderingSteps) return;

            _selectedStepIndex = stepsListView.SelectedIndices.Count == 0
                ? -1
                : stepsListView.SelectedIndices[0];
            UpdateStepEditor();
        }

        private void MoveSelectedStep(int direction)
        {
            if (_selectedStepIndex < 0) return;

            var targetIndex = _selectedStepIndex + direction;
            if (targetIndex < 0 || targetIndex >= _steps.Count) return;

            var step = _steps[_selectedStepIndex];
            _steps.RemoveAt(_selectedStepIndex);
            _steps.Insert(targetIndex, step);
            _selectedStepIndex = targetIndex;
            RenderSteps();
        }

        private void RenderSteps()
        {
            _renderingSteps = true;
            stepsListView.BeginUpdate();
            stepsListView.Items.Clear();
            for (var index = 0; index < _steps.Count; index++)
            {
                var step = _steps[index];
                stepsListView.Items.Add(new ListViewItem(new[]
                {
                    (index + 1).ToString(),
                    AshKeyMap.Get(step.KeyCode).Description,
                    step.RepeatCount.ToString(),
                    FormatHoldTime(step.HoldMilliseconds),
                    step.DelayAfterMilliseconds.ToString()
                }));
            }

            if (_selectedStepIndex >= 0)
            {
                var selected = stepsListView.Items[_selectedStepIndex];
                selected.Selected = true;
                selected.Focused = true;
                selected.EnsureVisible();
            }

            stepsListView.EndUpdate();
            _renderingSteps = false;
            UpdateStepEditor();
        }

        private static string FormatHoldTime(int milliseconds)
        {
            return milliseconds == 0 ? "全局" : milliseconds.ToString();
        }

        private void UpdateStepEditor()
        {
            var hasSelection = _selectedStepIndex >= 0 && _selectedStepIndex < _steps.Count;
            repeatNumeric.Enabled = hasSelection;
            holdNumeric.Enabled = hasSelection;
            delayNumeric.Enabled = hasSelection;
            removeStepButton.Enabled = hasSelection;
            moveStepUpButton.Enabled = hasSelection && _selectedStepIndex > 0;
            moveStepDownButton.Enabled = hasSelection && _selectedStepIndex < _steps.Count - 1;
            saveButton.Enabled = _triggerKey != null && _steps.Count > 0;
            selectedStepLabel.Text = hasSelection
                ? "步骤 " + (_selectedStepIndex + 1) + " · " + AshKeyMap.Get(_steps[_selectedStepIndex].KeyCode).Description
                : "步骤设置";

            if (!hasSelection) return;

            _updatingStepEditor = true;
            var step = _steps[_selectedStepIndex];
            repeatNumeric.Value = Math.Min(repeatNumeric.Maximum, Math.Max(repeatNumeric.Minimum, step.RepeatCount));
            holdNumeric.Value = Math.Min(holdNumeric.Maximum, Math.Max(holdNumeric.Minimum, step.HoldMilliseconds));
            delayNumeric.Value = Math.Min(delayNumeric.Maximum, Math.Max(delayNumeric.Minimum, step.DelayAfterMilliseconds));
            _updatingStepEditor = false;
        }
    }
}
