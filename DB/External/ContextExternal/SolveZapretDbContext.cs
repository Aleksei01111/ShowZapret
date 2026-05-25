using DB.External.EntitiesExternal;
using Microsoft.EntityFrameworkCore;

namespace DB.External.ContextExternal;

public partial class SolveZapretDbContext : DbContext
{
    public SolveZapretDbContext()
    {
    }

    public SolveZapretDbContext(DbContextOptions<SolveZapretDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kusplog> Kusplogs { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlServer(
        //     "Server=192.168.88.44;Database=SolveZapret;User Id=isp-223;Password=isp-223;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=SolveZapret;User Id=1234;Password=1234;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kusplog>(entity =>
        {
            entity.ToTable("KUSPLog", tb => tb.HasTrigger("ChangeUserHonorForReportUpdate"));

            entity.Property(e => e.DateOfAdd).HasColumnType("datetime");

            entity.HasOne(d => d.Report).WithMany(p => p.Kusplogs)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KUSPLog_Report");

            entity.HasOne(d => d.UserAdded).WithMany(p => p.Kusplogs)
                .HasForeignKey(d => d.UserAddedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KUSPLog_User");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.UserSender).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_User");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.ToTable("Rule");

            entity.HasOne(d => d.UserRoleMask).WithMany(p => p.Rules)
                .HasForeignKey(d => d.UserRoleMaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rule_UserRole");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Login).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserRole");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
