using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPF_XYHIS_OA_TOOLS.Common;
using WPF_XYHIS_OA_TOOLS.Config;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TaskManage : MetroWindow
    {

        #region 计时器

        /// <summary>
        /// 登入成功后执行的计时器
        /// </summary>
        WFTimer signupOkTimer = new WFTimer();

        /// <summary>
        /// 初始化界面数据计时器
        /// </summary>
        WFTimer initPageDataInfoTimer = new WFTimer();

        /// <summary>
        /// 筛选任务成功后执行的计时器
        /// </summary>
        WFTimer taskSelectTimer = new WFTimer();

        /// <summary>
        /// 执行任务
        /// </summary>
        WFTimer goTaskTimer = new WFTimer();

        /// <summary>
        /// 执行完毕
        /// </summary>
        WFTimer goOverTimer = new WFTimer();

        /// <summary>
        /// 返回任务
        /// </summary>
        WFTimer goTaskReturnTimer = new WFTimer();

        #endregion

        public TaskManage()
        {
            InitializeComponent();
            tbBuckle.Text = "0.8";
            tbBuckle1.Text = "0.65";

            #region 初始化计时器

            signupOkTimer.SetTick(signupOkTimer_Tick);
            initPageDataInfoTimer.SetTick(initPageDataInfoTimer_Tick);
            taskSelectTimer.SetTick(taskSelectTimer_Tick);
            goTaskTimer.SetTick(goTaskTimer_Tick);
            goOverTimer.SetTick(goOverTimer_Tick);
            goTaskReturnTimer.SetTick(goTaskReturnTimer_Tick);

            #endregion

            SetProgressRing(true, "登入成功，正在获取 Xyhis.OA 的页面数据，请稍后...");
            signupOkTimer.Enabled = true;
        }

        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("啊哈？");
            }
        }

        /// <summary>
        /// 界面按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tclPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is RadioButton rbtn)
            {
                switch (rbtn.Name)
                {
                    case "rbtnCheck":
                        cbxTaskState.SelectedValue = "101";
                        cbxSubmitterName.SelectedValue = null;
                        break;
                    case "rbtnOk":
                        this.cbxSubmitterName.SelectedValue = Global.UserName;
                        cbxTaskState.SelectedValue = null;
                        break;
                }
            }
            else if (e.Source is Hyperlink hlk)
            {
                switch (hlk.Name)
                {
                    case "hlkUesRead":
                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "wordpad.exe";
                        cmd.StartInfo.Arguments = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\外部文件使用手册.odt";
                        cmd.Start();
                        break;
                }
            }
            else if (e.Source is Button btn)
            {
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
        }

        private void signupOkTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                signupOkTimer.Enabled = false;

                Global.wbXyhisOa.Url = new Uri("http://oa.xyhis.com/Task/TaskList.aspx");

                initPageDataInfoTimer.Enabled = true;
            }
        }

        private void initPageDataInfoTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                initPageDataInfoTimer.Enabled = false;

                InitPageDataInfoForXyhisOA();
            }
        }

        /// <summary>
        /// 获取XyhisOA的条件数据初始化界面控件
        /// </summary>
        private void InitPageDataInfoForXyhisOA()
        {
            try
            {
                #region 客户
                var ddlCustomer = WbHelper.GetHtmlElement("ddlCustomer");//客户

                var customerKeyValues = SplitGetKeyValues(ddlCustomer.InnerHtml);
                this.cbxClient.DisplayMemberPath = "Key";
                this.cbxClient.SelectedValuePath = "Value";
                this.cbxClient.ItemsSource = customerKeyValues;
                #endregion

                #region 任务类别
                var ddlTaskType = WbHelper.GetHtmlElement("ddlTaskType");//任务类别

                var taskTypeKeyValues = SplitGetKeyValues(ddlTaskType.InnerHtml);
                this.cbxTaskType.DisplayMemberPath = "Key";
                this.cbxTaskType.SelectedValuePath = "Value";
                this.cbxTaskType.ItemsSource = taskTypeKeyValues;
                #endregion

                #region 提交人
                var ddlSubmitterName = WbHelper.GetHtmlElement("ddlSubmitterName");//提交人

                var submitterNameKeyValues = SplitGetKeyValues(ddlSubmitterName.InnerHtml);
                this.cbxSubmitterName.DisplayMemberPath = "Key";
                this.cbxSubmitterName.SelectedValuePath = "Value";
                this.cbxSubmitterName.ItemsSource = submitterNameKeyValues;
                #endregion

                #region 负责人
                var ddlInChargeManId = WbHelper.GetHtmlElement("ddlInChargeManId");//负责人

                var inChargeManIdKeyValues = SplitGetKeyValues(ddlInChargeManId.InnerHtml);
                this.cbxCharge.DisplayMemberPath = "Key";
                this.cbxCharge.SelectedValuePath = "Value";
                this.cbxCharge.ItemsSource = inChargeManIdKeyValues;
                #endregion

                #region 完成状态

                var ddlLsStatus = WbHelper.GetHtmlElement("ddlLsStatus");//完成状态
                var lsStatusKeyValues = SplitGetKeyValues(ddlLsStatus.InnerHtml);
                this.cbxTaskState.DisplayMemberPath = "Key";
                this.cbxTaskState.SelectedValuePath = "Value";
                this.cbxTaskState.ItemsSource = lsStatusKeyValues;

                #endregion

                #region 组长
                var ddlManager = WbHelper.GetHtmlElement("ddlManager");//组长
                var cbxManagerKeyValues = SplitGetKeyValues(ddlManager.InnerHtml);
                cbxManager.DisplayMemberPath = "Key";
                cbxManager.SelectedValuePath = "Value";
                cbxManager.ItemsSource = cbxManagerKeyValues;

                #endregion

                SetProgressRing(false);

            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("系统提示", $"程序发生了错误。【{ex.Message}】");
            }
        }

        private void BtnGoClick()
        {
            if (!Global.IsSignupOk)
            {
                this.ShowMessageAsync("系统提示", "您尚未登入，请登入后继续进行操作。");
                return;
            }

            goTaskType = rbtnCheck.IsChecked == true ? EnumGoTaskType.Check :
                rbtnOk.IsChecked == true ? EnumGoTaskType.Ok :
                rbtnReturn.IsChecked == true ? EnumGoTaskType.Return :
                EnumGoTaskType.Null;

            if (goTaskType == EnumGoTaskType.Null)
            {
                this.ShowMessageAsync("系统提示", "您尚未选择需要执行的操作，请选择需要执行的操作后继续进行操作。");
                return;
            }
            if (goTaskType == EnumGoTaskType.Return
                && (cbxClient.SelectedValue == null || cbxClient.SelectedValue.ToString() == "0"
                    || cbxTaskState.SelectedValue == null || cbxTaskState.SelectedValue.ToString() == "0"))
            {
                this.ShowMessageAsync("系统提示", "返回操作必须选择\"客户\"以及\"完成状态\"，请选择\"客户\"以及\"完成状态\"后继续进行操作。");
                return;
            }

            SetProgressRing(true, "正在进行中，请稍后...");
            fytMenu.IsOpen = false;
            btnGo.IsEnabled = false;
            fytOk.IsOpen = false;
            GoTask();
        }

        private void GoTask()
        {
            switch (goTaskType)
            {
                case EnumGoTaskType.Check:
                    SelectTaskCheck();
                    break;
                case EnumGoTaskType.Ok:
                    SelectTaskOk();
                    break;
                case EnumGoTaskType.Return:
                    SelectTaskReturn();
                    break;
            }

            taskSelectTimer.Enabled = true;
        }

        #region 筛选任务审查、确认、返回

        private void SelectTaskCheck()
        {
            var ckbViewALL = WbHelper.GetHtmlElement("ckbViewALL");//一页显示所有
            ckbViewALL.SetAttribute("checked", "checked");

            var ddlLsStatus = WbHelper.GetHtmlElement("ddlLsStatus");//完成状态 待审查
            ddlLsStatus.SetAttribute("value", "101");

            var ddlTechnicalManager = WbHelper.GetHtmlElement("ddlTechnicalManager");//审查人
            ddlTechnicalManager.SetAttribute("value", Global.UserName);

            ScreenTaskWhere();

            var btnSelect = WbHelper.GetHtmlElement("btnSelect");//查询
            btnSelect.InvokeMember("click");
        }

        private void SelectTaskOk()
        {
            var ckbViewALL = WbHelper.GetHtmlElement("ckbViewALL");//一页显示所有
            ckbViewALL.SetAttribute("checked", "checked");

            var ddlLsStatus = WbHelper.GetHtmlElement("ddlLsStatus");//完成状态 待确认
            ddlLsStatus.SetAttribute("value", "102");

            //var ddlSubmitterName = WbHelper.GetHtmlElement("ddlSubmitterName");//提交人
            //ddlSubmitterName.SetAttribute("value", Global.UserName);

            ScreenTaskWhere();

            var btnSelect = WbHelper.GetHtmlElement("btnSelect");//查询
            btnSelect.InvokeMember("click");
        }

        private void SelectTaskReturn()
        {
            var ckbViewALL = WbHelper.GetHtmlElement("ckbViewALL");//一页显示所有
            ckbViewALL.SetAttribute("checked", "checked");

            ScreenTaskWhere();

            var btnSelect = WbHelper.GetHtmlElement("btnSelect");//查询
            btnSelect.InvokeMember("click");
        }

        #endregion

        private void taskSelectTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                taskSelectTimer.Enabled = false;
                GetAllTask();

                if (taskUris.Count != 0)
                {
                    Global.wbXyhisOa.Url = new Uri(taskUris[i]);
                    goTaskTimer.Enabled = true;
                }
                else
                {
                    GoOver();
                }
            }
        }

        /// <summary>
        /// 筛选查询任务条件
        /// </summary>
        private void ScreenTaskWhere()
        {
            if (cbxClient.SelectedValue != null && cbxClient.SelectedValue.ToString() != "0")//客户
            {
                var ddlCustomer = WbHelper.GetHtmlElement("ddlCustomer");
                ddlCustomer.SetAttribute("value", cbxClient.SelectedValue.ToString());
            }
            if (cbxTaskType.SelectedValue != null && cbxTaskType.SelectedValue.ToString() != "0")//任务类别
            {
                var ddlTaskType = WbHelper.GetHtmlElement("ddlTaskType");
                ddlTaskType.SetAttribute("value", cbxTaskType.SelectedValue.ToString());
            }
            if (goTaskType != EnumGoTaskType.Return)
            {
                if (cbxSubmitterName.SelectedValue != null && cbxSubmitterName.SelectedValue.ToString() != "0")//提交人
                {
                    var ddlSubmitterName = WbHelper.GetHtmlElement("ddlSubmitterName");
                    ddlSubmitterName.SetAttribute("value", cbxSubmitterName.SelectedValue.ToString());
                }
            }
            if (cbxCharge.SelectedValue != null && cbxCharge.SelectedValue.ToString() != "0")//负责人
            {
                var ddlInChargeManId = WbHelper.GetHtmlElement("ddlInChargeManId");
                ddlInChargeManId.SetAttribute("value", cbxCharge.SelectedValue.ToString());
            }
            if (cbxManager.SelectedValue != null && cbxManager.SelectedValue.ToString() != "0")//组长
            {
                var ddlManager = WbHelper.GetHtmlElement("ddlManager");
                ddlManager.SetAttribute("value", cbxManager.SelectedValue.ToString());
            }
            if (!string.IsNullOrWhiteSpace(tbItemQruey.Text))
            {
                var txtMemo = WbHelper.GetHtmlElement("txtMemo");
                txtMemo.SetAttribute("value", tbItemQruey.Text);
            }
            if (!string.IsNullOrWhiteSpace(dpkStartDate.Text))
            {
                var txtFinishTimeFact0 = WbHelper.GetHtmlElement("txtFinishTimeFact0");
                txtFinishTimeFact0.SetAttribute("value", dpkStartDate.Text.Replace("/", "-"));
            }
            if (!string.IsNullOrWhiteSpace(dpkEndDate.Text))
            {
                var txtFinishTimeFact1 = WbHelper.GetHtmlElement("txtFinishTimeFact1");
                txtFinishTimeFact1.SetAttribute("value", dpkEndDate.Text.Replace("/", "-"));
            }
            switch (goTaskType)
            {
                case EnumGoTaskType.Return:
                    if (cbxTaskState.SelectedValue != null && cbxTaskState.SelectedValue.ToString() != "0")//完成状态
                    {
                        var ddlLsStatus = WbHelper.GetHtmlElement("ddlLsStatus");
                        ddlLsStatus.SetAttribute("value", cbxTaskState.SelectedValue.ToString());
                    }
                    break;
            }
        }

        bool part_HtmlElement = true;
        /// <summary>
        /// 获取所有任务
        /// </summary>
        private void GetAllTask()
        {
            string chstr = "";
            if (cbFileText.IsChecked == true)
            {
                var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\text.txt";
                chstr = System.IO.File.ReadAllText(path, System.Text.Encoding.Default);
                if (string.IsNullOrWhiteSpace(chstr))
                    return;
                System.IO.File.Delete(path);
                System.IO.File.Create(path);
                part_HtmlElement = false;
            }
            else
            {
                var gridView1 = WbHelper.GetHtmlElement("GridView1");
                if (gridView1 == null)//没有任务
                    return;
                //获取审核任务的ID
                chstr = gridView1.InnerHtml;
                part_HtmlElement = true;
            }

            while (true)
            {
                string t = "";
                if (part_HtmlElement)
                {
                    t = Units.GetValue(chstr, "ID=", "\"");
                    chstr = chstr.Remove(0, chstr.IndexOf($"ID={t}\"") + $"ID={t}\"".Length);//删除已截取的ID部分代码以下次循环截取ID
                }
                else
                {
                    t = Units.GetValue(chstr, "ID=", "'");
                    chstr = chstr.Remove(0, chstr.IndexOf($"ID={t}'") + $"ID={t}'".Length);//删除已截取的ID部分代码以下次循环截取ID
                }
                if (string.IsNullOrWhiteSpace(t))
                    break;
                taskUris.Add("http://oa.xyhis.com/Task/AddTask.aspx?IsClean=true&ID=" + t);
            }
            taskUris = taskUris.Distinct().ToList();
        }

        private void goTaskTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                goTaskTimer.Enabled = false;

                SetProgressRing(true, $"正在进行第{i + 1}/{taskUris.Count}项任务。");

                if (!string.IsNullOrWhiteSpace(tbNoConTaskName.Text))
                {
                    var divDescr = WbHelper.GetHtmlElement("divDescr");
                    var v = divDescr.InnerText;
                    if (v.Contains(tbNoConTaskName.Text))
                    {
                        goOverTimer.Enabled = true;
                        return;
                    }
                }

                switch (goTaskType)
                {
                    case EnumGoTaskType.Check:
                        GoTaskCheck();
                        goOverTimer.Enabled = true;
                        break;
                    case EnumGoTaskType.Ok:
                        GoTaskOk();
                        goOverTimer.Enabled = true;
                        break;
                    case EnumGoTaskType.Return:
                        GoTaskReturn();
                        break;
                }

            }
        }

        #region 执行任务审查、确认、返回

        private void GoTaskCheck()
        {
            var ddlInChargeManId = WbHelper.GetHtmlElement("ddlInChargeManId");//负责人
            var inChargeManId = ddlInChargeManId.GetAttribute("value");

            var ddlTaskType = WbHelper.GetHtmlElement("ddlTaskType");//任务类型
            string taskType = ddlTaskType.GetAttribute("value");

            var txtEstimateHours = WbHelper.GetHtmlElement("txtEstimateHours");//预计工时
            string estimateHours = txtEstimateHours.GetAttribute("value");

            var txtCodeRows = WbHelper.GetHtmlElement("txtCodeRows");//代码行数
            string inputCodeRowNumber = txtCodeRows.GetAttribute("value");
            string codeRowNumber = string.IsNullOrWhiteSpace(inputCodeRowNumber) ? "0" : inputCodeRowNumber;

            var txtAdminHours1 = WbHelper.GetHtmlElement("txtAdminHours1");//工时1
            string adminHours1 = txtAdminHours1.GetAttribute("value");

            var txtAdminHours2 = WbHelper.GetHtmlElement("txtAdminHours2");//工时2
            string adminHours2 = txtAdminHours2.GetAttribute("value");

            var txtAdminHours3 = WbHelper.GetHtmlElement("txtAdminHours3");//工时3
            string adminHours3 = txtAdminHours3.GetAttribute("value");

            var txtCodeQuality = WbHelper.GetHtmlElement("txtCodeQuality");//代码质量
            txtCodeQuality.SetAttribute("value", "已完成");

            var txtCodeReviewRows = WbHelper.GetHtmlElement("txtCodeReviewRows");//确认代码行数
            txtCodeReviewRows.SetAttribute("value", codeRowNumber);

            string strCodeReviewHour = "0";
            if (taskType != "5")//反工任务
            {
                if (this.rbtnDefaultReviewHour.IsChecked == true)
                {
                    float codeReviewHour = GetCodeReviewHour(taskType, adminHours1, adminHours2, adminHours3, estimateHours);
                    strCodeReviewHour = CodeReviewHourRounding(codeReviewHour);
                }
                else if (this.rbtnMaxReviewHour.IsChecked == true)
                {
                    float intAdminHours1 = 0;
                    float.TryParse(adminHours1, out intAdminHours1);
                    float intAdminHours2 = 0;
                    float.TryParse(adminHours2, out intAdminHours2);
                    float intAdminHours3 = 0;
                    float.TryParse(adminHours3, out intAdminHours3);

                    float adminHours = intAdminHours1 + intAdminHours2 + intAdminHours3;//自评工时
                    strCodeReviewHour = adminHours.ToString();
                }
                //else if (this.rbtnMinReviewHour.IsChecked == true)
                //{

                //}
            }

            var txtCodeReviewHour = WbHelper.GetHtmlElement("txtCodeReviewHour");//建议工时
            txtCodeReviewHour.SetAttribute("value", strCodeReviewHour);

            string codeLinkUpHour = "0.1";
            if (taskType == "4")
                codeLinkUpHour = "0.3";
            var txtCodeLinkUpHour = WbHelper.GetHtmlElement("txtCodeLinkUpHour");//审核工时
            txtCodeLinkUpHour.SetAttribute("value", codeLinkUpHour);

            var btnSH = WbHelper.GetHtmlElement("Task1_btnCodeReview");//审核
            btnSH.InvokeMember("click");
        }

        private void GoTaskOk()
        {
            #region 工时

            //var txtAdminHours1 = WbHelper.GetHtmlElement("txtAdminHours1");//工时1
            //string adminHours1 = txtAdminHours1.GetAttribute("value");

            //var txtAdminHours2 = WbHelper.GetHtmlElement("txtAdminHours2");//工时2
            //string adminHours2 = txtAdminHours2.GetAttribute("value");

            //var txtAdminHours3 = WbHelper.GetHtmlElement("txtAdminHours3");//工时3
            //string adminHours3 = txtAdminHours3.GetAttribute("value");

            //float intAdminHours1 = 0;
            //float.TryParse(adminHours1, out intAdminHours1);
            //float intAdminHours2 = 0;
            //float.TryParse(adminHours2, out intAdminHours2);
            //float intAdminHours3 = 0;
            //float.TryParse(adminHours3, out intAdminHours3);

            //float intAdminHoursCount = intAdminHours1 + intAdminHours2 + intAdminHours3;

            //var cbIsCrossDay = WbHelper.GetHtmlElement("cbIsCrossDay");
            //string isCrossDay = cbIsCrossDay.GetAttribute("checked");
            //if (intAdminHoursCount > 8 && isCrossDay != "checked")
            //{
            //    GoTaskReturn();
            //    return;
            //}

            #endregion

            var txtTestHours = WbHelper.GetHtmlElement("txtTestHours");//测试工时
            txtTestHours.SetAttribute("value", "0.1");

            var btnIsOk = WbHelper.GetHtmlElement("Task1_btnIsOk");//确认完成
            btnIsOk.InvokeMember("click");
        }

        private void GoTaskReturn()
        {
            //var btnUpdate = WbHelper.GetHtmlElement("btnReworkByCodeReview");//修改
            //btnUpdate.InvokeMember("click");

            var srbcr = Global.wbXyhisOa.Document.InvokeScript("SaveReworkByCodeReview");

            goTaskReturnTimer.Enabled = true;
        }

        #endregion

        /// <summary>
        /// 获取建议工时
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="adminHours1">自评工时1</param>
        /// <param name="adminHours2">自评工时2</param>
        /// <param name="adminHours3">自评工时3</param>
        /// <param name="estimateHours">建议工时</param>
        /// <returns></returns>
        private float GetCodeReviewHour(string taskType, string adminHours1, string adminHours2, string adminHours3, string estimateHours)
        {
            float codeReviewHour = 0.0f;
            float intAdminHours1 = 0;
            float.TryParse(adminHours1, out intAdminHours1);
            float intAdminHours2 = 0;
            float.TryParse(adminHours2, out intAdminHours2);
            float intAdminHours3 = 0;
            float.TryParse(adminHours3, out intAdminHours3);

            float adminHours = intAdminHours1 + intAdminHours2 + intAdminHours3;//自评工时

            float cout = 0f;//计算工时
            float.TryParse(estimateHours, out float intEstimateHours);//建议工时
            //if (intEstimateHours == 0 || intEstimateHours > adminHours)
            //    cout = adminHours;
            //else
            //    cout = intEstimateHours;
            cout = adminHours;

            if (adminHours > 5)
            {
                var f = 0f;
                if (string.IsNullOrWhiteSpace(tbBuckle.Text))
                    f = 0.8f;
                else
                    float.TryParse(tbBuckle.Text, out f);
                codeReviewHour = cout * f;
            }
            else
            {
                var f = 0f;
                if (string.IsNullOrWhiteSpace(tbBuckle1.Text))
                    f = 0.6f;
                else
                    float.TryParse(tbBuckle.Text, out f);
                codeReviewHour = cout * f;
            }

            switch (taskType)
            {
                case "3"://疑难杂症
                    codeReviewHour = codeReviewHour + adminHours * 0.8f;
                    break;
                case "4"://新需求开发
                    codeReviewHour = codeReviewHour + adminHours * 0.6f;
                    break;
                case "99"://其他
                    codeReviewHour = codeReviewHour + adminHours * 0.10f;
                    break;
            }

            if (codeReviewHour <= 0.1)
                codeReviewHour = 0.1f;

            if (codeReviewHour > adminHours)//当建议工时比自评工时大时
                codeReviewHour = adminHours * 0.65f;
            return codeReviewHour;

        }

        private void goTaskReturnTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                goTaskReturnTimer.Enabled = false;

                //var ddlLsStatus = WbHelper.GetHtmlElement("Task1_ddlLsStatus");//完成状态
                //ddlLsStatus.SetAttribute("value", "2");//已分配

                mshtml.IHTMLElement hTML = null;
                string str = "";
                System.Windows.Forms.HtmlElementCollection hecs = ((System.Windows.Forms.HtmlDocument)Global.wbXyhisOa.Document).GetElementsByTagName("input");
                foreach (System.Windows.Forms.HtmlElement item in hecs)
                {
                    mshtml.IHTMLElement ele = (mshtml.IHTMLElement)item.DomElement;
                    str += ele.className == null ? "" : ele.className + ",";
                    if (ele.className == "btnStyle1 btnStyle2")
                    {
                        hTML = ele;
                    }
                }
                if (hTML != null)
                    hTML.click();

                System.Windows.Forms.HtmlElementCollection hecs2 = Global.wbXyhisOa.Document.GetElementsByTagName("textarea");


                var txtResolveWay3 = WbHelper.GetHtmlElement("layui-layer-input");//任务描述
                var trwv = txtResolveWay3.GetAttribute("value");
                txtResolveWay3.SetAttribute("value", trwv + "\\\\依然存在此问题");

                var btnSaver = WbHelper.GetHtmlElement("layui-layer-btn0");//保存
                btnSaver.InvokeMember("click");

                goOverTimer.Enabled = true;
            }
        }

        private void goOverTimer_Tick(object sender, EventArgs e)
        {
            if (WbHelper.WbXyhisOaIsBusy())
            {
                goOverTimer.Enabled = false;
                SetProgressRing(true, $"第{i + 1}/{taskUris.Count}项任务已执行完成。");
                i++;
                if (i < taskUris.Count)
                {
                    Global.wbXyhisOa.Url = new Uri(taskUris[i]);
                    goTaskTimer.Enabled = true;
                }
                else
                {
                    GoOver();
                }
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

        /// <summary>
        /// 分割<Select></Select>标签的代码获取Key和Value
        /// </summary>
        /// <param name="innerHtml"></param>
        /// <returns></returns>
        private List<KeyValue> SplitGetKeyValues(string innerHtml)
        {
            string[] sArray = Regex.Split(innerHtml, "</OPTION>", RegexOptions.IgnoreCase);

            List<KeyValue> keyValues = new List<KeyValue>();
            foreach (var item in sArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    KeyValue keyValue = new KeyValue
                    {
                        Key = Regex.Split(item, ">", RegexOptions.IgnoreCase)[1],
                        Value = Units.GetValue(item, "value=", ">")
                    };
                    keyValues.Add(keyValue);
                }
            }
            return keyValues;
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="codeReviewHour"></param>
        /// <returns></returns>
        private string CodeReviewHourRounding(float codeReviewHour)
        {
            var pf = "0.00";
            if (codeReviewHour != 0)
            {
                var fs = codeReviewHour.ToString();
                if (fs.Length > 1)
                {
                    var fss = fs.Split(new char[] { '.' });

                    var fl = fss[1];
                    var flf = int.Parse(fl[0].ToString());
                    pf = $"{fss[0]}.";
                    if (flf < 3)
                        if (int.Parse(fss[0].ToString()) != 0)
                            pf = $"{pf}00";
                        else
                            pf = $"{pf}10";
                    else if (flf >= 3 && flf < 5)
                        pf = $"{pf}30";
                    else if (flf >= 5 && flf < 8)
                        pf = $"{pf}50";
                    else
                        pf = $"{pf}50";
                }
                else
                {
                    pf = codeReviewHour.ToString();
                }

                //if (fl.Length > 1)
                //{
                //    if (fss.Count() == 2)
                //    {
                //        var fll = int.Parse(fl[1].ToString());
                //        pf = $"{fss[0]}.{fl[0]}";
                //        if (fll <= 3)
                //        {
                //            pf = $"{pf}0";
                //        }
                //        else if (fll > 3 && fll <= 7)
                //        {
                //            pf = $"{pf}5";
                //        }
                //        else
                //        {
                //            pf = $"{pf}5";
                //        }
                //    }
                //    else
                //    {
                //        pf = tbi.ToString();
                //    }
                //}
                //else
                //{
                //    pf = tbi.ToString();
                //}
            }
            return pf;
        }

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

        #region 局部变量

        /// <summary>
        /// 执行任务类型
        /// </summary>
        private EnumGoTaskType goTaskType = EnumGoTaskType.Null;

        /// <summary>
        /// 任务的链接地址
        /// </summary>
        private List<string> taskUris = new List<string>();

        /// <summary>
        /// 当前执行的第i个任务
        /// </summary>
        private int i = 0;

        #endregion
    }

    public sealed class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public enum EnumGoTaskType
    {
        /// <summary>
        /// 无
        /// </summary>
        Null = 0,
        /// <summary>
        /// 审查
        /// </summary>
        Check = 1,
        /// <summary>
        /// 确认
        /// </summary>
        Ok = 2,
        /// <summary>
        /// 返回
        /// </summary>
        Return = 3
    }
}
