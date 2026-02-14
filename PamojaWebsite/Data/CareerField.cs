using Microsoft.AspNetCore.Components.Forms;

namespace PamojaWebsite.Data
{
    public class CareerField
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }
        public string? ImagePreviewUrl { get; set; }
    }
}
