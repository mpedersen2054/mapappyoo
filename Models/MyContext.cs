using Microsoft.EntityFrameworkCore;

namespace mapapp.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        { }
 
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups {get; set;}
        public DbSet<Location> Locations {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<Review> Reviews {get; set;}
        public DbSet<UserGroup> UserGroups {get; set;}
        public DbSet<LocationCategory> LocationCategories {get; set;}
        public DbSet<GroupLocation> GroupLocations {get; set;}



    }
}
