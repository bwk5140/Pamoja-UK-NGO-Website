namespace PamojaWebsite.Data
{
    public class ContactUsForm
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
