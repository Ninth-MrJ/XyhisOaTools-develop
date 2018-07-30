using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF = System.Windows.Forms;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public class WFWebBrowser : WF.WebBrowser
    {
        dynamic Iwb2;

        protected override void AttachInterfaces(object nativeActiveXObject)
        {
            Iwb2 = nativeActiveXObject;
            Iwb2.Silent = true;
            base.AttachInterfaces(nativeActiveXObject);
        }

        protected override void DetachInterfaces()
        {
            Iwb2 = null;
            base.DetachInterfaces();
        }

        public bool CheckIsBusy()
        {
            if (this.ReadyState == WF.WebBrowserReadyState.Complete && !this.IsBusy)
                return true;
            return false;
        }
    }
}
