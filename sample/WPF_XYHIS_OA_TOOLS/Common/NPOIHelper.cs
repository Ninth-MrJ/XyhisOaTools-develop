using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    /// <summary>
    /// NPOI 读取 Office 文件
    /// </summary>
    public static class NPOIHelper
    {
        public static List<string[]> ReadExcel(string fileName)
        {
            IWorkbook workbook = null;//新建IWorkbook对象
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var fileExtension = Path.GetExtension(fileName).ToLower();
                    if (fileExtension.IndexOf("xlsx") > 0)//2007版本
                        workbook = new XSSFWorkbook(fileStream);//xlsx数据读入workbook
                    else if (fileExtension.IndexOf("xls") > 0)//2003版本
                        workbook = new HSSFWorkbook(fileStream);//xls数据读入workbook
                    else
                        return new List<string[]>();
                    ISheet sheet = workbook.GetSheetAt(0);//获取第一个工作表
                    IRow row;//新建当前工作表行数据
                    var vals = new List<string[]>();
                    for (int i = 1; i <= sheet.LastRowNum; i++)//对工作表每一行
                    {
                        row = sheet.GetRow(i);//row读入第i行数据
                        if (row != null)
                        {
                            var val = new string[row.LastCellNum];
                            for (int j = 0; j < row.LastCellNum; j++)//对工作表每一列
                            {
                                string cellValue = row.GetCell(j).ToString();//获取i行j列数据

                                val[j] = cellValue;
                            }
                            vals.Add(val);
                        }
                    }
                    return vals;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                workbook.Close();
            }
        }
    }
}
