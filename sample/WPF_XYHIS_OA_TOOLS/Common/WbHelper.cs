using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPF_XYHIS_OA_TOOLS.Config;
using WF = System.Windows.Forms;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public static class WbHelper
    {
        public static bool WbXyhisOaIsBusy()
        {
            if (Global.wbXyhisOa.ReadyState == WF.WebBrowserReadyState.Complete && Global.wbXyhisOa.IsBusy == false)
                return true;
            return false;
        }

        public static WF.HtmlElement GetHtmlElement(string elementId)
        {
            return ((WF.HtmlDocument)Global.wbXyhisOa.Document).All[elementId];
        }

        public static WF.HtmlElement GetHtmlElementByOuterHtml(string elementId, string selectName)
        {
            var aradios = WbHelper.GetHtmlElements(elementId);
            var aradio = aradios.First(t => t.OuterHtml.Contains(selectName));
            return aradio;
        }

        public static List<WF.HtmlElement> GetHtmlElements(string elementId)
        {
            List<WF.HtmlElement> htmls = new List<WF.HtmlElement>();

            WF.HtmlElementCollection elementCollection = ((WF.HtmlDocument)Global.wbXyhisOa.Document).All as WF.HtmlElementCollection;
            var elements = elementCollection.GetElementsByName(elementId);

            foreach (WF.HtmlElement item in elements)
            {
                htmls.Add(item);
            }

            return htmls;
        }
    }
}
