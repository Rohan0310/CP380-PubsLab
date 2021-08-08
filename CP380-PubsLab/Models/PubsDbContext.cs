using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CP380_PubsLab.Models
{
    public class PubsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbpath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\pubs.mdf"));
            optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Integrated Security=true;AttachDbFilename={dbpath}");
        }

        // TODO: Add DbSets

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Jobs> Jobs { get; set; }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<Titles> Titles { get; set; }
        public DbSet<Stores> Stores { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO
            modelBuilder.Entity<Sales>().HasKey(a => new { a.stor_id, a.title_id });

            modelBuilder
                .Entity<Stores>()
                .HasMany(a => a.Titles)
                .WithMany(a => a.Stores)
                .UsingEntity<Sales>(
                J => J
                        .HasOne(at => at.Title)
                        .WithMany(t => t.Sales)
                        .HasForeignKey(at => at.title_id),

                J => J

                .HasOne(at => at.Store)
                .WithMany(t => t.Sales)
                .HasForeignKey(at => at.stor_id)

                );


        }
    }


    public class Titles
    {
        // TODO
       [Key]

        public string title_id { get; set; }
        public string title { get; set; }
        public List<Sales> Sales{ get; set; }
        public ICollection<Stores> Stores { get; set; }
    }


    public class Stores
    {
        // TODO
        [Key]

        public string stor_id { get; set; }
        public string stor_name { get; set; }
        public List<Sales> Sales { get; set; }
        public ICollection<Titles> Titles { get; set; }
    }


    public class Sales
    {
        // TODO
        [Column("stor_id")]

        public string stor_id { get; set; }
        [ForeignKey("stor_id")]
        public Stores Store { get; set; }
        [Column("title_id")]
        public string title_id { get; set; }
        [ForeignKey("title_id")]
        public Titles Title { get; set; }
    }

    public class Employee
    {
        // TODO
        [Key]
        [Column("emp_id")]
        public string Emp_id { get; set; }
        [Column("fname")]
        public string Firstname { get; set; }
        [Column("lname")]
        public string Lastname { get; set; }

        [Column("job_id")]
        public Int16 J_id { get; set; }
        [ForeignKey("J_id")]

        public Jobs Job { get; set; }
    }

    public class Jobs
    {
        // TODO

        [Key, Column("job_id")]

        public Int16 J_id { get; set; }
        [Column("job_desc")]

        public string Desc { get; set; }

        public List<Employee> Employee { get; set; }
    }
}
