using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    { }

    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            var entity = entry.Entity;
            var now = DateTime.Now;
            string name = "Njezi";
            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedDate = now;
                    entity.CreatedBy = name;
                    break;
                case EntityState.Modified:
                    entity.LastModifiedDate = now;
                    entity.LastModifiedBy = name;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
