using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public static class Units
    {
        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        public static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }
            return 0;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        public static string GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            string[] vs = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return vs[(int)dayOfWeek];
        }


        public static string GetFile(string filter, string title = "请选择文件")
        {
            var s = SelectFile(filter, title, false);
            if (s == null)
                return "";
            return (string)s;
        }

        public static string[] GetFiles(string filter, string title = "请选择文件")
        {
            var s = SelectFile(filter, title, true);
            if (s == null)
                return new string[0];
            return (string[])s;
        }

        public static object SelectFile(string filter, string title, bool isMultiselect)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = isMultiselect;//该值确定是否可以选择多个文件
            dialog.Title = title;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                if (isMultiselect)
                {
                    string[] files = dialog.FileNames;
                    return files;
                }
                else
                {
                    string file = dialog.FileName;
                    return file;
                }
            }
            return null;
        }

        public static string SaveFile(/*string filter, string title = "请选择保存的文件夹"*/)
        {
            //Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            //dialog.Title = title;
            //dialog.Filter = filter;
            //dialog.RestoreDirectory = true;
            //if (dialog.ShowDialog() == true)
            //{
            //    return dialog.FileName;
            //}
            //return "";

            string path = string.Empty;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fbd.SelectedPath;
            }
            return path;
        }

        /// <summary>
        /// 分割 Html <Select></Select> 标签的代码获取Key和Value
        /// </summary>
        /// <param name="innerHtml"></param>
        /// <returns></returns>
        public static List<KeyValue> SplitHtmlSelectDom(string innerHtml)
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
    }
}
