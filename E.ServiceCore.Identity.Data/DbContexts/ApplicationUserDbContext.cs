using E.ServiceCore.Identity.Data.Enities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E.ServiceCore.Identity.Data.DbContexts
{
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser, ApplicationRoles, string>
    {
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");



            modelBuilder.Entity<Users>(entity =>
           {
               entity.ToTable("users");

               entity.Property(e => e.Id).HasColumnName("id");

               entity.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("datetime");

               entity.Property(e => e.DeletedAt)
                   .HasColumnName("deleted_at")
                   .HasColumnType("datetime");

               entity.Property(e => e.DepartmentId).HasColumnName("department_id");

               entity.Property(e => e.JabatanId).HasColumnName("jabatan_id");

               entity.Property(e => e.LocationId).HasColumnName("location_id");

               entity.Property(e => e.Name)
                   .IsRequired()
                   .HasColumnName("name")
                   .HasMaxLength(50)
                   .IsUnicode(false);

               entity.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasColumnType("datetime");

               entity.Property(e => e.UserId)
                   .HasColumnName("user_id")
                   .HasMaxLength(450);
           });


        }
    }
}
