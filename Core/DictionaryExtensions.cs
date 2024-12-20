using System.Collections.Generic;

namespace AshClicker
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value; // 更新值
            }
            else
            {
                dictionary.Add(key, value); // 添加新键值对
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue = default)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}