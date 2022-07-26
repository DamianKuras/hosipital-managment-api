using hosipital_managment_api.Models;
using Microsoft.EntityFrameworkCore;
namespace hosipital_managment_api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Medicine> Medicines { get; set; }
    }
}
