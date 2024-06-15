using Microsoft.EntityFrameworkCore;

namespace GoodNewsTask.Models
{
    public class NewsContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }

        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
