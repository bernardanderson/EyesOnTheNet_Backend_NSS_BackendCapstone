using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyesOnTheNet.Models;
using EyesOnTheNet.Private;

namespace EyesOnTheNet.DAL
{
    public class EyesOnTheNetContext : DbContext
    {
        public EyesOnTheNetContext(DbContextOptions<EyesOnTheNetContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
