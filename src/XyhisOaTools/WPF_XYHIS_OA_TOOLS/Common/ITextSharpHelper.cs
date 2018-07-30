using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XYHIS_OA_TOOLS.Common
{
    /// <summary>
    /// iTextSharp 读取 PDF 文件
    /// </summary>
    public static class ITextSharpHelper
    {
        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string OnCreated(string filePath)
        {
            try
            {
                string pdffilename = filePath;
                PdfReader pdfReader = new PdfReader(pdffilename);
                int numberOfPages = pdfReader.NumberOfPages;
                string text = string.Empty;

                for (int i = 1; i <= numberOfPages; ++i)
                {
                    iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                    text += iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i, strategy);
                }
                pdfReader.Close();

                return text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="pdfFile"></param>
        public static void ExtractImage(string pdfFile)
        {
            PdfReader pdfReader = new PdfReader(pdfFile);
            for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
            {
                PdfReader pdf = new PdfReader(pdfFile);
                PdfDictionary pg = pdf.GetPageN(pageNumber);
                PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

                try
                {
                    foreach (PdfName name in xobj.Keys)
                    {
                        PdfObject obj = xobj.Get(name);
                        if (obj.IsIndirect())
                        {
                            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                            string width = tg.Get(PdfName.WIDTH).ToString();
                            string height = tg.Get(PdfName.HEIGHT).ToString();
                            //ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject((GraphicsState)new Matrix(float.Parse(width), float.Parse(height)), (PRIndirectReference)obj, tg);
                            ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="renderInfo"></param>
        /// <param name="savePath"></param>
        public static void RenderImage(ImageRenderInfo renderInfo, string savePath)
        {
            PdfImageObject image = renderInfo.GetImage();
            using (Image dotnetImg = image.GetDrawingImage())
            {
                if (dotnetImg != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        dotnetImg.Save(ms, ImageFormat.Tiff);
                        Bitmap d = new Bitmap(dotnetImg);
                        d.Save(savePath);
                    }
                }

            }

        }
    }
}
