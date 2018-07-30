using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_XYHIS_OA_TOOLS.Common;
using WPF_XYHIS_OA_TOOLS.Config;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : MetroWindow
    {
        public NewWindow()
        {
            InitializeComponent();

            //SystemTheme("Blue", "BaseDark");

            signupTimer.SetTick(signupTimer_Tick);
            //signupOkTimer.SetTick(signupOkTimer_Tick);

            Global.wbXyhisOa.Url = new Uri("http://oa.xyhis.com");

            transitioning.Content = new UserSignup();

            var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "IntPtrForPageMassage.exe";
            var info = System.Diagnostics.Process.Start(path);
        }

        private void gPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Tile tile)
            {
                switch (tile.Name)
                {
                    case "ctlTaskManage":
                        TaskManage taskManage = new TaskManage();
                        taskManage.ShowDialog();
                        break;
                    case "ctlTrainManage":
                        TrainManage trainManage = new TrainManage();
                        trainManage.ShowDialog();
                        break;
                    case "ctlSatisfactoryCheck":
                        SatisfactoryCheck satisfactoryCheck = new SatisfactoryCheck();
                        satisfactoryCheck.ShowDialog();
                        break;
                    case "ctlKeepLog":
                        break;
                    case "ctlRecruitManage":
                        RecruitManage recruitManage = new RecruitManage();
                        recruitManage.ShowDialog();
                        break;
                    case "ctlWrodToPDF":
                        WrodToPDFTool wrodToPDFTool = new WrodToPDFTool();
                        wrodToPDFTool.ShowDialog();
                        break;
                    case "ctlKeyboardTool":
                        KeyboardTool keyboardTool = new KeyboardTool();
                        keyboardTool.ShowDialog();
                        break;
                    case "ctlTellMe":
                        System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=202980469&site=qq&menu=yes");
                        break;
                }
            }
            //else if (e.Source is ToggleSwitch cts)
            //{
            //    switch (cts.Name)
            //    {
            //        case "ctsSysTheme":
            //            if (ctsSysTheme.IsChecked == false)
            //                SystemTheme("Blue", "BaseLight");
            //            else
            //                SystemTheme("Blue", "BaseDark");
            //            break;
            //    }
            //}
            else if (e.OriginalSource is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSignUp":
                        BtnSignUpClick();
                        break;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                BtnSignUpClick();
            }
        }

        private void BtnSignUpClick()
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                var txtUserName = WbHelper.GetHtmlElement("txtUserName");
                txtUserName.SetAttribute("value", ((UserSignup)transitioning.Content).UserName);

                var txtPwd = WbHelper.GetHtmlElement("txtPwd");
                txtPwd.SetAttribute("value", ((UserSignup)transitioning.Content).UserPwd);

                var imgbtnSignUp = WbHelper.GetHtmlElement("ImageButton1");
                imgbtnSignUp.InvokeMember("click");

                ((UserSignup)transitioning.Content).SetProgressRing(true);
                signupTimer.Enabled = true;
            }
        }

        private void signupTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                signupTimer.Enabled = false;

                ((UserSignup)transitioning.Content).SetProgressRing(false);

                if (Global.wbXyhisOa.Url.AbsoluteUri == "https://oa.xyhis.com/Login.aspx")
                {
                    this.ShowMessageAsync("系统", "登入失败，用户名或密码错误。");
                }
                else
                {
                    Global.UserName = ((UserSignup)transitioning.Content).UserName;
                    Global.IsSignupOk = true;

                    InitWorkbenchData();
                }
            }
        }

        private void InitWorkbenchData()
        {
            //Global.wbXyhisOa.Url = new Uri("https://oa.xyhis.com/WelCome2.aspx");
            //signupOkTimer.Enabled = true;

            transitioning.Content = new Workbench();
        }

        //private void signupOkTimer_Tick(object sender, EventArgs e)
        //{
        //    if (WbHelper.WbXyhisOaIsBusy())
        //    {
        //        signupOkTimer.Enabled = false;

        //        var taskRemind = WbHelper.GetHtmlElement("TaskRemind");
        //        if (taskRemind != null)
        //        {
        //            this.ctlTaskManage.Count = (Units.SubstringCount(taskRemind.InnerHtml, "</tr>") - 3).ToString();
        //        }

        //        var trainingRemind = WbHelper.GetHtmlElement("TrainingRemind");
        //        if (trainingRemind != null)
        //        {
        //            this.ctlTrainManage.Count = (Units.SubstringCount(trainingRemind.InnerHtml, "</tr>") - 3).ToString();
        //        }
        //    }
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            System.Diagnostics.Process[] intPtrCommon = System.Diagnostics.Process.GetProcessesByName("IntPtrCommon");
            if (intPtrCommon.Length != 0)
            {
                foreach (var item in intPtrCommon)
                {
                    item.Kill();
                }
            }
            System.Diagnostics.Process[] intPtrForPageMassage = System.Diagnostics.Process.GetProcessesByName("IntPtrForPageMassage");
            if (intPtrForPageMassage.Length != 0)
            {
                foreach (var item in intPtrForPageMassage)
                {
                    item.Kill();
                }
            }
            base.OnClosing(e);
        }

        private void SystemTheme(string accentsName, string appThemeName)
        {
            MahApps.Metro.Accent expectedAccent = MahApps.Metro.ThemeManager.Accents.First(x => x.Name == accentsName);
            MahApps.Metro.AppTheme expectedTheme = MahApps.Metro.ThemeManager.GetAppTheme(appThemeName);
            MahApps.Metro.ThemeManager.ChangeAppStyle(Application.Current, expectedAccent, expectedTheme);
        }

        #region 计时器

        /// <summary>
        /// 点击登入后执行的计时器
        /// </summary>
        private WFTimer signupTimer = new WFTimer();
        ///// <summary>
        ///// 登入成功后执行的计时器
        ///// </summary>
        //private WFTimer signupOkTimer = new WFTimer();
        #endregion

        #region 局部变量

        #endregion
    }
}
