using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using iTextSharp.text;
using System.Web;
using System.Linq;
using iTextSharp.text.pdf;



using System.Web.Routing;

namespace ShoppingCart.GeneralLib.PdfPrinter  
{
    class PdfPrinterOnServer
    {
        
        public PdfPrinterOnServer()
        {
            
        }

        //FilePath => filepath + filename
        public void HTMLToPdf(string HTML, string FilePath, string filename)
        {
            Document document = new Document();

            PdfWriter.GetInstance(document, new FileStream(FilePath, FileMode.Create));
            document.Open();

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();

            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

            hw.Parse(new StringReader(HTML));
            document.Close();
            ShowPdf(filename);
        }

        // on controller
        private void ShowPdf(string s)
        {
            /*
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "inline;filename=" + s);
            Response.ContentType = "application/pdf";
            Response.WriteFile(s);
            Response.Flush();
            Response.Clear();
            */
        }
    }
}
