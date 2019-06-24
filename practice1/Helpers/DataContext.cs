using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practice1.Models;

namespace practice1.Helpers
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) :
            base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(i => i.UserId);

            base.OnModelCreating(builder);

            builder.Entity<Company>()
           .HasMany(ag => ag.Users)
           .WithOne(au => au.Company)
           .HasForeignKey(au => au.CompanyId);



        }


    }
}
