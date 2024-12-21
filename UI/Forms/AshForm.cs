using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AshClicker.Driver;
using Microsoft.Win32;

namespace AshClicker
{
    public partial class AshForm : Form
    {
        private const int ASH_HOTKEY_ID = 12489;
        private DdControl dd;
        private bool started;
        private int delay;
        private int press;
        private bool DDLoaded;
        private static readonly string defaultValue = "(添加)";
        private Dictionary<int, int> _keyMap = new Dictionary<int, int>();
        private Dictionary<int, int> _modeMap = new Dictionary<int, int>();
        private Dictionary<int, bool> pressedKeys = new Dictionary<int, bool>();
        private InterceptionControl ic;
        private InterceptionClient icl;

        public AshForm()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"Version: {version}");

            var infoVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            Console.WriteLine($"Informational Version: {infoVersion}");
            InitializeComponent();
            try
            {
                ic = new InterceptionControl();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // 定义日志文件路径
                var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");

                // 写入日志文件
                File.AppendAllText(logFilePath,
                    $"[{DateTime.Now}] Exception: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}");
                try
                {
                    string mouseStatus = RunScCommand("sc query mouse.sys");
                    string keyboardStatus = RunScCommand("sc query keyboard.sys");

                    File.AppendAllText(logFilePath, $"[{DateTime.Now}] SC Query Results:{Environment.NewLine}");
                    File.AppendAllText(logFilePath,
                        $"Mouse.sys Status:{Environment.NewLine}{mouseStatus}{Environment.NewLine}");
                    File.AppendAllText(logFilePath,
                        $"Keyboard.sys Status:{Environment.NewLine}{keyboardStatus}{Environment.NewLine}");
                }
                catch (Exception scEx)
                {
                    File.AppendAllText(logFilePath,
                        $"[{DateTime.Now}] Error querying SC status: {scEx.Message}{Environment.NewLine}");
                }

                buttonSwitchEnable.Enabled = false;
                MessageBox.Show("还没有安装驱动喔！请记得先点击安装驱动！");
            }
        }

        string RunScCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new Exception($"Error running command: {error}");
            }

            return output;
        }

        private void AshForm_Load(object sender, EventArgs e)
        {
            dd = new DdControl();
            AshConfigPool.Instance.LoadToList(listView1);
            label1.Text = label1.Text.Replace("%version%", ThisAssembly.Git.Tag);
            linkLabel1.LinkClicked += (s, evt) => Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/lj2000lj/AshClicker/",
                UseShellExecute = true
            });
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("winmm.dll")]
        public static extern uint timeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll")]
        public static extern uint timeEndPeriod(uint uMilliseconds);

        private void tick()
        {
            if (!started) return;

            foreach (var kvp in _keyMap)
            {
                int vKey = kvp.Key;
                int ddcode = kvp.Value;

                if (_modeMap.GetValueOrDefault(vKey, 0) == 0 && (GetAsyncKeyState(vKey) & 0x8000) != 0)
                {
                    PressKey(ddcode, press);
                }

                if (_modeMap.GetValueOrDefault(vKey, 0) == 1 && pressedKeys.GetValueOrDefault(vKey, false))
                {
                    PressKey(ddcode, press);
                }
            }
        }

        private void PressKey(int ddcode, int press)
        {
            if (usingInterception)
            {
                /*var keyStroke = new KeyboardInputData
                {
                    UnitId = 0, // 固定为 0
                    MakeCode = (ushort)ddcode, // 'A' 键的扫描码
                    Flags = 0x00, // 按下状态
                    Reserved = 0, // 固定为 0
                    ExtraInformation = 0 // 无附加信息
                };
                icl.SendKeyStroke(keyStroke);
                AshStopwatch.PreciseDelay(press);
                keyStroke.Flags = 0x01;
                icl.SendKeyStroke(keyStroke);*/
                var stroke = new InterceptionControl.Stroke
                {
                    Key = new InterceptionControl.KeyStroke
                    {
                        Code = (ushort)ddcode,
                        State = 0x00 // 按下键
                    }
                };
                ic.Send(1, ref stroke);
                AshStopwatch.PreciseDelay(press);
                stroke.Key.State = 0x01; // 松开键
                ic.Send(1, ref stroke);
            }
            else
            {
                dd.key(ddcode, 1);
                AshStopwatch.PreciseDelay(press);
                dd.key(ddcode, 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "DD|*.DLL" };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            LoadDllFile(ofd.FileName);
            usingInterception = false;
        }

        private void LoadDllFile(string dllfile)
        {
            int ret = dd.Load(dllfile);
            if (ret != 1)
            {
                MessageBox.Show("Load Error");
                return;
            }

            ret = dd.btn(0);
            if (ret != 1)
            {
                MessageBox.Show("Initialize Error");
                return;
            }

            buttonLoadDriver.Text = "DD 驱动已加载";
            buttonSwitchEnable.Enabled = true;
            DDLoaded = true;
        }

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers modkey, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void button2_Click(object sender, EventArgs e)
        {
            started = !started;
            if (started)
            {
                AshStopwatch.UsePrecise = preciseDelay.Checked;
                AshConfigPool.Instance.GetAshConfigs(usingInterception, out _keyMap, out _modeMap);
                var modes = _modeMap.Where(m => m.Value == 1).ToList();

                // No idea why, but with Fody enabled, foreach will throw an exception :(
                for (var i = 0; i < modes.Count; i++)
                {
                    RegisterKey(modes[i].Key, 0);
                }

                delay = Math.Max(int.Parse(textBox1.Text), 1);
                AshTimer.Instance.Delay = delay;
                AshTimer.Instance.Func = tick;
                AshTimer.Instance.Start();
                buttonSwitchEnable.Text = "按键关闭";
                label3.Text = "已开启";
                preciseDelay.Enabled = false;
                buttonAddKey.Enabled = false;
                buttonDeleteKey.Enabled = false;
            }
            else
            {
                AshTimer.Instance.Stop();
                foreach (var modes in _modeMap)
                {
                    if (modes.Value == 1)
                    {
                        UnregisterHotKey(Handle, ASH_HOTKEY_ID + modes.Key);
                    }
                }

                buttonSwitchEnable.Text = "按键开启";
                label3.Text = "关闭";
                preciseDelay.Enabled = true;
                buttonAddKey.Enabled = true;
                buttonDeleteKey.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox1.Text = "1";

            textBox1.Text = new string(textBox1.Text.Where(char.IsDigit).ToArray());
            textBox1.SelectionStart = textBox1.Text.Length;
            delay = Math.Max(int.Parse(textBox1.Text), 1);
            AshTimer.Instance.Delay = delay;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0) textBox2.Text = "1";

            textBox2.Text = new string(textBox2.Text.Where(char.IsDigit).ToArray());
            textBox2.SelectionStart = textBox2.Text.Length;
            press = Math.Max(int.Parse(textBox2.Text), 1);
        }

        private void buttonAddKey_Click(object sender, EventArgs e)
        {
            new KeySelectionFrame(listView1).Show();
        }

        private void buttonDeleteKey_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                listView1.Items.Remove(selectedItem);
            }

            AshConfigPool.Instance.UpdateFromList(listView1);
        }

        private void RegisterKey(int vKey, KeyModifiers modifiers)
        {
            int id = ASH_HOTKEY_ID + vKey;
            _modeMap[vKey] = 1;
            pressedKeys[vKey] = false;

            if (!RegisterHotKey(Handle, id, modifiers, (Keys)vKey))
            {
                MessageBox.Show($"热键注册失败: {vKey}");
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                int vKey = id - ASH_HOTKEY_ID;

                if (_modeMap.GetValueOrDefault(vKey, 0) == 1)
                {
                    pressedKeys[vKey] = !pressedKeys[vKey];
                }
            }

            base.WndProc(ref m);
        }

        private bool usingInterception = true;

        private void buttonDriverless_Click(object sender, EventArgs e)
        {
            buttonLoadDriver.Text = "无驱动模式";
            buttonLoadDriver.Enabled = false;
            buttonSwitchEnable.Enabled = true;
            usingInterception = true;
        }

        private void buttonInstallInterception_Click(object sender, EventArgs e)
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

        static string RunInstaller(string arguments)
        {
            // 创建临时文件来存储 Properties.Resources.installer
            string tempInstallerPath = Path.Combine(Path.GetTempPath(), "installer.exe");
            File.WriteAllBytes(tempInstallerPath, Properties.Resources.installer);

            // 配置进程启动信息
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = tempInstallerPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas" // 以管理员权限运行
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                // 捕获输出
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // 删除临时文件
                if (File.Exists(tempInstallerPath))
                {
                    File.Delete(tempInstallerPath);
                }

                // 返回最后一行的输出
                if (!string.IsNullOrEmpty(output))
                {
                    string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    return lines[lines.Length - 1]; // 返回最后一行
                }
                else
                {
                    throw new Exception("No output received from installer.");
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
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

        static void CreateRegistry(string serviceName)
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

                    // 创建主项
                    using (RegistryKey serviceKey = servicesKey.CreateSubKey(serviceName))
                    {
                        if (serviceKey == null)
                        {
                            throw new Exception($"Failed to create {serviceName} registry key.");
                        }

                        // 可选：创建 Security 和 Enum 子项
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
    }
}