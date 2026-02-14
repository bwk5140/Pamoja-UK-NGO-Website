namespace PamojaWebsite.Data
{
    public class CountryCode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; }
        public string ISOCode { get; set; }
        public string Country { get; set; }
    }
}
