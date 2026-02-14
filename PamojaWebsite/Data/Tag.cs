namespace PamojaWebsite.Data
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public ICollection<string> BlogPostIds { get; set; } = new List<string>();
    }

}
