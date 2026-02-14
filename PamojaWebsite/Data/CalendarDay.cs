namespace PamojaWebsite.Data
{
    public class CalendarDay
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public bool IsInThisMonth { get; set; }
    }
}
