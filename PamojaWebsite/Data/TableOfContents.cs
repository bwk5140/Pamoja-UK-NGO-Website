namespace PamojaWebsite.Data
{
    public class TableOfContents
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Anchor { get; set; } = string.Empty;
    }
}
