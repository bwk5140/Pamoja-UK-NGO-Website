namespace PamojaWebsite.Data
{
    public class BlogPost
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorRole { get; set; }
        public string AuthorBio { get; set; }
        public string AuthorImageContentType { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int ReadTime { get; set; }
        public string Content { get; set; }
        public string PreviewImageContentType { get; set; }
        public byte[] PreviewImage { get; set; }
        public byte[] AuthorImage { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
        public ICollection<string> TagIds { get; set; } = new List<string>();
        public ICollection<string> AuthorSocialMediaIds { get; set; } = new List<string>();
    }
}
