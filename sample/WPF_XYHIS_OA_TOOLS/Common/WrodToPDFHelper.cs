using System;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    public static class WrodToPDFHelper
    {
        /// <summary>
        /// Office COM接口 将 Word 文档转换为 PDF
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static bool OfficeWordToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;
            try
            {
                application.Visible = false;
                document = application.Documents.Open(sourcePath);
                document.ExportAsFixedFormat(targetPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                document.Close();
            }
            return result;
        }
    }
}
