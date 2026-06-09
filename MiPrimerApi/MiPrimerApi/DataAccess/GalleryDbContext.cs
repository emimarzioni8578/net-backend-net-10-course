using Microsoft.EntityFrameworkCore;
using MiPrimerApi.Entities;

namespace MiPrimerApi.DataAccess
{
    public class GalleryDbContext : DbContext
    {
        public GalleryDbContext(DbContextOptions<GalleryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Geo> Geos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);
            
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId);

            modelBuilder.Entity<Address>()
                 .HasOne(g => g.Geo)
                 .WithOne(a => a.Address)
                 .HasForeignKey<Geo>(g => g.AddressId);
        }
    }
}

    



