using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AshClicker.Driver;
using AshClicker.Properties;
using Microsoft.Win32;

namespace AshClicker
{
    public partial class AshForm : Form
    {
        private const int HotKeyIdBase = 12489;
        private const int WmHotKey = 0x0312;
        private const int DefaultPressDelay = 15;
        private const int DefaultReleaseDelay = 1;

        private readonly Dictionary<int, bool> _toggledKeys = new Dictionary<int, bool>();
        private List<AshRuntimeMapping> _runtimeMappings = new List<AshRuntimeMapping>();
        private DdControl _ddControl;
        private InterceptionControl _interceptionControl;
        private SoundPlayer _startSound;
        private SoundPlayer _stopSound;
        private IconToolTips _iconToolTips;
        private bool _isRunning;
        private bool _useInterception = true;

        public AshForm()
        {
            InitializeComponent();

            try
            {
                _interceptionControl = new InterceptionControl();
            }
            catch (Exception ex)
            {
                LogDriverLoadFailure(ex);
                toggleButton.Enabled = false;
                SetStatus(false, "驱动未就绪");
                MessageBox.Show("尚未检测到可用驱动，请先安装驱动并重启电脑。", "驱动未就绪",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigureIcons()
        {
            _iconToolTips = new IconToolTips(components);
            SetRunIcon(false);
            _iconToolTips.SetIcon(installDriverButton, "\uE896", "↓", "安装 Interception 驱动",
                "安装完成后重启 Windows。");
            _iconToolTips.SetIcon(uninstallDriverButton, "\uE898", "↑", "卸载 Interception 驱动",
                "卸载前会要求确认，重启后生效。");
            _iconToolTips.SetIcon(loadDdDriverButton, "\uE8B7", "▣", "加载 DD 驱动");
            _iconToolTips.SetIcon(useInterceptionButton, "\uE765", "⌨", "切换到 Interception");
            _iconToolTips.SetIcon(githubButton, "\uED15", "↗", "在 GitHub 查看项目或反馈问题");
            _iconToolTips.SetIcon(voiceLicenseButton, "\uE8A5", "▤", "查看 VOICEVOX 音源使用条款");
            _iconToolTips.SetIcon(addMappingButton, "\uE710", "+", "添加按键映射");
            _iconToolTips.SetIcon(editMappingButton, "\uE70F", "✎", "编辑所选映射");
            _iconToolTips.SetIcon(deleteMappingButton, "\uE74D", "×", "删除所选映射");
            _iconToolTips.SetIcon(preciseDelayCheckBox, "\uE916", "◷", "精确延迟",
                "提高计时精度，可能增加 CPU 占用。");
            _iconToolTips.SetIcon(soundEffectCheckBox, "\uE767", "♪", "音效提示",
                "切换按键映射时播放提示音。");
            _iconToolTips.SetIcon(highPerformanceCheckBox, "\uE945", "ϟ", "高性能发送（实验性）",
                "复用 Interception 的发送缓冲区。");
            _iconToolTips.Describe(pressDelayNumeric, "连发间隔（毫秒）", "控制两次调度之间的最短等待。");
            _iconToolTips.Describe(releaseDelayNumeric, "按住时间（毫秒）", "适用于未单独设置按住时间的步骤。");
            _iconToolTips.SetHelp(timingHelpButton, "节奏设置说明",
                "连发间隔控制调度频率。按住时间控制每次发键从按下到松开的时长。");
            UpdateMappingActions();
        }

        private void SetRunIcon(bool running)
        {
            if (running)
            {
                _iconToolTips.SetIcon(toggleButton, "\uE71A", "■", "停止连发");
            }
            else
            {
                _iconToolTips.SetIcon(toggleButton, "\uE768", "▶", "启动连发",
                    "启动后使用映射里的触发键。");
            }
        }

        private static string RunScCommand(string command)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new InvalidOperationException($"命令执行失败: {error}");
                }

                return output;
            }
        }

