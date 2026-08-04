using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PamojaWebsite.Data.Contexts;
using System.Text.RegularExpressions;

namespace PamojaWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IMemoryCache Cache;
        private IHttpClientFactory ClientFactory;
        public DbController(ApplicationDbContext db, IMemoryCache _cache, IHttpClientFactory _clientFactory)
        {
            _db = db;
            Cache = _cache;
            ClientFactory = _clientFactory;
        }

        [HttpGet("donations")]
        public async Task<IActionResult> GetDonations()
        {
            var donations = await Cache.GetOrCreateAsync("Donations", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _donations = await _db.Donation.ToListAsync();
                return _donations;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(donations);
        }
        [HttpGet("doc-metadata")]
        public async Task<IActionResult> GetDocMetaData()
        {
            var documentsMetadata = await Cache.GetOrCreateAsync("DocumentMetadata", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var docsmetadata = await _db.DocumentMetadata.ToListAsync();
                return docsmetadata;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(documentsMetadata);
        }
        [HttpGet("refreshed-donations")]
        public async Task<IActionResult> GetRefreshedDonations()
        {
            Cache.Remove("Donations");
            var donations = await Cache.GetOrCreateAsync("Donations", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _donations = await _db.Donation.ToListAsync();
                return _donations;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(donations);
        }
        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await Cache.GetOrCreateAsync("Payments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _payments = await _db.Payment.ToListAsync();
                return _payments;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(payments);
        }
        [HttpGet("refreshed-payments")]
        public async Task<IActionResult> GetRefreshedPayments()
        {
            Cache.Remove("Payments");
            var payments = await Cache.GetOrCreateAsync("Payments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _payments = await _db.Payment.ToListAsync();
                return _payments;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(payments);
        }
        [HttpGet("country-codes")]
        public async Task<IActionResult> GetCountryCodes()
        {
            var country_codes = await Cache.GetOrCreateAsync("CountryCodes", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var codes = await _db.CountryCode.ToListAsync();
                return codes;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(country_codes);
        }
        [HttpGet("refreshed-country-codes")]
        public async Task<IActionResult> GetRefreshedCountryCodes()
        {
            Cache.Remove("CountryCodes");
            var country_codes = await Cache.GetOrCreateAsync("CountryCodes", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var codes = await _db.CountryCode.ToListAsync();
                return codes;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(country_codes);
        }
        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments()
        {
            var documents = await Cache.GetOrCreateAsync("Documents", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var docs = await _db.Document.ToListAsync();
                return docs;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(documents);
        }
        [HttpGet("refreshed-documents")]
        public async Task<IActionResult> GetRefreshedDocuments()
        {
            Cache.Remove("Documents");
            var documents = await Cache.GetOrCreateAsync("Documents", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var docs = await _db.Document.ToListAsync();
                return docs;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(documents);
        }
        [HttpGet("refreshed-blogs")]
        public async Task<IActionResult> GetRefreshedBlogs()
        {
            Cache.Remove("Blogs");
            var blogposts = await Cache.GetOrCreateAsync("Blogs", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var blogs = await _db.BlogPost.ToListAsync();
                return blogs;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(blogposts);
        }
        [HttpGet("blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await Cache.GetOrCreateAsync("Blogs", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _blogs = await _db.BlogPost.ToListAsync();
                return _blogs;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(blogs);
        }
        [HttpGet("tags")]
        public async Task<IActionResult> GetBlogTags()
        {
            var tags = await Cache.GetOrCreateAsync("Tags", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _tags = await _db.Tag.ToListAsync();
                return _tags;
            });

            Response.Headers["Cache-Control"] = "no-store, no-cache";

            return Ok(tags);
        }
        [HttpGet("update-tags")]
        public async Task<IActionResult> UpdateBlogTags()
        {
            var tags = await _db.Tag.ToListAsync();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            Cache.Set("Tags", tags, cacheOptions);

            Response.Headers["Cache-Control"] = "no-store, no-cache";

            return Ok(tags);
        }
        [HttpGet("blog-categories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var blog_categories = await Cache.GetOrCreateAsync("BlogCategories", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _blog_categories = await _db.BlogCategory.ToListAsync();
                return _blog_categories;
            });

            Response.Headers["Cache-Control"] = "no-store, no-cache";

            return Ok(blog_categories);
        }
        [HttpGet("update-categories")]
        public async Task<IActionResult> UpdateBlogCategories()
        {
            var categories = await _db.BlogCategory.ToListAsync();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            Cache.Set("BlogCategories", categories, cacheOptions);

            Response.Headers["Cache-Control"] = "no-store, no-cache";

            return Ok(categories);
        }
        [HttpGet("career-roles")]
        public async Task<IActionResult> GetCareerRoles()
        {
            var career_roles = await Cache.GetOrCreateAsync("AvailableRoles", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var _roles = await _db.CareerRole.ToListAsync();
                return _roles;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(career_roles);
        }
        [HttpGet("career-fields")]
        public async Task<IActionResult> GetCareerFields()
        {
            var career_fields = await Cache.GetOrCreateAsync("AvailableFields", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var fields = await _db.CareerField.ToListAsync();
                return fields;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(career_fields);
        }
        [HttpGet("social-media-links")]
        public async Task<IActionResult> GetSocialMedia()
        {
            var social_media_links = await Cache.GetOrCreateAsync("SocialMediaLinks", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var links = await _db.SocialMediaLink.ToListAsync();
                return links;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(social_media_links);
        }
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = 
            await Cache.GetOrCreateAsync("Appointments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var links = await _db.Appointment.ToListAsync();
                return links;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(appointments);
        }
        [HttpGet("refreshed-appointments")]
        public async Task<IActionResult> GetRefreshedAppointments()
        {
            Cache.Remove("Appointments");
            var appointments = 
            await Cache.GetOrCreateAsync("Appointments", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var links = await _db.Appointment.ToListAsync();
                return links;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(appointments);
        }
        [HttpGet("refreshed-tags")]
        public async Task<IActionResult> GetRefreshedTags()
        {
            Cache.TryGetValue("Tags", out var cachedTags);
            if (cachedTags is not null)
            {
                Cache.Remove("Tags");
            }
            var tags = 
            await Cache.GetOrCreateAsync("Tags", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var links = await _db.Tag.ToListAsync();
                return links;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(tags);
        }
        [HttpGet("refreshed-categories")]
        public async Task<IActionResult> GetRefreshedCategories()
        {
            Cache.Remove("BlogCategories");
            var categories = 
            await Cache.GetOrCreateAsync("BlogCategories", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var links = await _db.BlogCategory.ToListAsync();
                return links;
            });

            Response.Headers["Cache-Control"] = "public, max-age=3600";

            return Ok(categories);
        }
        [HttpGet("filter-documents")]
        public async Task<IActionResult> GetFilteredDocuments([FromQuery] string? filter)
        {
            // Fetch all documents first
            var docs = await Cache.GetOrCreateAsync("Documents", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                var docs = await _db.Document.ToListAsync();
                return docs;
            });

            if (!string.IsNullOrEmpty(filter))
            {
                var regexPattern = ".*" + Regex.Escape(filter.ToLower()) + ".*";

                docs = docs
                    .Where(d =>
                        d.ContentType != null &&
                        Regex.IsMatch(GetShortExtension(d.ContentType), regexPattern, RegexOptions.IgnoreCase)
                    )
                    .ToList();
            }
            Response.Headers["Cache-Control"] = "public, max-age=3600";
            return Ok(docs);
        }
        private string GetShortExtension(string contentType)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "doc" },
            { "application/msword", "doc" },
            { "application/pdf", "pdf" },
            { "text/plain", "txt" },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xls" },
            { "image/png", "png" }
        };

            return mapping.TryGetValue(contentType, out var extension) ? extension : "unknown";
        }
    }
}
