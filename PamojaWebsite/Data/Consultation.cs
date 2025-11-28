using System.ComponentModel.DataAnnotations;

namespace PamojaWebsite.Data
{
    public class Consultation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

    }
}
