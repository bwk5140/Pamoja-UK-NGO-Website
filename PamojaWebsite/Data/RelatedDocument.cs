namespace PamojaWebsite.Data
{
    public class RelatedDocument
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }

    }
}
