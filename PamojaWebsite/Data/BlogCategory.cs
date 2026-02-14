namespace PamojaWebsite.Data
{
    public class BlogCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