        private void AshForm_Load(object sender, EventArgs e)
        {
            _ddControl = new DdControl();

            using (var stream = new MemoryStream(Resources.start))
            {
                _startSound = new SoundPlayer(stream);
                _startSound.Load();
            }

            using (var stream = new MemoryStream(Resources.stop))
            {
                _stopSound = new SoundPlayer(stream);
                _stopSound.Load();
            }

            using (var stream = new MemoryStream(Resources.silence))
            {
                new SoundPlayer(stream).PlaySync();
            }

            AshConfigPool.Instance.LoadConfig();
            var pressDelay = AshConfigPool.Instance.PressDelay;
            var releaseDelay = AshConfigPool.Instance.ReleaseDelay;
            AshConfigPool.Instance.PressDelay = pressDelay > 0 ? pressDelay : DefaultPressDelay;
            AshConfigPool.Instance.ReleaseDelay = releaseDelay > 0 ? releaseDelay : DefaultReleaseDelay;
            pressDelayNumeric.Value = AshConfigPool.Instance.PressDelay;
            releaseDelayNumeric.Value = AshConfigPool.Instance.ReleaseDelay;
            highPerformanceCheckBox.Checked = AshConfigPool.Instance.UseHighPerformanceInput;
            UpdateVersionLabel();
            RefreshMappingList();
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        private void ProcessMappings()
        {
            if (!_isRunning) return;

            var now = Environment.TickCount & int.MaxValue;
            foreach (var mapping in _runtimeMappings)
            {
                var active = mapping.TriggerMode == AshTriggerMode.Hold
                    ? (GetAsyncKeyState(mapping.TriggerKeyCode) & 0x8000) != 0
                    : _toggledKeys.GetValueOrDefault(mapping.TriggerKeyCode, false);
                mapping.SetActive(active, now);

                if (!active || !HasReached(now, mapping.NextRunAt)) continue;

                var step = mapping.CurrentStep;
                SendKey(step.OutputCode, step.HoldMilliseconds);
                mapping.Advance(now + step.DelayAfterMilliseconds);
            }
        }

        private void SendKey(int outputCode, int releaseDelay)
        {
            if (_useInterception)
            {
                var stroke = new InterceptionControl.Stroke
                {
                    Key = new InterceptionControl.KeyStroke
                    {
                        Code = (ushort)outputCode,
                        State = 0x00
                    }
                };
                _interceptionControl.Send(1, ref stroke);
                AshStopwatch.PreciseDelay(releaseDelay);
                stroke.Key.State = 0x01;
                _interceptionControl.Send(1, ref stroke);
            }
            else
            {
                _ddControl.key(outputCode, 1);
                AshStopwatch.PreciseDelay(releaseDelay);
                _ddControl.key(outputCode, 2);
            }
        }

        private static bool HasReached(int now, int dueAt)
        {
            return unchecked(now - dueAt) >= 0;
        }

        private void UpdateVersionLabel()
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(version))
            {
                version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "dev";
            }

            var fullVersion = version.StartsWith("v", StringComparison.OrdinalIgnoreCase) ? version : "v" + version;
            var metadataIndex = fullVersion.IndexOf('+');
            versionLabel.Text = metadataIndex < 0 ? fullVersion : fullVersion.Substring(0, metadataIndex);
            _iconToolTips.Describe(versionLabel, fullVersion);
        }

