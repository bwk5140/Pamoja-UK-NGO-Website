using System.ComponentModel.DataAnnotations;

namespace PamojaWebsite.Data
{
    public class Donation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string InvoiceNumber { get; set; } = "#" + Guid.NewGuid().ToString();
        public string Name { get; set; }

        public string Email { get; set; }
        public string Currency { get; set; }

        public decimal Amount { get; set; }
        public decimal ProcessingFee { get; set; }
        public decimal Total { get; set; }
    }
}
