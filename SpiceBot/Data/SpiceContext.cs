using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SpiceBot.Data
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

        public virtual DbSet<Thing> Things { get; set; }
        public virtual DbSet<Statement> Statements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=spice.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thing>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
