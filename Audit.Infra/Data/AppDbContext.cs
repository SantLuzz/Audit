using Audit.Infra.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Audit.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : 
        DbContext(options)
    {
        public DbSet<UserDTO> Users { get; set; } = null!;
        public DbSet<TransactionDTO> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDTO>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<TransactionDTO>()
                 .HasOne(t => t.User) 
                 .WithMany(u => u.Transactions) 
                 .HasForeignKey(t => t.UserId) 
                 .HasPrincipalKey(u => u.Email); 

            base.OnModelCreating(modelBuilder);
        }

    }
}