        private void LoadDdDriverButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Filter = "DD 驱动 (*.dll)|*.dll" })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;

                LoadDdDriver(dialog.FileName);
                _useInterception = false;
            }
        }

        private void LoadDdDriver(string dllFile)
        {
            var result = _ddControl.Load(dllFile);
            if (result != 1)
            {
                MessageBox.Show("DD 驱动加载失败。", "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            result = _ddControl.btn(0);
            if (result != 1)
            {
                MessageBox.Show("DD 驱动初始化失败。", "初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            toggleButton.Enabled = true;
            SetStatus(false, "DD 驱动就绪");
        }

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers modkey, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                AshStopwatch.UsePrecise = preciseDelayCheckBox.Checked;
                _runtimeMappings = AshConfigPool.Instance.CreateRuntimeMappings(_useInterception);
                if (_runtimeMappings.Count == 0)
                {
                    MessageBox.Show("请先添加至少一条按键序列。", "没有可运行的映射", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (_interceptionControl != null)
                {
                    _interceptionControl.UseReusableSendBuffer = highPerformanceCheckBox.Checked;
                }
                var modes = _runtimeMappings.Where(mapping => mapping.TriggerMode == AshTriggerMode.Toggle).ToList();
                var registeredHotKeys = new List<int>();

                for (var i = 0; i < modes.Count; i++)
                {
                    if (!RegisterKey(modes[i].TriggerKeyCode, 0))
                    {
                        foreach (var virtualKey in registeredHotKeys)
                        {
                            UnregisterHotKey(Handle, HotKeyIdBase + virtualKey);
                        }

                        _toggledKeys.Clear();
                        return;
                    }

                    registeredHotKeys.Add(modes[i].TriggerKeyCode);
                }

                _isRunning = true;
                AshConfigPool.Instance.PressDelay = (int)pressDelayNumeric.Value;
                AshTimer.Instance.Delay = AshConfigPool.Instance.PressDelay;
                AshTimer.Instance.Func = ProcessMappings;
                AshTimer.Instance.Start();
                SetStatus(true, "正在运行");
                SetConfigurationEnabled(false);
            }
            else
            {
                _isRunning = false;
                AshTimer.Instance.Stop();
                foreach (var mapping in _runtimeMappings)
                {
                    if (mapping.TriggerMode == AshTriggerMode.Toggle)
                    {
                        UnregisterHotKey(Handle, HotKeyIdBase + mapping.TriggerKeyCode);
                    }
                }

                _toggledKeys.Clear();
                _runtimeMappings.Clear();
                SetStatus(false, "已停止");
                SetConfigurationEnabled(true);
            }
        }

        private void PressDelayNumeric_ValueChanged(object sender, EventArgs e)
        {
            AshConfigPool.Instance.PressDelay = (int)pressDelayNumeric.Value;
            AshTimer.Instance.Delay = AshConfigPool.Instance.PressDelay;
            AshConfigPool.Instance.SaveConfig();
        }

        private void ReleaseDelayNumeric_ValueChanged(object sender, EventArgs e)
        {
            AshConfigPool.Instance.ReleaseDelay = (int)releaseDelayNumeric.Value;
            AshConfigPool.Instance.SaveConfig();
        }

        private void HighPerformanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AshConfigPool.Instance.UseHighPerformanceInput = highPerformanceCheckBox.Checked;
            AshConfigPool.Instance.SaveConfig();
        }

        private void AddMappingButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new KeySelectionFrame())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        AshConfigPool.Instance.ReplaceMapping(null, dialog.Mapping);
                        RefreshMappingList();
                    }
                    catch (InvalidOperationException exception)
                    {
                        MessageBox.Show(exception.Message, "无法保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void DeleteMappingButton_Click(object sender, EventArgs e)
        {
            if (_isRunning || !(mappingListView.SelectedItems.Count > 0 &&
                  mappingListView.SelectedItems[0].Tag is AshKeyMapping mapping)) return;

            var trigger = AshKeyMap.Get(mapping.TriggerKeyCode).Description;
            if (MessageBox.Show(this, $"删除「{trigger}」的按键映射？", "删除映射",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes) return;

            AshConfigPool.Instance.RemoveMapping(mapping);
            RefreshMappingList();
        }

        private void EditMappingButton_Click(object sender, EventArgs e)
        {
            if (_isRunning || !(mappingListView.SelectedItems.Count > 0 &&
                  mappingListView.SelectedItems[0].Tag is AshKeyMapping mapping)) return;

            using (var dialog = new KeySelectionFrame(mapping))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    AshConfigPool.Instance.ReplaceMapping(mapping, dialog.Mapping);
                    RefreshMappingList();
                }
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show(exception.Message, "无法保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool RegisterKey(int vKey, KeyModifiers modifiers)
        {
            var id = HotKeyIdBase + vKey;

            if (!RegisterHotKey(Handle, id, modifiers, (Keys)vKey))
            {
                MessageBox.Show($"热键注册失败: {AshKeyMap.Get(vKey).Description}", "无法启动",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _toggledKeys[vKey] = false;
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmHotKey)
            {
                var id = m.WParam.ToInt32();
                var vKey = id - HotKeyIdBase;

                if (_runtimeMappings.Any(mapping => mapping.TriggerKeyCode == vKey &&
                                                    mapping.TriggerMode == AshTriggerMode.Toggle))
                {
                    _toggledKeys[vKey] = !_toggledKeys[vKey];
                    if (_toggledKeys[vKey] && soundEffectCheckBox.Checked)
                    {
                        _startSound.Play();
                    }
                    else if (soundEffectCheckBox.Checked)
                    {
                        _stopSound.Play();
                    }
                }
            }

            base.WndProc(ref m);
        }

        private void UseInterceptionButton_Click(object sender, EventArgs e)
        {
            loadDdDriverButton.Enabled = false;
            toggleButton.Enabled = true;
            _useInterception = true;
            SetStatus(false, "Interception 已就绪");
        }

        private void InstallDriverButton_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show(
                "即将安装 Interception 驱动。\n\n" +
                "该驱动基于 LGPL 协议发布。\n" +
                "项目地址: https://github.com/oblitum/Interception/\n\n" +
                "此驱动用于模拟键盘和鼠标输入，可能影响输入设备的行为。\n\n" +
                "是否继续？",
                "确认安装",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            var result = RunInstaller("/install");
            if (result.Contains("successfully installed"))
            {
                RunScCommand(
                    "sc create keyboard type= kernel start= auto binPath= \"C:\\Windows\\System32\\drivers\\keyboard.sys\"");
                RunScCommand(
                    "sc create mouse type= kernel start= auto binPath= \"C:\\Windows\\System32\\drivers\\mouse.sys\"");
                RunScCommand(
                    "sc start keyboard");
                RunScCommand(
                    "sc start mouse");
                MessageBox.Show(
                    "安装成功，但是你应该需要重启一下电脑才能使用喔！\n\n" +
                    "提示：如果长时间不使用该驱动，建议及时卸载以减少安全隐患。",
                    "安装完成",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (result.Contains("Could not write"))
            {
                MessageBox.Show(
                    "文件写入失败，可能是驱动已经安装过。\n\n" +
                    "请尝试重启电脑后重新运行程序，并检查按键功能是否正常开启。",
                    "安装失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(
                    "操作失败，详细信息如下：\n\n" + result,
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string RunInstaller(string arguments)
        {
            string tempInstallerPath = Path.Combine(Path.GetTempPath(), "installer.exe");
            File.WriteAllBytes(tempInstallerPath, Properties.Resources.installer);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = tempInstallerPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            try
            {
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        return lines[lines.Length - 1];
                    }

                    throw new InvalidOperationException("安装程序没有返回输出。");
                }
            }
            finally
            {
                if (File.Exists(tempInstallerPath)) File.Delete(tempInstallerPath);
            }
        }

        private void UninstallDriverButton_Click(object sender, EventArgs e)
        {
            var confirmation = MessageBox.Show(
                "确定要卸载 Interception 驱动吗？\n\n" +
                "卸载后按键功能将无法使用，并且需要重启电脑才能完成卸载。",
                "确认卸载驱动",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes) return;

            CreateRegistry("keyboard");
            CreateRegistry("mouse");
            var result = RunInstaller("/uninstall");
            if (result.Contains("uninstalled"))
            {
                MessageBox.Show(
                    "卸载成功！请重启电脑以完成卸载过程。\n\n" +
                    "提示：如果需要重新安装驱动，请重启后再运行。",
                    "卸载完成",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "操作失败，详细信息如下：\n\n" + result,
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void CreateRegistry(string serviceName)
        {
            try
            {
                using (RegistryKey servicesKey =
                       Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services", true))
                {
                    if (servicesKey == null)
                    {
                        throw new Exception("Failed to open Services registry key.");
                    }

                    using (RegistryKey serviceKey = servicesKey.CreateSubKey(serviceName))
                    {
                        if (serviceKey == null)
                        {
                            throw new Exception($"Failed to create {serviceName} registry key.");
                        }

                        serviceKey.CreateSubKey("Security")?.Close();
                        serviceKey.CreateSubKey("Enum")?.Close();

                        Console.WriteLine($"{serviceName} registry recreated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recreating {serviceName} registry: {ex.Message}");
            }
        }

        private void GithubButton_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/lj2000lj/AshClicker/");
        }

        private void VoiceLicenseButton_Click(object sender, EventArgs e)
        {
            OpenUrl("https://zunko.jp/con_ongen_kiyaku.html");
        }

        private static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }

        private void MappingListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMappingActions();
        }

        private void UpdateMappingActions()
        {
            var canEdit = !_isRunning && mappingListView.SelectedItems.Count > 0;
            editMappingButton.Enabled = canEdit;
            deleteMappingButton.Enabled = canEdit;
        }

        private void SetConfigurationEnabled(bool enabled)
        {
            preciseDelayCheckBox.Enabled = enabled;
            highPerformanceCheckBox.Enabled = enabled;
            pressDelayNumeric.Enabled = enabled;
            releaseDelayNumeric.Enabled = enabled;
            addMappingButton.Enabled = enabled;
            UpdateMappingActions();
            installDriverButton.Enabled = enabled;
            uninstallDriverButton.Enabled = enabled;
        }

        private void SetStatus(bool running, string text)
        {
            SetRunIcon(running);
            statusValueLabel.Text = text;
            statusDotLabel.ForeColor = running
                ? System.Drawing.Color.FromArgb(55, 184, 126)
                : System.Drawing.Color.FromArgb(142, 151, 172);
        }

        private void RefreshMappingList()
        {
            mappingListView.Items.Clear();
            foreach (var mapping in AshConfigPool.Instance.Mappings)
            {
                mappingListView.Items.Add(AshUtil.CreateKeyMappingItem(mapping));
            }

            mappingCountLabel.Text = $"{mappingListView.Items.Count} 条";
            UpdateMappingActions();
        }

        private static void LogDriverLoadFailure(Exception exception)
        {
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
            File.AppendAllText(logFilePath,
                $"[{DateTime.Now}] Exception: {exception.Message}{Environment.NewLine}{exception.StackTrace}{Environment.NewLine}");

            try
            {
                var mouseStatus = RunScCommand("sc query mouse.sys");
                var keyboardStatus = RunScCommand("sc query keyboard.sys");
                File.AppendAllText(logFilePath,
                    $"[{DateTime.Now}] SC Query Results:{Environment.NewLine}" +
                    $"Mouse.sys Status:{Environment.NewLine}{mouseStatus}{Environment.NewLine}" +
                    $"Keyboard.sys Status:{Environment.NewLine}{keyboardStatus}{Environment.NewLine}");
            }
            catch (Exception queryException)
            {
                File.AppendAllText(logFilePath,
                    $"[{DateTime.Now}] Error querying SC status: {queryException.Message}{Environment.NewLine}");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_isRunning)
            {
                _isRunning = false;
                AshTimer.Instance.Stop();
            }

            _startSound?.Dispose();
            _stopSound?.Dispose();
            _interceptionControl?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
