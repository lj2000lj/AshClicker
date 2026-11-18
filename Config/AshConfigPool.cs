using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AshClicker
{
    public sealed class AshConfigPool
    {
        private const string ConfigFilePath = "AshConfig.json";
        private static readonly Lazy<AshConfigPool> LazyInstance =
            new Lazy<AshConfigPool>(() => new AshConfigPool());

        private readonly List<AshKeyMapping> _mappings = new List<AshKeyMapping>();

        public static AshConfigPool Instance => LazyInstance.Value;

        public int PressDelay { get; set; }
        public int ReleaseDelay { get; set; }
        public bool UseHighPerformanceInput { get; set; }
        public IReadOnlyList<AshKeyMapping> Mappings => _mappings;

        private AshConfigPool()
        {
        }

        public void LoadConfig()
        {
            if (!File.Exists(ConfigFilePath)) return;

            try
            {
                var json = File.ReadAllText(ConfigFilePath);
                var config = JsonConvert.DeserializeObject<ConfigData>(json);
                if (config == null) return;

                PressDelay = config.PressDelay;
                ReleaseDelay = config.ReleaseDelay;
                UseHighPerformanceInput = config.UseHighPerformanceInput;

                _mappings.Clear();
                if (config.Mappings != null && config.Mappings.Count > 0)
                {
                    foreach (var mapping in config.Mappings.Where(IsValidMapping))
                    {
                        NormalizeMapping(mapping);
                        _mappings.Add(mapping);
                    }
                }
                else
                {
                    _mappings.AddRange(MigrateLegacyMappings(config));
                    if (_mappings.Count > 0) SaveConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
            }
        }

        public void SaveConfig()
        {
            try
            {
                var config = new ConfigData
                {
                    Mappings = _mappings,
                    PressDelay = PressDelay,
                    ReleaseDelay = ReleaseDelay,
                    UseHighPerformanceInput = UseHighPerformanceInput
                };
                File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public void ReplaceMapping(AshKeyMapping original, AshKeyMapping replacement)
        {
            if (!IsValidMapping(replacement)) throw new ArgumentException("按键映射无效。", nameof(replacement));

            var duplicate = _mappings.FirstOrDefault(mapping =>
                mapping.TriggerKeyCode == replacement.TriggerKeyCode && !ReferenceEquals(mapping, original));
            if (duplicate != null)
            {
                throw new InvalidOperationException("同一个触发按键只能配置一条映射。");
            }

            if (original == null)
            {
                _mappings.Add(replacement);
            }
            else
            {
                var index = _mappings.IndexOf(original);
                if (index >= 0) _mappings[index] = replacement;
                else _mappings.Add(replacement);
            }

            SaveConfig();
        }

        public void RemoveMapping(AshKeyMapping mapping)
        {
            if (mapping == null) return;
            _mappings.Remove(mapping);
            SaveConfig();
        }

        internal List<AshRuntimeMapping> CreateRuntimeMappings(bool useScanCodes)
        {
            return _mappings.Select(mapping =>
            {
                var runtime = new AshRuntimeMapping
                {
                    TriggerKeyCode = mapping.TriggerKeyCode,
                    TriggerMode = mapping.TriggerMode
                };

                foreach (var step in mapping.Steps)
                {
                    var key = AshKeyMap.Get(step.KeyCode);
                    runtime.Steps.Add(new AshRuntimeStep
                    {
                        OutputCode = useScanCodes ? key.ScanCode : key.DdCode,
                        RepeatCount = Math.Max(step.RepeatCount, 1),
                        HoldMilliseconds = step.HoldMilliseconds > 0 ? step.HoldMilliseconds : ReleaseDelay,
                        DelayAfterMilliseconds = Math.Max(step.DelayAfterMilliseconds, 0)
                    });
                }

                return runtime;
            }).ToList();
        }

        private static bool IsValidMapping(AshKeyMapping mapping)
        {
            return mapping != null && mapping.TriggerKeyCode != 0 && mapping.Steps != null &&
                   mapping.Steps.Count > 0 && mapping.Steps.All(step => step.KeyCode != 0);
        }

        private static void NormalizeMapping(AshKeyMapping mapping)
        {
            foreach (var step in mapping.Steps)
            {
                step.RepeatCount = Math.Max(step.RepeatCount, 1);
                step.HoldMilliseconds = Math.Max(step.HoldMilliseconds, 0);
                step.DelayAfterMilliseconds = Math.Max(step.DelayAfterMilliseconds, 0);
            }
        }

        private static IEnumerable<AshKeyMapping> MigrateLegacyMappings(ConfigData config)
        {
            if (config.KeyMap == null) yield break;

            foreach (var pair in config.KeyMap)
            {
                List<int> legacySequence;
                if (config.SequenceMap == null || !config.SequenceMap.TryGetValue(pair.Key, out legacySequence) ||
                    legacySequence == null || legacySequence.Count == 0)
                {
                    legacySequence = new List<int> { pair.Value };
                }

                yield return new AshKeyMapping
                {
                    TriggerKeyCode = pair.Key,
                    TriggerMode = (AshTriggerMode)(config.ModeMap?.GetValueOrDefault(pair.Key, 0) ?? 0),
                    Steps = legacySequence.Select(keyCode => new AshSequenceStep
                    {
                        KeyCode = keyCode,
                        RepeatCount = 1
                    }).ToList()
                };
            }
        }

        private sealed class ConfigData
        {
            public List<AshKeyMapping> Mappings { get; set; }
            public int PressDelay { get; set; }
            public int ReleaseDelay { get; set; }
            public bool UseHighPerformanceInput { get; set; }

            // Legacy fields are read once and rewritten into Mappings on the next save.
            public Dictionary<int, int> KeyMap { get; set; }
            public Dictionary<int, int> ModeMap { get; set; }
            public Dictionary<int, List<int>> SequenceMap { get; set; }
        }
    }
}
