using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /*This override will allow for every instance that gets created and DateCreated will be saved to it*/
        public override int SaveChanges()
        {
            foreach (
                var history in
                    this.ChangeTracker.Entries()
                        .Where(
                            e =>
                                e.Entity is IModificationHistory &&
                                (e.State == EntityState.Added || e.State == EntityState.Modified))
                        .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated==DateTime.MinValue)
                {
                    history.DateCreated=DateTime.Now;
                }
            }

            var result = base.SaveChanges();
            
            return result;
        }
        
    }
}