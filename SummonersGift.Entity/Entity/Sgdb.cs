
using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SummonersGift.Models.Entity
{
    public partial class SgdbContext : DbContext
    {
        public SgdbContext()
            : base("name=sgdb")
        {
        }

        public virtual DbSet<Champion> Champions { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Lane> Lanes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Stat> Stats { get; set; }
        public virtual DbSet<Tier> Tiers { get; set; }
        public virtual DbSet<AggStat> AggStats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Champion>()
                .Property(e => e.Version)
                .IsUnicode(false);

            modelBuilder.Entity<Champion>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Champion>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Champion>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Champion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Division>()
                .Property(e => e.DivisionName)
                .IsUnicode(false);

            modelBuilder.Entity<Division>()
                .Property(e => e.DivisionJson)
                .IsUnicode(false);

            modelBuilder.Entity<Division>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Division)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lane>()
                .Property(e => e.LaneName)
                .IsUnicode(false);

            modelBuilder.Entity<Lane>()
                .Property(e => e.LaneJson)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.RegionName)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.RegionJson)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleJson)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stat>()
                .Property(e => e.StatName)
                .IsUnicode(false);

            modelBuilder.Entity<Stat>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Stat>()
                .Property(e => e.JsonEndPoint)
                .IsUnicode(false);

            modelBuilder.Entity<Stat>()
                .Property(e => e.JsonPath)
                .IsUnicode(false);

            modelBuilder.Entity<Stat>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Stat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tier>()
                .Property(e => e.TierName)
                .IsUnicode(false);

            modelBuilder.Entity<Tier>()
                .Property(e => e.TierJson)
                .IsUnicode(false);

            modelBuilder.Entity<Tier>()
                .HasMany(e => e.AggStats)
                .WithRequired(e => e.Tier)
                .WillCascadeOnDelete(false);
        }
    }
}
