using Microsoft.EntityFrameworkCore;
using System;
using PetData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetData.Mixins;

namespace PetData
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.Tracked += OnEntityTracked;
            ChangeTracker.StateChanged += OnEntityStateChanged;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => new { u.Name, u.Surname}).IsUnique();
        }
        void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is IHasCreationLastModified entity)
            {
                entity.Created = DateTime.Now;
                entity.LastModified = DateTime.Now;
            }
        }

        void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.NewState == EntityState.Modified && e.Entry.Entity is IHasCreationLastModified entity)
                entity.LastModified = DateTime.Now;
        }
    }
}
