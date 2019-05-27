using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace My.Custom.Template.DataLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        //ToDO: Add DbSets



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            // foreach (var entry in ChangeTracker.Entries<Product>())
            // {
            //     switch (entry.State)
            //     {
            //         case EntityState.Added:
            //             entry.CurrentValues["IsDeleted"] = false;
            //             entry.CurrentValues["CreatedOn"] = DateTime.UtcNow;
            //             entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
            //             break;

            //         case EntityState.Deleted:
            //             entry.State = EntityState.Modified;
            //             entry.CurrentValues["IsDeleted"] = true;
            //             entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
            //             break;

            //         case EntityState.Modified:
            //             entry.State = EntityState.Modified;
            //             entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;
            //             break;
            //     }
            // }

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //New Query Filters in EF Core
            // builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

            base.OnModelCreating(builder);
        }

    }
}