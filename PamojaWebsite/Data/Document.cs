namespace PamojaWebsite.Data
{
    public class Document
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public string Author { get; set; }
        public long Size { get; set; }
        public int Downloads { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Category { get; set; }
        public string ContentType { get; set; } // e.g., "application/pdf"
        public byte[] Data { get; set; }
        public DocumentMetadata Metadata { get; set; }
        public TableOfContents TableOfContents { get; set; }
        public List<RelatedDocument> RelatedDocuments { get; set; }
    }
}
