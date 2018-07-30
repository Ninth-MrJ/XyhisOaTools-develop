using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_XYHIS_OA_TOOLS.Common;
using WPF_XYHIS_OA_TOOLS.Config;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// SatisfactoryCheck.xaml 的交互逻辑
    /// </summary>
    public partial class SatisfactoryCheck : MetroWindow
    {
        public SatisfactoryCheck()
        {
            InitializeComponent();

            #region 计时器初始化

            signuoOkTimer.SetTick(signuoOkTimer_Tick);
            taskGoTimer.SetTick(taskGoTimer_Tick);
            overTimer.SetTick(overTimer_Tick);

            #endregion

            SetProgressRing(true, "登入成功，正在获取 Xyhis.OA 的页面数据，请稍后...");
            Global.wbXyhisOa.Url = new Uri("https://oa.xyhis.com/hr/SatisfiedAdmin.aspx?Id=0");
            signuoOkTimer.Open();
        }

        private void signuoOkTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                signuoOkTimer.Close();
                SetProgressRing(false);
            }
        }

        private void tclPanel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn == null)
                return;
            switch (btn.Name)
            {
                case "btnGo":
                    BtnGoClick();
                    break;
                case "btnMenu":
                    this.fytMenu.IsOpen = true;
                    break;
            }
        }

        private void BtnGoClick()
        {
            fytMenu.IsOpen = false;
            btnGo.IsEnabled = false;
            SetProgressRing(true, "正在进行中，请稍后...");
            taskGoTimer.Open();
        }

        private void taskGoTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                taskGoTimer.Close();
                var cout = rbtnVeryPoor.IsChecked == true ? 4 :
                        rbtnCommonly.IsChecked == true ? 6 :
                        rbtnGood.IsChecked == true ? 8 :
                        rbtnVeryGood.IsChecked == true ? 10 : 8;

                var gvArticle_ctl02_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl02_txtFraction");
                gvArticle_ctl02_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl03_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl03_txtFraction");
                gvArticle_ctl03_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl04_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl04_txtFraction");
                gvArticle_ctl04_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl05_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl05_txtFraction");
                gvArticle_ctl05_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl06_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl06_txtFraction");
                gvArticle_ctl06_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl07_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl07_txtFraction");
                gvArticle_ctl07_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl08_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl08_txtFraction");
                gvArticle_ctl08_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl09_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl09_txtFraction");
                gvArticle_ctl09_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl10_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl10_txtFraction");
                gvArticle_ctl10_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl11_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl11_txtFraction");
                gvArticle_ctl11_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl12_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl12_txtFraction");
                gvArticle_ctl12_txtFraction.SetAttribute("value", cout.ToString());
                var gvArticle_ctl13_txtFraction = WbHelper.GetHtmlElement("gvArticle_ctl13_txtFraction");
                gvArticle_ctl13_txtFraction.SetAttribute("value", cout.ToString());

                var btnSaver = WbHelper.GetHtmlElement("btnSaver");
                btnSaver.InvokeMember("click");

                overTimer.Open(); 
            }
        }

        private void overTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                overTimer.Close();

                GoOver();
            }
        }

        private async void GoOver()
        {
            SetProgressRing(false);
            MessageDialogResult result = await this.ShowMessageAsync("系统提示", "恭喜，全部已完成。");
            if (result == MessageDialogResult.Affirmative)
            {
                fytOk.IsOpen = true;
            }
        }

        #region 内部方法

        private void SetProgressRing(bool isActive, string msg = "")
        {
            if (isActive)
            {
                wplGo.IsEnabled = false;
                splProgressRing.Visibility = Visibility.Visible;
                prgLoding.IsActive = true;
                tbInvokeMsg.Text = msg;
            }
            else
            {
                wplGo.IsEnabled = true;
                splProgressRing.Visibility = Visibility.Collapsed;
                prgLoding.IsActive = false;
                tbInvokeMsg.Text = "";
            }
        }

        #endregion

        #region 计时器

        WFTimer signuoOkTimer = new WFTimer();

        WFTimer taskGoTimer = new WFTimer();

        WFTimer overTimer = new WFTimer();
        #endregion
    }
}
