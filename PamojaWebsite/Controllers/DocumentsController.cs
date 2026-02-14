using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PamojaWebsite.Data.Contexts;
using PamojaWebsite.Services;

namespace PamojaWebsite.Controllers
{
    [Route("api/documents")]
    public class DocumentsController : Controller
    {
        readonly ApplicationDbContext _db;
        private readonly DocumentConverters _documentConverters;

        public DocumentsController(ApplicationDbContext db, DocumentConverters documentConverters)
        {
            _db = db;
            _documentConverters = documentConverters;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var doc = await _db.Document.FindAsync(id);
            if (doc == null) return NotFound();

            var stream = new MemoryStream(doc.Data);

            // Use the original content type (Word, Excel, etc.)
            Response.Headers["Content-Disposition"] = $"inline; filename={doc.Title}";
            return new FileStreamResult(stream, doc.ContentType);
        }
        //[HttpGet("{id}/html")]
        //public async Task<IActionResult> GetHtml(string Id)
        //{
        //    var docs = _db.Document.ToList();
        //    var doc = docs.FirstOrDefault(d => d.Id == Id);
        //    string html = doc.ContentType switch
        //    {
        //        "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => _documentConverters.ConvertDocxToHtml(doc.Data),
        //        "application/msword" => _documentConverters.ConvertDocxToHtml(doc.Data),
        //        "application/vnd.openxmlformats-officedocument.presentationml.presentation" => _documentConverters.ConvertPptxToHtml(doc.Data),
        //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => _documentConverters.ConvertXlsxToHtml(doc.Data),
        //        "application/vnd.ms-excel" => _documentConverters.ConvertCsvToHtml(doc.Data),
        //        "application/pdf" => _documentConverters.ConvertPdfToHtml(doc.Data),
        //        "text/plain" => _documentConverters.ConvertPlainTextToHtml(doc.Data),
        //        _ => "<div>Unsupported format</div>"
        //    };
        //    return base.Content(html, "text/html");
        //}
    }
}
