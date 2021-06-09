using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SpiceBot
{
    public class SpiceContext : DbContext
    {
        public SpiceContext()
        {
        }

        public SpiceContext(DbContextOptions<SpiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Noun> Nouns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=spice.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Noun>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
