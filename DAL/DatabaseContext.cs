using ImageUpload.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageUpload.DAL
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext>options):base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}
