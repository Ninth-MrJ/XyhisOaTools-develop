using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_XYHIS_OA_TOOLS.Common;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;//设置关闭模式为主窗体关闭则应用程序关闭

            //this.Startup += new StartupEventHandler(App_FrameStartup);//解决占用CUP高的问题，WPF的Animati
            this.Startup += new StartupEventHandler(App_DisableBackspaceShortcutKeysStartup);//禁用Backspace快捷键向后回退各个页面
            this.Startup += new StartupEventHandler(App_DisableF5ShortcutKeysStartup);//禁用F5快捷键刷新页面功能
            this.Startup += new StartupEventHandler(App_ImproveProcessPriorityStartup);//提高进程优先级
        }

        /// <summary>
        /// 修改帧幅数，调低了CUP占用率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_FrameStartup(object sender, StartupEventArgs e)
        {
            System.Windows.Media.Animation.Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(System.Windows.Media.Animation.Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 20 });
        }

        /// <summary>
        /// 禁用Backspace快捷键向后回退各个页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DisableBackspaceShortcutKeysStartup(object sender, StartupEventArgs e)
        {
            //禁用Backspace快捷键向后回退各个页面
            System.Windows.Input.NavigationCommands.BrowseBack.InputGestures.Clear();
        }

        /// <summary>
        /// 禁用F5快捷键界面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DisableF5ShortcutKeysStartup(object sender, StartupEventArgs e)
        {
            //禁用F5快捷键强制刷新页面
            System.Windows.Input.NavigationCommands.Refresh.InputGestures.Clear();
        }

        /// <summary>
        /// 提高程序进程优先级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_ImproveProcessPriorityStartup(object sender, StartupEventArgs e)
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

        }
    }
}
