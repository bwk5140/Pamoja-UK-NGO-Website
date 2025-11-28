using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PamojaWebsite.Data;

namespace PamojaWebsite.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<PamojaWebsite.Data.CareerField> CareerField { get; set; } = default!;
        public DbSet<PamojaWebsite.Data.CareerRole> CareerRole { get; set; } = default!;
    }
}
