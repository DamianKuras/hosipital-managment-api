using Microsoft.EntityFrameworkCore;
namespace hosipital_managment_api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Medicne> Medicines;
    }
}
