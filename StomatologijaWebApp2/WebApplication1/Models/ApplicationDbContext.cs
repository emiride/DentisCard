using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dentist>().HasRequired(t => t.Schedule).WithRequiredPrincipal(t => t.Dentist);
            modelBuilder.Entity<Patient>().HasRequired(t => t.MedicalHistory).WithRequiredPrincipal(t => t.Patient);
            modelBuilder.Entity<Dentist>().HasRequired(t => t.Patients);
            modelBuilder.Entity<MedicalHistory>().HasRequired(t => t.MedicalRecords);
            modelBuilder.Entity<MedicalHistory>().HasRequired(t => t.Teeth);
            modelBuilder.Entity<Patient>().HasRequired(t => t.Appointments);
            modelBuilder.Entity<Schedule>().HasRequired(t => t.Appointments);
            //modelBuilder.Entity<Patient>().HasRequired(t => t.Dentist);
            //modelBuilder.Entity<Appointment>().HasRequired(t => t.Schedule);
            //modelBuilder.Entity<Appointment>().HasRequired(t => t.Patient);

            base.OnModelCreating(modelBuilder);
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