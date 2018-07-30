using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPF_XYHIS_OA_TOOLS.Common;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// WrodToPDFTool.xaml 的交互逻辑
    /// </summary>
    public partial class WrodToPDFTool : MetroWindow
    {
        public WrodToPDFTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var fileNames = Units.GetFiles("Wrod文件(*.doc;*.docx)|*.doc;*.docx");
            if (fileNames.Length == 0)
                return;
            List<ToolsFileInfo> toolsFileInfos = new List<ToolsFileInfo>();
            for (int i = 0; i < fileNames.Length; i++)
            {
                ToolsFileInfo toolsFileInfo = new ToolsFileInfo
                {
                    IsSelected = true,
                    FileName = System.IO.Path.GetFileNameWithoutExtension(fileNames[i]),
                    FilePath = fileNames[i]
                };
                toolsFileInfos.Add(toolsFileInfo);
            }
            this.dgFileInfo.ItemsSource = toolsFileInfos;
        }

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var savePath = Units.SaveFile();
            if (string.IsNullOrWhiteSpace(savePath))
                return;
            var toolsFileInfos = (List<ToolsFileInfo>)this.dgFileInfo.ItemsSource;
            Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(delegate ()
                {
                    this.prgLoding.IsActive = true;
                    this.sPanel.IsEnabled = false;
                });
                foreach (var item in toolsFileInfos.FindAll(t => t.IsSelected))
                {
                    WrodToPDFHelper.OfficeWordToPDF(item.FilePath, savePath + "\\" + item.FileName);
                }
            }).ContinueWith((cw) =>
            {
                Dispatcher.Invoke(delegate ()
                {
                    this.ShowMessageAsync("系统提示", "恭喜，全部已完成。");
                    this.dgFileInfo.ItemsSource = null;
                    this.prgLoding.IsActive = false;
                    this.sPanel.IsEnabled = true;
                    System.Diagnostics.Process.Start("explorer.exe", savePath);
                });
            });
        }
    }

    public class ToolsFileInfo
    {
        public bool IsSelected { get; set; } = true;
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
