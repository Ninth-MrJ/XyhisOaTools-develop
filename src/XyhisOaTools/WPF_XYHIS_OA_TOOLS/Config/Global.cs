using WPF_XYHIS_OA_TOOLS.Common;

namespace WPF_XYHIS_OA_TOOLS.Config
{
    public static class Global
    {
        public readonly static WFWebBrowser wbXyhisOa = new WFWebBrowser
        {
            ScriptErrorsSuppressed = true,
        };

        public static bool IsSignupOk { get; set; } = false;

        public static string UserName { get; set; }
    }
}
