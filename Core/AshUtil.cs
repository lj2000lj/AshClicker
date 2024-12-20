using System;
using System.Windows.Forms;

namespace AshClicker
{
    public class AshUtil
    {
        public static ListViewItem CreateKeyMappingItem(AshKey userPress, AshKey willPress, string modeText, int mode)
        {
            if (userPress == null || willPress == null)
            {
                throw new ArgumentNullException("userPress 和 willPress 都不能为 null。");
            }

            // 创建一个 ListViewItem
            var item = new ListViewItem(userPress.Description);
            item.SubItems.Add(willPress.Description);
            item.SubItems.Add(modeText);

            // 绑定自定义 Tag
            item.Tag = new AshKeypress(userPress, willPress, mode);

            return item;
        }
    }
}