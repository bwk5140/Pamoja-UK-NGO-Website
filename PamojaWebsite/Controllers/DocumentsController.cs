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

        public DocumentsController(ApplicationDbContext db)
        {
            _db = db;
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
    }
}
