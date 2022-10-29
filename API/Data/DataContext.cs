using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<ForumCategory> Categories { get; set; }
        public DbSet<ForumThread> Threads { get; set; }
        public DbSet<ForumComment> Comments { get; set; }
    }
}