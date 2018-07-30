using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public static class IntPtrHelper
    {
        public static Process WebSelectFile(string v)
        {
            var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "IntPtrCommon.exe";
            var info = new System.Diagnostics.ProcessStartInfo(path);

            string type = "\"WebSelectFile\"";
            string fn = "\"" + v + "\"";
            info.Arguments = type + " " + fn;
            var sp = Process.Start(info);
            return sp;
        }
    }
}
