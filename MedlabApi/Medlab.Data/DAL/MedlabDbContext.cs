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

        public DbSet<Category> Cateogries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
            base.OnModelCreating(modelBuilder);
        }


    }

    
}
