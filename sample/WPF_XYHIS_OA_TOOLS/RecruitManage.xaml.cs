using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WPF_XYHIS_OA_TOOLS.Common;
using WPF_XYHIS_OA_TOOLS.Config;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// RecruitManage.xaml 的交互逻辑
    /// </summary>
    public partial class RecruitManage : MetroWindow
    {
        public RecruitManage()
        {
            InitializeComponent();

            #region 初始化计时器

            signupOkTimer.SetTick(signupOkTimer_Tick);
            btnGoTimer.SetTick(btnGoTimer_Tick);
            goOverTimer.SetTick(goOverTimer_Tick);
            intPteTimer.SetTick(intPteTimer_Tick);

            #endregion

            SetProgressRing(true, "登入成功，正在获取 Xyhis.OA 的页面数据，请稍后...");
            Global.wbXyhisOa.Url = new Uri("https://oa.xyhis.com/Zhaopin/hrCandidates.aspx");
            signupOkTimer.Open();
        }

        private void signupOkTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                signupOkTimer.Close();
                var recruitmentChannels = WbHelper.GetHtmlElement("RecruitmentChannels");
                if (recruitmentChannels != null)
                {
                    var rcKeyValues = Units.SplitHtmlSelectDom(recruitmentChannels.InnerHtml);
                    this.cbxRecruit.DisplayMemberPath = "Key";
                    this.cbxRecruit.SelectedValuePath = "Value";
                    this.cbxRecruit.ItemsSource = rcKeyValues;
                    this.cbxRecruit.SelectedValue = "51Job";
                    isPower = true;
                }
                SetProgressRing(false);
            }
        }

        /// <summary>
        /// 没有权限，关闭界面
        /// </summary>
        private async void NullAsync()
        {
            MessageDialogResult result = await this.ShowMessageAsync("系统提示", "您无权操作此项。");
            if (result == MessageDialogResult.Affirmative)
            {
                //this.Close();
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
                    fytMenu.IsOpen = true;
                    break;
            }
        }

        private void BtnGoClick()
        {
            if (!isPower)
            {
                NullAsync();
                return;
            }

            //var file = Units.GetFile("文件(*.xls;*.xlsx;*.pdf)|*.xls;*.xlsx;*.pdf");
            var files = Units.GetFiles("PDF文档(*.pdf)|*.pdf");
            //if (string.IsNullOrWhiteSpace(file))
            if (files.Length == 0)
            {
                //this.ShowMessageAsync("系统提示", "您尚未选择文件，请选择文件后执行操作。");
                return;
            }
            fytMenu.IsOpen = false;
            btnGo.IsEnabled = false;
            SetProgressRing(true, "操作进行中，请不要在操作过程中删除、移动或打开文件，否则都有可能导致出现错误，请稍后...");
            try
            {
                switch (".pdf")
                {
                    case ".pdf":
                        goRecruitType = EnumGoRecruitType.PDF;
                        for (int i = 0; i < files.Length; i++)
                        {
                            ReadPDF(files[i]);
                        }
                        btnGoTimer.Open();
                        break;
                    //case ".xls":
                    //case ".xlsx":
                    //    goRecruitType = EnumGoRecruitType.EXCEL;
                    //    recruits = NPOIHelper.ReadExcel(file).FindAll(t => !string.IsNullOrWhiteSpace(t[0]));//名字不为空的数据
                    //    btnGoTimer.Open();
                    //    break;
                }
            }
            catch (Exception)
            {
                this.ShowMessageAsync("系统提示", "操作文件出现错误，请确保在操作过程中没有删除、移动或打开文件，否则都有可能导致出现错误。");
                SetProgressRing(false);
            }
        }

        private void ReadPDF(string file)
        {
            string pdfContent = ITextSharpHelper.OnCreated(file);
            var strs = Regex.Split(pdfContent, "\n");
            //由于51job导出的PDF文档格式存在不同，故分两次解析
            try
            {
                if (strs[0].Contains("更 新 时 间"))
                {
                    //1.51段格式PDF
                    Recruit recruit = new Recruit
                    {
                        Name = RemoveSpace(strs[1]),
                        Phone = RemoveSpace(strs[6]),
                        GraduateInstitutions = RemoveSpace(GetColonReghtValue(strs[16])),
                        Specialty = RemoveSpace(GetColonReghtValue(strs[15])),
                        Mail = RemoveSpace(strs[8]),
                        Sex = strs[9][1].ToString(),
                        FilePath = file
                    };
                    recruitList.Add(recruit);
                    //SetHtmlValue(recruit, true, file);
                }
                else if (strs[0].Contains("应 聘 职 位"))
                {
                    //2.51长格式PDF
                    Recruit recruit = new Recruit
                    {
                        Name = RemoveSpace(strs[5]),
                        Phone = RemoveSpace(strs[10]),
                        GraduateInstitutions = RemoveSpace(GetColonReghtValue(strs[20])),
                        Specialty = RemoveSpace(GetColonReghtValue(strs[19])),
                        Mail = RemoveSpace(strs[12]),
                        Sex = strs[13][1].ToString(),
                        FilePath = file
                    };
                    recruitList.Add(recruit);
                    //SetHtmlValue(recruit, true, file);
                }
                else
                {
                    this.ShowMessageAsync("系统提示", "这是一个格式错误的报错，请确保使用的为51job导出的PDF文档，如果已确认文档正确请联系开发员。");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnGoTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                btnGoTimer.Close();

                //var item = recruits[recruiti];
                //Recruit recruit = new Recruit
                //{
                //    Name = item[0],
                //    Phone = item[15],
                //    GraduateInstitutions = item[13],
                //    Specialty = item[14],
                //    Mail = item[16],
                //    Sex = item[7],
                //};

                SetHtmlValue(recruitList[recruiti], true, recruitList[recruiti].FilePath);
            }
        }

        private void SetHtmlValue(Recruit recruit, bool openFile = false, string fileName = "")
        {
            var tbName = WbHelper.GetHtmlElement("Name");
            tbName.SetAttribute("value", recruit.Name);
            var tbPhone = WbHelper.GetHtmlElement("Phone");
            tbPhone.SetAttribute("value", recruit.Phone);
            var tbGraduateInstitutions = WbHelper.GetHtmlElement("GraduateInstitutions");
            tbGraduateInstitutions.SetAttribute("value", recruit.GraduateInstitutions);
            var tbSpecialty = WbHelper.GetHtmlElement("Specialty");
            tbSpecialty.SetAttribute("value", recruit.Specialty);
            var tbMail = WbHelper.GetHtmlElement("Mail");
            tbMail.SetAttribute("value", recruit.Mail);
            var cbSex = WbHelper.GetHtmlElement("Sex");
            cbSex.SetAttribute("value", recruit.Sex);
            var tbRecruitmentChannels = WbHelper.GetHtmlElement("RecruitmentChannels");
            tbRecruitmentChannels.SetAttribute("value", this.cbxRecruit.SelectedValue.ToString());

            if (openFile)
            {
                IntPtrHelper.WebSelectFile(fileName);

                var fileUpload = WbHelper.GetHtmlElement("FileUpload1");
                fileUpload.InvokeMember("click");

                intPteTimer.Open();
            }
            else
            {
                BtnGoSave();
            }
        }

        /// <summary>
        /// 句柄查询窗体并赋值确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void intPteTimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] processArray = System.Diagnostics.Process.GetProcessesByName("IntPtrCommon");
            if (processArray.Length == 0)
            {
                intPteTimer.Close();

                BtnGoSave();
            }
        }

        private void BtnGoSave()
        {
            var btnSaver = WbHelper.GetHtmlElement("btnSaver");
            btnSaver.InvokeMember("click");

            goOverTimer.Open();
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

        private void goOverTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                goOverTimer.Close();
                recruiti++;
                if (recruiti < recruitList.Count())
                {
                    btnGoTimer.Open();
                }
                else
                {
                    GoOver();
                }
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

        private string RemoveSpace(string val)
        {
            var rs = val.Replace(" ", "");
            return rs;
        }

        private string GetColonReghtValue(string val)
        {
            var s = val.Split('：');
            if (s.Count() > 1)
                return s[1];
            return val;
        }

        #endregion

        #region 计时器

        WFTimer signupOkTimer = new WFTimer();

        WFTimer btnGoTimer = new WFTimer();

        WFTimer goOverTimer = new WFTimer();

        WFTimer intPteTimer = new WFTimer();


        #endregion

        #region 局部变量

        private bool isPower = false;

        private EnumGoRecruitType goRecruitType = EnumGoRecruitType.PDF;

        private int recruiti = 0;

        private List<string[]> recruits = new List<string[]>();
        private List<Recruit> recruitList = new List<Recruit>();

        #endregion
    }

    public enum EnumGoRecruitType
    {
        PDF,
        EXCEL
    }

    public class Recruit
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string GraduateInstitutions { get; set; }
        public string Specialty { get; set; }
        public string Mail { get; set; }
        public string Sex { get; set; }
        public string FilePath { get; internal set; }
    }
}
