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
        public DbSet<User> Users { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                    .UseMySql(PrivateParameters.MySQLParameterString); //Requires access to the private MySQL conntection string 
    }
}
