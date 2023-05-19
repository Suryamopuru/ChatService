using chatService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace chatService.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> context) : base(context)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupUsers> GroupUsers { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }
}
