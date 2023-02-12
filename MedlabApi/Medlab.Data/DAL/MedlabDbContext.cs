using Medlab.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Medlab.Data.DAL
{
    public class MedlabDbContext:IdentityDbContext<AppUser>
    {
        public MedlabDbContext(DbContextOptions<MedlabDbContext> options):base(options)
        {

        }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Category> Cateogries { get; set; }
        public DbSet<Slider> Sliders { get; set; }





        //===================
        // Fluent Validation
        //===================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
            base.OnModelCreating(modelBuilder);
        }


    }

    
}
