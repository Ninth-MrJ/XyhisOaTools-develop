using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace IntPtrCommon
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WebSelectFile : Window
    {
        private static string fileName = "";

        public WebSelectFile(string fn)
        {
            InitializeComponent();

            fileName = fn;

            timer.Tick += timer_Tick;
            timer.Enabled = true;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (SelectFile())
            {
                timer.Enabled = false;
                this.Close();
            }
        }

        private const int WM_SETTEXT = 0x000C;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_CLOSE = 0x0010;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("User32.dll ")]
        public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childe, string strclass, string FrmText);

        public static bool SelectFile()
        {
            //查找窗体
            IntPtr hwnd = FindWindow(null, "选择要加载的文件");
            if (hwnd == IntPtr.Zero)
                return false;

            //查找窗体中输入文件地址的输入框
            IntPtr hcbe = FindWindowEx(hwnd, IntPtr.Zero, "ComboBoxEx32", null);
            IntPtr hcb = FindWindowEx(hcbe, IntPtr.Zero, "ComboBox", null);
            IntPtr htb = FindWindowEx(hcb, IntPtr.Zero, "Edit", null);
            SendMessage(htb, WM_SETTEXT, IntPtr.Zero, fileName);//填写文本框。

            //查找确认按钮
            IntPtr hbtn = FindWindowEx(hwnd, IntPtr.Zero, "Button", null);//0x00290cd8=打开
            SendMessage(hbtn, WM_LBUTTONDOWN, IntPtr.Zero, null);//鼠标按下按钮。
            SendMessage(hbtn, WM_LBUTTONUP, IntPtr.Zero, null);//鼠标松开按钮。

            return true;
        }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
        {
            Interval = 100,
            Enabled = false
        };
    }
}
