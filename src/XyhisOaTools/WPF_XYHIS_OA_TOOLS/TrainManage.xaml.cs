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
    /// TrainManage.xaml 的交互逻辑
    /// </summary>
    public partial class TrainManage : MetroWindow
    {
        public TrainManage()
        {
            InitializeComponent();

            #region 初始化计时器

            signupOkTimer.SetTick(signupOkTimer_Tick);
            goTrainAddTimer.SetTick(goTrainAdd_Tick);
            trainAddClickedTimer.SetTick(trainAddClicked_Tick);
            trainSaveTimer.SetTick(trainSaveTimer_Tick);

            #endregion

            SetProgressRing(true, "登入成功，正在获取 Xyhis.OA 的页面数据，请稍后...");
            signupOkTimer.Enabled = true;
        }

        private void signupOkTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                signupOkTimer.Enabled = false;

                Global.wbXyhisOa.Url = new Uri("https://oa.xyhis.com/TrainEvaluationList.aspx?comId=1506");

                goTrainAddTimer.Enabled = true;
            }
        }

        private void goTrainAdd_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                goTrainAddTimer.Enabled = false;

                var btnAdd = WbHelper.GetHtmlElement("btnAdd");
                btnAdd.InvokeMember("click");

                trainAddClickedTimer.Enabled = true;
            }
        }

        private void trainAddClicked_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                trainAddClickedTimer.Enabled = false;

                if (Global.wbXyhisOa.Url.AbsoluteUri == "https://oa.xyhis.com/WelCome2.aspx")
                {
                    fytOk.IsOpen = true;
                    return;
                }

                SetProgressRing(false);
                if(i > 0)
                    BtnGoClick();

                i++;
            }
        }

        private void tclPanel_Click(object sender, RoutedEventArgs e)
        {
            var btn = e.Source as Button;
            if (btn == null)
                return;

            SetProgressRing(true, "正在开始执行，请稍后...");

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
            var score = 8;
            if (rbtnVeryPoor.IsChecked == true)
            {
                BtnVeryPoorClick();
                score = 4;
            }
            else if (rbtnCommonly.IsChecked == true)
            {
                BtnCommonlyClick();
                score = 6;
            }
            else if (rbtnGood.IsChecked == true)
            {
                BtnGoodClick();
                score = 8;
            }
            else if (rbtnVeryGood.IsChecked == true)
            {
                BtnVeryGoodClick();
                score = 10;
            }
            else
            {
                BtnGoodClick();
                score = 8;
            }

            if (!string.IsNullOrWhiteSpace(tbScore.Text))
            {
                var parse = int.TryParse(tbScore.Text, out score);
                if (!parse || score > 10)
                {
                    this.ShowMessageAsync("系统提示", "输入有误，总分请填写数字，最高得分为10分。");
                    SetProgressRing(false);
                    return;
                }
            }

            this.fytMenu.IsOpen = false;
            btnGo.IsEnabled = false;

            var txtScore = WbHelper.GetHtmlElement("score");
            txtScore.SetAttribute("value", score.ToString());

            if (!string.IsNullOrWhiteSpace(tbProposal.Text))
            {
                var orthersuggess = WbHelper.GetHtmlElement("orthersuggess");
                orthersuggess.SetAttribute("value", tbProposal.Text);
            }

            var imageButton1 = WbHelper.GetHtmlElement("ImageButton1");
            imageButton1.InvokeMember("click");

            trainSaveTimer.Enabled = true;
        }

        private void BtnVeryPoorClick()
        {
            SetAttributeCheckedBySelectName("很差");
            SetAttributeChecked("Dradio1", "较小");
            SetAttributeChecked("Dradio1", "不满");
        }

        private void BtnCommonlyClick()
        {
            SetAttributeCheckedBySelectName("一般");
            SetAttributeChecked("Dradio1", "普通");
            SetAttributeChecked("Eradio1", "普通");
            SetAttributeCheckeds("Cradio1");
        }

        private void BtnGoodClick()
        {
            SetAttributeCheckedBySelectName("良好");
            SetAttributeChecked("Dradio1", "有效");
            SetAttributeChecked("Eradio1", "满意");
            SetAttributeCheckeds("Cradio1");
        }

        private void BtnVeryGoodClick()
        {
            SetAttributeCheckedBySelectName("很好");
            SetAttributeChecked("Dradio1", "非常有效");
            SetAttributeChecked("Eradio1", "非常满意");
            SetAttributeCheckeds("Cradio1");
        }

        private void SetAttributeCheckedBySelectName(string selectName)
        {
            SetAttributeChecked("Aradio1", selectName);
            SetAttributeChecked("Aradio2", selectName);
            SetAttributeChecked("Aradio3", selectName);
            SetAttributeChecked("Bradio1", selectName);
            SetAttributeChecked("Bradio2", selectName);
            SetAttributeChecked("Bradio3", selectName);
            SetAttributeChecked("Bradio4", selectName);
            SetAttributeChecked("Bradio5", selectName);
        }

        private void SetAttributeChecked(string elementId, string selectName)
        {
            var aradio = WbHelper.GetHtmlElementByOuterHtml(elementId, selectName);
            aradio?.SetAttribute("checked", "checked");
        }

        private void SetAttributeCheckeds(string elementId)
        {
            var aradios = WbHelper.GetHtmlElements(elementId);
            foreach (var item in aradios)
            {
                item?.SetAttribute("checked", "checked");
            }
        }

        private void trainSaveTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                trainSaveTimer.Enabled = false;



                signupOkTimer.Enabled = true;
            }
        }

        #region 公共方法

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

        /// <summary>
        /// 登入成功
        /// </summary>
        WFTimer signupOkTimer = new WFTimer();

        /// <summary>
        /// 添加培训
        /// </summary>
        WFTimer goTrainAddTimer = new WFTimer();

        WFTimer trainAddClickedTimer = new WFTimer();

        /// <summary>
        /// 点击保存
        /// </summary>
        WFTimer trainSaveTimer = new WFTimer();
        #endregion

        #region 局部变量

        /// <summary>
        /// 第一次执行需要填写信息，后续的执行无需填写自动执行
        /// </summary>
        private int i = 0;

        #endregion
    }
}
