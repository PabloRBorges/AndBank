using AndBank.Core.Data;
using AndBank.Process.Data.Mappings;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Database=positionsdb;Username=postgres;Password=1q2w3e4r@");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PositionMapping());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            //TODO: incluir validação extras

            var sucesso = await base.SaveChangesAsync() > 0;

            return sucesso;
        }
    }
}
