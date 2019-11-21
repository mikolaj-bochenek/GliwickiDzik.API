using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base (options) {}
        
        public DbSet<User> Users { get; set; }
        
    }
}