using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Text;
using System.Xml.Linq;
using UglyToad.PdfPig;

namespace PamojaWebsite.Services
{
    public class DocumentConverters
    {
        public string ConvertDocxToHtml(byte[] content)
        {
            using var ms = new MemoryStream(content);
            using var wordDoc = WordprocessingDocument.Open(ms, false);

            var settings = new HtmlConverterSettings()
            {
                PageTitle = "Converted Document"
            };

            XElement html = HtmlConverter.ConvertToHtml(wordDoc, settings);

            // Convert XElement to string
            return html.ToString();
        }
        public string ConvertXlsxToHtml(byte[] content)
        {
            using var ms = new MemoryStream(content); // MemoryStream is IDisposable
            var workbook = new XLWorkbook(ms);        // XLWorkbook is NOT IDisposable
            var ws = workbook.Worksheets.First();

            var sb = new StringBuilder("<table border='1'>");
            foreach (var row in ws.RowsUsed())
            {
                sb.Append("<tr>");
                foreach (var cell in row.Cells())
                {
                    sb.Append($"<td>{System.Net.WebUtility.HtmlEncode(cell.Value.ToString())}</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        public string ConvertPptxToHtml(byte[] content)
        {
            var sb = new StringBuilder("<div class='ppt'>");
            using var ms = new MemoryStream(content);
            using var pptDoc = PresentationDocument.Open(ms, false);

            foreach (var slide in pptDoc.PresentationPart.SlideParts)
            {
                sb.Append("<div class='slide'>");
                var texts = slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>();
                foreach (var t in texts)
                {
                    sb.Append($"<p>{t.Text}</p>");
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public string ConvertCsvToHtml(byte[] content)
        {
            string csv = Encoding.UTF8.GetString(content);
            var rows = csv.Split('\n');
            var sb = new StringBuilder("<table border='1'>");

            foreach (var row in rows)
            {
                sb.Append("<tr>");
                foreach (var cell in row.Split(','))
                {
                    sb.Append($"<td>{System.Net.WebUtility.HtmlEncode(cell)}</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string ConvertPlainTextToHtml(byte[] content)
        {
            if (content == null || content.Length == 0)
                return "<div>(empty document)</div>";

            // Decode the byte array into a UTF‑8 string
            string text = Encoding.UTF8.GetString(content);

            // Wrap in <pre> to preserve line breaks and spacing
            // HtmlEncode ensures special characters don’t break HTML
            string encoded = System.Net.WebUtility.HtmlEncode(text);

            return $"<pre style='white-space: pre-wrap; font-family: inherit;'>{encoded}</pre>";
        }
        public string ConvertPdfToHtml(byte[] content)
        {
            using var ms = new MemoryStream(content);
            using var pdf = PdfDocument.Open(ms);

            var sb = new StringBuilder("<div>");
            foreach (var page in pdf.GetPages())
            {
                sb.Append("<p>");
                sb.Append(page.Text);
                sb.Append("</p>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
