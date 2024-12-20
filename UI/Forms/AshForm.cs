using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            InitializeComponent();
            try
            {
                ic = new InterceptionControl();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(
                    $"没有找到驱动程序，请先安装喔！\n岚尘按键使用 Interception 驱动作为底层\n详情请参考: Interception 项目",
                    "https://github.com/oblitum/Interception/issues");
                Console.WriteLine(ex.Message);
                buttonSwitchEnable.Enabled = false;
            }
        }

        private void AshForm_Load(object sender, EventArgs e)
        {
            dd = new DdControl();
            AshConfigPool.Instance.LoadToList(listView1);
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
                foreach (var modes in _modeMap)
                {
                    if (modes.Value == 1)
                    {
                        RegisterKey(modes.Key, 0);
                    }
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
            MessageBox.Show(RunInstaller("/install"));
            /*try
            {
                if (DriverInstaller.InstallDriver())
                {
                    MessageBox.Show("驱动安装成功，请重启电脑");
                }
                else
                {
                    MessageBox.Show("系统中可能已经存在 Interception 驱动，或者上一次的卸载尚未完成\n请在详细检查之后，重启电脑后再试");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"驱动安装失败: {ex.Message}");
            }*/
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
            MessageBox.Show(RunInstaller("/uninstall"));
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