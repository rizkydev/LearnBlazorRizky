using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RizkyApps.API.Models;

namespace RizkyApps.API.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<MsUser> MsUsers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Database=LearnBlazorRizkyDB;Username=postgres;Password=jembatan1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MsUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MsUser_pkey");

            entity.ToTable("MsUser");

            entity.Property(e => e.Birthday).HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
