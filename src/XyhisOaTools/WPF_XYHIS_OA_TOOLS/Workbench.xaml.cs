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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_XYHIS_OA_TOOLS.Common;

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// Workbench.xaml 的交互逻辑
    /// </summary>
    public partial class Workbench : UserControl
    {
        public Workbench()
        {
            InitializeComponent();

            //GetToDay();
        }

        //private void GetToDay()
        //{
        //    var nowDate = DateTime.Now;
        //    ctlKeepLog.Count = nowDate.Day.ToString();
        //    ctlKeepLog.Content = Units.GetDayOfWeek(nowDate.DayOfWeek);
        //}
    }
}
