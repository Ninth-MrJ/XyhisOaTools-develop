using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntPtrCommon
{
    class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var type = args[0];
            switch (type)
            {
                case "WebSelectFile":
                    var fn = args[1];
                    App app = new App();
                    WebSelectFile window = new WebSelectFile(fn);
                    app.Run();
                    break;
            }
        }
    }
}
