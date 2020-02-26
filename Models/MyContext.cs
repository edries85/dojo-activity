 using Microsoft.EntityFrameworkCore; 
    namespace BeltExam.Models
    {
        public class MyContext : DbContext
        {
            public MyContext(DbContextOptions options) : base(options) { }
            
            // "users" table is represented by this DbSet "Users"
            public DbSet<User> Users {get;set;}
            public DbSet<DojoActivity> DojoActivities{get;set;}
            public DbSet<RSVP> RSVPs{get;set;}
            
            
        }
    }