using System;
using WF = System.Windows.Forms;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public sealed class WFTimer : WF.Timer
    {
        public WFTimer()
        {
            Interval = 200;
            Enabled = false;
        }

        public WFTimer(int interval)
        {
            Interval = interval;
            Enabled = false;
        }

        public void SetTick(EventHandler eventHandler)
        {
            this.Tick += eventHandler;
        }

        public void Open()
        {
            this.Enabled = true;
        }

        public void Close()
        {
            this.Enabled = false;
        }
    }
}
