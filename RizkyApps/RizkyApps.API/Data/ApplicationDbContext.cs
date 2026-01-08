using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RizkyApps.API.Models;

namespace RizkyApps.API.Data;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    //private readonly IConfiguration _configuration;
    //public ApplicationDbContext(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //}


    public virtual DbSet<Exam> Exams { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
    //        optionsBuilder.UseNpgsql(connectionString);
    //    }
    //}
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        OnModelCreatingPartial(modelBuilder);
    //    }

    //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
