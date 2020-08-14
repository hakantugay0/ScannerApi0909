using Microsoft.EntityFrameworkCore;
using Scanner.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scanner.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Billing> Billings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseLazyLoadingProxies(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscribe>()
                .HasOne(a => a.Billing).WithOne(b => b.Subscribe)
                .HasForeignKey<Billing>(e => e.SubscribeId);
        }

    }
}
