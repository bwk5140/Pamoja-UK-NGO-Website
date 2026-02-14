namespace PamojaWebsite.Data
{
    public class CareerRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? WorkType { get; set; }
        public string? Location { get; set; }
        public string? ExperienceLevel { get; set; }
        public string? Link { get; set; }
        public string? CareerField { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Expiration { get; set; }
    }
}
