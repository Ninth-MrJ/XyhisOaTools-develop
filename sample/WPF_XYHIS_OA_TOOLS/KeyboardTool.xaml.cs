using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// KeyboardTool.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardTool : MetroWindow
    {
        public KeyboardTool()
        {
            InitializeComponent();

            timer.Elapsed += timer_Elapsed;
            timeTimer.Elapsed += timeTimer_Elapsed;
        }

        private void gPanel_Click(object sender, RoutedEventArgs e)
        {
            var btn = e.Source as Button;
            if (btn == null)
                return;

            switch (btn.Name)
            {
                case "btnGo":
                    timer.Start();
                    timeTimer.Start();
                    break;
                case "btnEnd":
                    timer.Stop();
                    timeTimer.Stop();
                    break;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string jpKeys = "qwertyuiopasdfghjklzxcvbnm1234567890";
            while (true)
            {
                Random rd = new Random();
                var index = rd.Next(0, jpKeys.Length);
                System.Windows.Forms.SendKeys.SendWait(jpKeys[index].ToString());
                Dispatcher.Invoke(delegate ()
                {
                    tbCount.Text = count.ToString();
                });
                count++;
                System.Threading.Thread.Sleep(200);
            }
        }

        private void timeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(delegate ()
            {
                tbGoTime.Text = goTime.ToString("hh:mm:ss");
            });
            goTime.AddSeconds(1);
        }

        #region 局部变量

        private int count = 0;

        private DateTime goTime = new DateTime(0001, 01, 01, 0, 0, 0, 0);

        System.Timers.Timer timer = new System.Timers.Timer
        {
            Interval = 200,
            Enabled = false
        };

        System.Timers.Timer timeTimer = new System.Timers.Timer
        {
            Interval = 1000,
            Enabled = false
        };

        #endregion
    }
}
