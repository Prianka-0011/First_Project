using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NikuHotel.Models;

namespace NikuHotel.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

       

        public DbSet<NikuHotel.Models.Booking> Booking { get; set; }

        public DbSet<NikuHotel.Models.Customer> Customer { get; set; }

        public DbSet<NikuHotel.Models.Employee> Employee { get; set; }

        public DbSet<NikuHotel.Models.Room> Room { get; set; }

        public DbSet<NikuHotel.Models.Payment> Payment { get; set; }
    }
}
