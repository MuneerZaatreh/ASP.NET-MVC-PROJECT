using Expose_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Expose_Tracker.Data
{
    public class ApplicetionDbContext : DbContext
    {
        public ApplicetionDbContext(DbContextOptions<ApplicetionDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Employee>().ToTable("Employees", "HR");
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public object Transaction { get; internal set; }
    }
}
