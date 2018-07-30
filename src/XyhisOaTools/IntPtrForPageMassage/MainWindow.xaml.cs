using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntPtrForPageMassage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            timer.Elapsed += timer_Elapsed;
            timer.Start();
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

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //查找窗体
            IntPtr hwnd = FindWindow(null, "来自网页的消息");
            if (hwnd == IntPtr.Zero)
                return;

            //查找窗体中输入文件地址的输入框
            IntPtr hb = FindWindowEx(hwnd, IntPtr.Zero, "Button", null);
            if (hb == IntPtr.Zero)
            {
                IntPtr hdir = FindWindowEx(hwnd, IntPtr.Zero, "DirectUIHWND", null);
                IntPtr hctr = FindWindowEx(hdir, IntPtr.Zero, "CtrlNotifySink", null);
                IntPtr hctr2 = FindWindowEx(hdir, hctr, "CtrlNotifySink", null);
                IntPtr hctr3 = FindWindowEx(hdir, hctr2, "CtrlNotifySink", null);
                IntPtr hctr4 = FindWindowEx(hdir, hctr3, "CtrlNotifySink", null);
                IntPtr hctr5 = FindWindowEx(hdir, hctr4, "CtrlNotifySink", null);
                IntPtr hctr6 = FindWindowEx(hdir, hctr5, "CtrlNotifySink", null);
                IntPtr hctr7 = FindWindowEx(hdir, hctr6, "CtrlNotifySink", null);
                hb = FindWindowEx(hctr7, IntPtr.Zero, "Button", null);
            }
            SendMessage(hb, WM_LBUTTONDOWN, IntPtr.Zero, null);//鼠标按下按钮。
            SendMessage(hb, WM_LBUTTONUP, IntPtr.Zero, null);//鼠标松开按钮。
        }

        Timer timer = new Timer
        {
            Interval = 100,
            Enabled = false
        };
    }
}
