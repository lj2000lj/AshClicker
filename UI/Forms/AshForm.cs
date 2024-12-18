using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AshClicker;

public partial class AshForm : Form
{
    private const int ASH_HOTKEY_ID = 12489;
    private DdControl dd;
    private bool started;
    private int delay;
    private int press;
    private bool DDLoaded;
    private static readonly string defaultValue = "(添加)";
    private Dictionary<int, int> _keyMap = new();
    private Dictionary<int, int> _modeMap = new();
    private Dictionary<int, bool> pressedKeys = new();

    public AshForm()
    {
        InitializeComponent();
    }

    private void AshForm_Load(object sender, EventArgs e)
    {
        dd = new DdControl();
        buttonSwitchEnable.Enabled = false;
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
        dd.key(ddcode, 1);
        AshStopwatch.PreciseDelay(press);
        dd.key(ddcode, 2);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog { Filter = "DD|*.DLL" };

        if (ofd.ShowDialog() != DialogResult.OK) return;

        LoadDllFile(ofd.FileName);
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
            (_keyMap, _modeMap) = AshConfigPool.Instance.GetAshConfigs();
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
}