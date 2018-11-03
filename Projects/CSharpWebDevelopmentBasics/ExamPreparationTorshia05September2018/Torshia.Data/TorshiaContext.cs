using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Torshia.Models;

namespace Torshia.Data
{
    public class TorshiaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<TaskSector> TaskSectors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-OSBRO1S\SQLEXPRESS; Database=Torshia; Integrated Security=true") ;
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskSector>().HasKey(x => new { x.SectorId, x.TaskId });

            modelBuilder.Entity<Report>()
                .HasOne(p => p.Task)
                .WithOne(i => i.Report)
                .HasForeignKey<Report>(b => b.TaskId);
        }
    }
}
