using PamojaWebsite.Data;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore.Internal;
using PamojaWebsite.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PamojaWebsite.Services
{
    public class ResourcesService
    {
        private readonly Lazy<Task<List<Tag>>> Tags;
        private readonly Lazy<Task<List<Document>>> Documents;
        private readonly Lazy<Task<List<BlogPost>>> BlogPosts;
        private readonly Lazy<Task<List<BlogCategory>>> BlogCategories;
        private readonly Lazy<Task<List<SocialMediaLink>>> SocialMediaLinks;
        private readonly MyApiService _myApiService;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private List<DocumentMetadata> DocumentsMetaData = new();
        private List<BlogCategory> UpdatedBlogCategories = new();
        private List<Tag> UpdatedTags = new();

        public ResourcesService (MyApiService myApiService, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _myApiService = myApiService;
            _dbContextFactory = dbContextFactory;
            BlogPosts = new Lazy<Task<List<BlogPost>>>(async () => await _myApiService.GetHttpClient().GetFromJsonAsync<List<BlogPost>>("api/Db/blogs"));
            Documents = new Lazy<Task<List<Document>>>(async () => await _myApiService.GetHttpClient().GetFromJsonAsync<List<Document>>("api/Db/documents"));
            SocialMediaLinks = new Lazy<Task<List<SocialMediaLink>>>(async () => await _myApiService.GetHttpClient().GetFromJsonAsync<List<SocialMediaLink>>("api/Db/social-media-links"));
            Tags = new Lazy<Task<List<Tag>>>(async () => await _myApiService.GetHttpClient().GetFromJsonAsync<List<Tag>>("api/Db/tags"));
            BlogCategories = new Lazy<Task<List<BlogCategory>>>(async () => await _myApiService.GetHttpClient().GetFromJsonAsync<List<BlogCategory>>("api/Db/blog-categories"));
        }
        public int GetDocxPageCount(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);
            var pageCountText = doc.ExtendedFilePropertiesPart?.Properties?.Pages?.Text;

            return int.TryParse(pageCountText, out int count) ? count : 1;
        }
        public async Task<List<BlogCategory>> GetBlogCategories()
        {
            return UpdatedBlogCategories.Any() ? UpdatedBlogCategories : await BlogCategories.Value;
        }
        public async Task<List<BlogPost>> GetBlogPosts()
        {
            return await BlogPosts.Value;
        }
        public async Task<List<BlogPost>> GetRefreshedBlogPosts()
        {
            var RefreshedBlogPosts = await _myApiService.GetHttpClient().GetFromJsonAsync<List<BlogPost>>("api/Db/refreshed-blogs");
            return RefreshedBlogPosts;
        }
        public async Task<List<Tag>> GetBlogTags()
        {
            return UpdatedTags.Any() ? UpdatedTags : await Tags.Value;
        }
        public async Task<List<Document>> GetDocuments()
        {
            return await Documents.Value;
        }
        public async Task<List<DocumentMetadata>> GetDocumentsMetaData()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            DocumentsMetaData = DocumentsMetaData ?? await context.DocumentMetadata.ToListAsync();
            return DocumentsMetaData;
        }
        public async Task<List<SocialMediaLink>> GetSocialMediaLinks()
        {
            return await SocialMediaLinks.Value;
        }
        public async Task UpdateCategories()
        {
            UpdatedBlogCategories = await _myApiService.GetHttpClient().GetFromJsonAsync<List<BlogCategory>>("api/Db/update-categories");
        }
        public async Task UpdateTags()
        {
            UpdatedTags = await _myApiService.GetHttpClient().GetFromJsonAsync<List<Tag>>("api/Db/update-tags");
        }
    }
}
