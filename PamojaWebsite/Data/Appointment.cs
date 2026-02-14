namespace PamojaWebsite.Data
{
    public class Appointment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal? Amount { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(DateTime.UtcNow);
        public string? Calendar { get; set; }
        public string? Message { get; set; }
    }
}
