using AndBank.Core.Data;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;

namespace AndBank.Data.Context
{
    public class PositionDbContext : DbContext, IUnitOfWork
    {
        public PositionDbContext(DbContextOptions<PositionDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected PositionDbContext()
        {
        }

        public DbSet<PositionModel> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=positionsdb;Username=postgres;Password=1q2w3e4r@");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PositionModel>().HasKey(p => new { p.PositionId, p.Date });
        }

        public async Task<bool> Commit()
        {
            //TODO: incluir validação extras

            var sucesso = await base.SaveChangesAsync() > 0;

            return sucesso;
        }
    }
}
