namespace PamojaWebsite.Data
{
    public class SocialMediaLink
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Platform { get; set; } = "";
        public string Url { get; set; } = "";
        public string UserId { get; set; }
        public ICollection<string> BlogPostIds { get; set; } = new List<string>();
    }
}
