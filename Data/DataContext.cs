﻿using Microsoft.EntityFrameworkCore;
using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Database=minimal_db;Username=admin;Password=admin123;Port=5434");
        }

        public DbSet<Student> Students { get; set; }

    }
}
