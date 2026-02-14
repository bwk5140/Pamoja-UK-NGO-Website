namespace PamojaWebsite.Data
{
    public class DocumentMetadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime? LastModified { get; set; }
        public string Category { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public List<TableOfContents> TableOfContents { get; set; } = new();
        public List<RelatedDocument> RelatedDocuments { get; set; } = new();

    }
}
