using Fundo.Applications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.Infrastructure.DatabaseContext;

public class FundoDatabaseContext : DbContext
{
    public FundoDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Loan> Loans { get; set; } = default!;
    public DbSet<LoanPayment> LoanPayments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
        });

        modelBuilder.Entity<LoanPayment>(entity =>
        {
            entity.Property(e => e.Amount).HasPrecision(18, 2);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FundoDatabaseContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
