using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AshClicker;

public class AshConfigPool
{
    private static readonly Lazy<AshConfigPool> _instance = new(() => new AshConfigPool());
    public static AshConfigPool Instance => _instance.Value;

    private AshConfigPool()
    {
    }

    Dictionary<int, int> KeyMap { get; set; } = new();
    Dictionary<int, int> ModeMap { get; set; } = new();

    public (Dictionary<int, int> keyMap, Dictionary<int, int> modeMap) GetAshConfigs(bool driverless = false)
    {
        var keyMap = new Dictionary<int, int>();
        var modeMap = new Dictionary<int, int>();

        foreach (var key in KeyMap.Keys)
        {
            var willPress = AshKeyMap.Get(KeyMap[key]);
            var mode = ModeMap.GetValueOrDefault(key, 0);
            keyMap[key] = driverless ? willPress.ScanCode : willPress.DdCode;
            modeMap[key] = mode;
        }

        return (keyMap, modeMap);
    }

    public void LoadToList(ListView listView)
    {
        LoadConfig();
        foreach (var key in KeyMap.Keys)
        {
            var userPress = AshKeyMap.Get(key);
            var willPress = AshKeyMap.Get(KeyMap[key]);
            var mode = ModeMap.GetValueOrDefault(key, 0);
            var modeText = AshKeyMap.MapMode(mode);
            listView.Items.Add(AshUtil.CreateKeyMappingItem(userPress, willPress, modeText, mode));
        }
    }

    public void UpdateFromList(ListView listView)
    {
        KeyMap.Clear();
        ModeMap.Clear();
        var invalid = new HashSet<ListViewItem>();

        foreach (ListViewItem item in listView.Items)
        {
            if (item.Tag is AshKeypress press)
            {
                KeyMap[press.UserPress.VKey] = press.WillPress.VKey;
                ModeMap[press.UserPress.VKey] = press.Mode;
            }
            else
            {
                invalid.Add(item);
            }
        }

        if (invalid.Count > 0)
        {
            MessageBox.Show("检测到错误配置，已删除错误的条目:(");
        }

        foreach (var listViewItem in invalid)
        {
            listView.Items.Remove(listViewItem);
        }

        SaveConfig();
    }

    private static readonly string ConfigFilePath = "AshConfig.json";

    public void SaveConfig()
    {
        try
        {
            var configData = new { KeyMap, ModeMap };
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(configData, options);
            File.WriteAllText(ConfigFilePath, json);
            Console.WriteLine($"Configuration saved to {ConfigFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving configuration: {ex.Message}");
        }
    }

    public void LoadConfig()
    {
        try
        {
            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                var configData = JsonSerializer.Deserialize<ConfigData>(json);

                if (configData != null)
                {
                    KeyMap = configData.KeyMap ?? new Dictionary<int, int>();
                    ModeMap = configData.ModeMap ?? new Dictionary<int, int>();
                    Console.WriteLine("Configuration loaded successfully.");
                }
            }
            else
            {
                Console.WriteLine("Configuration file not found. Using default settings.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading configuration: {ex.Message}");
        }
    }

    private class ConfigData
    {
        public Dictionary<int, int> KeyMap { get; set; }
        public Dictionary<int, int> ModeMap { get; set; }
    }
}