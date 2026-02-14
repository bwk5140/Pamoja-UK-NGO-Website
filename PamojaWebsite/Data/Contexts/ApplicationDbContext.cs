using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PamojaWebsite.Data;

namespace PamojaWebsite.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<PamojaWebsite.Data.CareerField> CareerField { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.CareerRole> CareerRole { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.ContactForm> ContactForm { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.ContactUsForm> ContactUsForm { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.Donation> Donation { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.Payment> Payment { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.Appointment> Appointment { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.CountryCode> CountryCode { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.Document> Document { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.BlogPost> BlogPost { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.DocumentMetadata> DocumentMetadata { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.TableOfContents> TableOfContents { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.RelatedDocument> RelatedDocument { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.Tag> Tag { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.BlogCategory> BlogCategory { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.SocialMediaLink> SocialMediaLink { get; set; } = default!;
    }
}
