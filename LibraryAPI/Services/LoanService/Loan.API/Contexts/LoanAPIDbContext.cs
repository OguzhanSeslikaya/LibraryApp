using Loan.Shared.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Loan.API.Contexts
{
    public class LoanAPIDbContext : DbContext
    {
        public DbSet<Loan.Shared.Entities.Models.Loan> loans { get; set; }
        public DbSet<LoanOutbox> loanOutboxes { get; set; }
        public DbSet<StockInbox> stockInboxes { get; set; }
        public LoanAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanOutbox>().HasKey(a => a.idempotentToken);
            modelBuilder.Entity<StockInbox>().HasKey(a => a.idempotentToken);
            base.OnModelCreating(modelBuilder);
        }
    }
}
