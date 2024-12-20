using System.Collections.Generic;

namespace AshClicker
{
    public static class AshKeyMap
    {
        public static string MapMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    return "按键切换";
                default:
                    return "长按连发";
            }
        }

        public static AshKey Get(int keycode)
        {
            if (!keyMap.TryGetValue(keycode, out var ashKey))
            {
                ashKey = keyMap.GetValueOrDefault(keycode, keyMap[0xFFFF]);
            }

            return ashKey;
        }

        private static Dictionary<int, AshKey> keyMap = new Dictionary<int, AshKey>()
        {
            [0x1B] = new AshKey("ESC", 0x1B, 100, 0x01, "退出键"),
            [0x70] = new AshKey("F1", 0x70, 101, 0x3B, "功能键 F1"),
            [0x71] = new AshKey("F2", 0x71, 102, 0x3C, "功能键 F2"),
            [0x72] = new AshKey("F3", 0x72, 103, 0x3D, "功能键 F3"),
            [0x73] = new AshKey("F4", 0x73, 104, 0x3E, "功能键 F4"),
            [0x74] = new AshKey("F5", 0x74, 105, 0x3F, "功能键 F5"),
            [0x75] = new AshKey("F6", 0x75, 106, 0x40, "功能键 F6"),
            [0x76] = new AshKey("F7", 0x76, 107, 0x41, "功能键 F7"),
            [0x77] = new AshKey("F8", 0x77, 108, 0x42, "功能键 F8"),
            [0x78] = new AshKey("F9", 0x78, 109, 0x43, "功能键 F9"),
            [0x79] = new AshKey("F10", 0x79, 110, 0x44, "功能键 F10"),
            [0x7A] = new AshKey("F11", 0x7A, 111, 0x57, "功能键 F11"),
            [0x7B] = new AshKey("F12", 0x7B, 112, 0x58, "功能键 F12"),
            [0xC0] = new AshKey("`", 0xC0, 200, 0x29, "反引号键"),
            [0x31] = new AshKey("1", 0x31, 201, 0x02, "数字键 1"),
            [0x32] = new AshKey("2", 0x32, 202, 0x03, "数字键 2"),
            [0x33] = new AshKey("3", 0x33, 203, 0x04, "数字键 3"),
            [0x34] = new AshKey("4", 0x34, 204, 0x05, "数字键 4"),
            [0x35] = new AshKey("5", 0x35, 205, 0x06, "数字键 5"),
            [0x36] = new AshKey("6", 0x36, 206, 0x07, "数字键 6"),
            [0x37] = new AshKey("7", 0x37, 207, 0x08, "数字键 7"),
            [0x38] = new AshKey("8", 0x38, 208, 0x09, "数字键 8"),
            [0x39] = new AshKey("9", 0x39, 209, 0x0A, "数字键 9"),
            [0x30] = new AshKey("0", 0x30, 210, 0x0B, "数字键 0"),
            [0xBD] = new AshKey("-", 0xBD, 211, 0x0C, "减号键"),
            [0xBB] = new AshKey("=", 0xBB, 212, 0x0D, "等号键"),
            [0xDC] = new AshKey("\\", 0xDC, 213, 0x2B, "反斜杠键"),
            [0x08] = new AshKey("BACKSPACE", 0x08, 214, 0x0E, "退格键"),
            [0x09] = new AshKey("TAB", 0x09, 300, 0x0F, "制表键"),
            [0x51] = new AshKey("q", 0x51, 301, 0x10, "Q 键"),
            [0x57] = new AshKey("w", 0x57, 302, 0x11, "W 键"),
            [0x45] = new AshKey("e", 0x45, 303, 0x12, "E 键"),
            [0x52] = new AshKey("r", 0x52, 304, 0x13, "R 键"),
            [0x54] = new AshKey("t", 0x54, 305, 0x14, "T 键"),
            [0x59] = new AshKey("y", 0x59, 306, 0x15, "Y 键"),
            [0x55] = new AshKey("u", 0x55, 307, 0x16, "U 键"),
            [0x49] = new AshKey("i", 0x49, 308, 0x17, "I 键"),
            [0x4F] = new AshKey("o", 0x4F, 309, 0x18, "O 键"),
            [0x50] = new AshKey("p", 0x50, 310, 0x19, "P 键"),
            [0xDB] = new AshKey("[", 0xDB, 311, 0x1A, "左方括号键"),
            [0xDD] = new AshKey("]", 0xDD, 312, 0x1B, "右方括号键"),
            [0x0D] = new AshKey("ENTER", 0x0D, 313, 0x1C, "回车键"),
            [0x14] = new AshKey("CAPS", 0x14, 400, 0x3A, "大写锁定键"),
            [0x41] = new AshKey("a", 0x41, 401, 0x1E, "A 键"),
            [0x53] = new AshKey("s", 0x53, 402, 0x1F, "S 键"),
            [0x44] = new AshKey("d", 0x44, 403, 0x20, "D 键"),
            [0x46] = new AshKey("f", 0x46, 404, 0x21, "F 键"),
            [0x47] = new AshKey("g", 0x47, 405, 0x22, "G 键"),
            [0x48] = new AshKey("h", 0x48, 406, 0x23, "H 键"),
            [0x4A] = new AshKey("j", 0x4A, 407, 0x24, "J 键"),
            [0x4B] = new AshKey("k", 0x4B, 408, 0x25, "K 键"),
            [0x4C] = new AshKey("l", 0x4C, 409, 0x26, "L 键"),
            [0xBA] = new AshKey(";", 0xBA, 410, 0x27, "分号键"),
            [0xDE] = new AshKey("'", 0xDE, 411, 0x28, "单引号键"),
            [0xA0] = new AshKey("LSHIFT", 0xA0, 500, 0x2A, "左 Shift 键"),
            [0x5A] = new AshKey("z", 0x5A, 501, 0x2C, "Z 键"),
            [0x58] = new AshKey("x", 0x58, 502, 0x2D, "X 键"),
            [0x43] = new AshKey("c", 0x43, 503, 0x2E, "C 键"),
            [0x56] = new AshKey("v", 0x56, 504, 0x2F, "V 键"),
            [0x42] = new AshKey("b", 0x42, 505, 0x30, "B 键"),
            [0x4E] = new AshKey("n", 0x4E, 506, 0x31, "N 键"),
            [0x4D] = new AshKey("m", 0x4D, 507, 0x32, "M 键"),
            [0xBC] = new AshKey(",", 0xBC, 508, 0x33, "逗号键"),
            [0xBE] = new AshKey(".", 0xBE, 509, 0x34, "句号键"),
            [0xBF] = new AshKey("/", 0xBF, 510, 0x35, "斜杠键"),
            [0xA1] = new AshKey("RSHIFT", 0xA1, 511, 0x36, "右 Shift 键"),
            [0xA2] = new AshKey("LCTRL", 0xA2, 600, 0x1D, "左 Ctrl 键"),
            [0x5B] = new AshKey("WIN", 0x5B, 601, 0x5B, "Windows 键"),
            [0xA4] = new AshKey("LALT", 0xA4, 602, 0x38, "左 Alt 键"),
            [0x20] = new AshKey("SPACE", 0x20, 603, 0x39, "空格键"),
            [0xA5] = new AshKey("RALT", 0xA5, 604, 0x38, "右 Alt 键"),
            [0x5C] = new AshKey("RWIN", 0x5C, 605, 0x5C, "右 Windows 键"),
            [0x2C] = new AshKey("PRINT", 0x2C, 700, 0x37, "打印屏幕键"),
            [0x91] = new AshKey("SCROLL", 0x91, 701, 0x46, "滚动锁定键"),
            [0x13] = new AshKey("PAUSE", 0x13, 702, 0x45, "暂停键"),
            [0x2D] = new AshKey("INS", 0x2D, 703, 0x52, "插入键"),
            [0x24] = new AshKey("HOME", 0x24, 704, 0x47, "主页键"),
            [0x21] = new AshKey("PGUP", 0x21, 705, 0x49, "向上翻页键"),
            [0x2E] = new AshKey("DEL", 0x2E, 706, 0x53, "删除键"),
            [0x23] = new AshKey("END", 0x23, 707, 0x4F, "结束键"),
            [0x22] = new AshKey("PGDN", 0x22, 708, 0x51, "向下翻页键"),
            [0x26] = new AshKey("UP", 0x26, 709, 0x48, "向上箭头键"),
            [0x25] = new AshKey("LEFT", 0x25, 710, 0x4B, "向左箭头键"),
            [0x28] = new AshKey("DOWN", 0x28, 711, 0x50, "向下箭头键"),
            [0x27] = new AshKey("RIGHT", 0x27, 712, 0x4D, "向右箭头键"),
            [0x60] = new AshKey("NUM0", 0x60, 800, 0x52, "数字键盘 0"),
            [0x61] = new AshKey("NUM1", 0x61, 801, 0x4F, "数字键盘 1"),
            [0x62] = new AshKey("NUM2", 0x62, 802, 0x50, "数字键盘 2"),
            [0x63] = new AshKey("NUM3", 0x63, 803, 0x51, "数字键盘 3"),
            [0x64] = new AshKey("NUM4", 0x64, 804, 0x4B, "数字键盘 4"),
            [0x65] = new AshKey("NUM5", 0x65, 805, 0x4C, "数字键盘 5"),
            [0x66] = new AshKey("NUM6", 0x66, 806, 0x4D, "数字键盘 6"),
            [0x67] = new AshKey("NUM7", 0x67, 807, 0x47, "数字键盘 7"),
            [0x68] = new AshKey("NUM8", 0x68, 808, 0x48, "数字键盘 8"),
            [0x69] = new AshKey("NUM9", 0x69, 809, 0x49, "数字键盘 9"),
            [0x90] = new AshKey("NUMLOCK", 0x90, 810, 0x45, "数字锁定键"),
            [0x6F] = new AshKey("NUM/", 0x6F, 811, 0x35, "数字键盘 /"),
            [0x6A] = new AshKey("NUM*", 0x6A, 812, 0x37, "数字键盘 *"),
            [0x6D] = new AshKey("NUM-", 0x6D, 813, 0x4A, "数字键盘 -"),
            [0x6B] = new AshKey("NUM+", 0x6B, 814, 0x4E, "数字键盘 +"),
            [0x6E] = new AshKey("NUM.", 0x6E, 816, 0x53, "数字键盘 ."),
            [0xFFFF] = new AshKey("FN", 0xFFFF, 606, 0x00, "（按下选择）")
        };
    }
}