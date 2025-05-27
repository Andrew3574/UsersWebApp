using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp_Tak4.Models;

namespace WebApp_Tak4.Data;

public partial class Task4DbContext : DbContext
{
    private string _connectionString;
    public Task4DbContext()
    {
    }

    public Task4DbContext(DbContextOptions<Task4DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Lastlogin)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(256)
                .HasColumnName("passwordhash");
            entity.Property(e => e.UserState)
                .HasMaxLength(10)
                .HasColumnName("userstate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
