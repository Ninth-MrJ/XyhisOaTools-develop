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

namespace WPF_XYHIS_OA_TOOLS
{
    /// <summary>
    /// UserSignup.xaml 的交互逻辑
    /// </summary>
    public partial class UserSignup : UserControl
    {
        public UserSignup()
        {
            InitializeComponent();

            //tbUserName.Focus();
        }

        public string UserName
        {
            get { return this.tbUserName.Text; }
        }

        public string UserPwd
        {
            get { return this.tbUserPwd.Password; }
        }

        public void SetProgressRing(bool isActive)
        {
            if (isActive)
            {
                prgLoding.IsActive = true;
            }
            else
            {
                prgLoding.IsActive = false;
            }
        }
    }
}
