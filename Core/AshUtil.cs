using System;
using System.Linq;
using System.Windows.Forms;

namespace AshClicker
{
    public static class AshUtil
    {
        public static ListViewItem CreateKeyMappingItem(AshKeyMapping mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            var triggerKey = AshKeyMap.Get(mapping.TriggerKeyCode);
            var sequence = mapping.Steps.Select(step =>
            {
                var text = AshKeyMap.Get(step.KeyCode).KeyName.ToUpperInvariant();
                return step.RepeatCount > 1 ? text + " x" + step.RepeatCount : text;
            });

            var item = new ListViewItem(triggerKey.Description);
            item.SubItems.Add(string.Join("  →  ", sequence));
            item.SubItems.Add(AshKeyMap.MapMode((int)mapping.TriggerMode));
            item.Tag = mapping;

            return item;
        }
    }
}
