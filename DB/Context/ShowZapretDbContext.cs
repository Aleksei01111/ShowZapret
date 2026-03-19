using System;
using System.Collections.Generic;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.Context;

internal partial class ShowZapretDbContext : DbContext
{
    public ShowZapretDbContext()
    {
    }

    public ShowZapretDbContext(DbContextOptions<ShowZapretDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=192.168.88.44;Database=ShowZapret;User Id=isp-223;Password=isp-223;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        
        // optionsBuilder.UseSqlServer(
        //     "Server=localhost;Database=ShowZapret;User Id=1234;Password=1234;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Note_User");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.ToTable("Rule");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            
            entity.Property(u => u.Role)
                .HasConversion<int>()
                .HasColumnName("Role")
                .HasColumnType("int")
                .IsRequired();
            
            entity.HasMany(e => e.Notes)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Rules)
                .WithOne(e => e.UserCreator)
                .HasForeignKey(e => e.UserCreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}