using Microsoft.EntityFrameworkCore;

namespace Form.Models
{
    public class DBConnection : DbContext
    {
        public DBConnection(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SignForm> SignForm { get; set; }

        public DbSet<Cities> Cities { get; set; }

        public DbSet<States> States { get; set; }

    }
}
