namespace PamojaWebsite.Data
{
    public class ContactForm
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SelectedService { get; set; } // optional
        public string Message { get; set; }
        public string Package { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
