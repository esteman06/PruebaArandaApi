using Microsoft.EntityFrameworkCore;

namespace PruebaAranda.DataModel.Model
{
    public partial class PruebaArandaContext : DbContext
    {
        public PruebaArandaContext() { }
        public PruebaArandaContext(DbContextOptions<PruebaArandaContext> options) : base(options) { }
        public virtual DbSet<Authorization> Authorization { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<IdentityUser> IdentityUser { get; set; }
        public virtual DbSet<InformationUser> InformationUser { get; set; }
        public virtual DbSet<RolAuthorizations> RolAuthorizations { get; set; }
        public virtual DbSet<Rols> Rols { get; set; }
        public virtual DbSet<SecurityLog> SecurityLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-J5RKTFBG\\SQLEXPRESS;Initial Catalog=PruebaArandaSoftware;Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authorization>(entity =>
            {
                entity.Property(e => e.AuthorizationId)
                    .HasColumnName("authorizationId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuthorizationNameSystem).HasColumnName("authorizationNameSystem");

                entity.Property(e => e.AuthorizationNameApplication).HasColumnName("authorizationNameApplication");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.Property(e => e.EventLogId)
                    .HasColumnName("eventLogID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Action).HasColumnName("action");

                entity.Property(e => e.IdentityUserId).HasColumnName("identityUserId");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsSuccessfull).HasColumnName("isSuccessfull");

                entity.Property(e => e.ObjectType)
                    .IsRequired()
                    .HasColumnName("objectType")
                    .HasMaxLength(50);

                entity.Property(e => e.Parameters).HasColumnName("parameters");

                entity.Property(e => e.Response).HasColumnName("response");
            });

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.Property(e => e.IdentityUserId)
                    .HasColumnName("identityUserId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsTempPassword).HasColumnName("isTempPassword");
            });

            modelBuilder.Entity<InformationUser>(entity =>
            {
                entity.Property(e => e.InformationUserId)
                    .HasColumnName("informationUserId")
                    .ValueGeneratedNever();
                
                entity.Property(e => e.IdentityUserId)
                    .HasColumnName("identityUserId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FirstName).HasColumnName("firstName");

                entity.Property(e => e.LastName).HasColumnName("lastName");

                entity.Property(e => e.Address).HasColumnName("addres");

                entity.Property(e => e.Phone).HasColumnName("phone");
                
                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Age).HasColumnName("age");
                
                entity.Property(e => e.RolsId)
                    .HasColumnName("rolsId")
                    .ValueGeneratedNever();

            });

            modelBuilder.Entity<RolAuthorizations>(entity =>
            {
                entity.Property(e => e.RolAuthorizationsId)
                    .HasColumnName("rolAuthorizationsId")
                    .ValueGeneratedNever();

                entity.Property(e => e.RolId)
                    .HasColumnName("rolId")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuthorizationId)
                    .HasColumnName("authorizationId")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Rols>(entity =>
            {
                entity.Property(e => e.RolsId)
                    .HasColumnName("rolsId")
                    .ValueGeneratedNever();

                entity.Property(e => e.RolNameSystem).HasColumnName("rolNameSystem");

                entity.Property(e => e.RolNameApplication).HasColumnName("rolNameApplication");
            });

            modelBuilder.Entity<SecurityLog>(entity =>
            {
                entity.Property(e => e.SecurityLogId)
                    .HasColumnName("securityLogID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activity)
                    .HasColumnName("activity")
                    .HasMaxLength(50);

                entity.Property(e => e.IdentityUserId).HasColumnName("identityUserId");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RemoteIpaddress)
                    .HasColumnName("remoteIPAddress")
                    .HasMaxLength(40);
            });
        }
    }
}
