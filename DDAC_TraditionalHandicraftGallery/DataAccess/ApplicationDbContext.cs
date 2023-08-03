using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace DDAC_TraditionalHandicraftGallery.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Handicraft> Handicrafts { get; set; }
        public DbSet<HandicraftType> HandicraftTypes { get; set; }
        public DbSet<QuoteRequest> QuoteRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Seed the HandicraftType data
            //modelBuilder.Entity<HandicraftType>().HasData(
            //    new HandicraftType { Id = 1, Name = "Pottery", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            //    new HandicraftType { Id = 2, Name = "Woodwork", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            //);

            //// Seed the Handicraft data
            //modelBuilder.Entity<Handicraft>().HasData(
            //    new Handicraft { Id = 1, Name = "Glazed Pot", Description = "A beautifully crafted glazed pot.", AuthorName = "John Smith", AuthorEmail = "johnsmith@example.com", TypeId = 1, IsHidden = false, ImageURL = "url1", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            //    new Handicraft { Id = 2, Name = "Oak Table", Description = "A sturdy oak table, hand-crafted with precision.", AuthorName = "Jane Doe", AuthorEmail = "janedoe@example.com", TypeId = 2, IsHidden = false, ImageURL = "url2", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            //);

            //// Seed the User data (Admin and User)
            //var hasher = new PasswordHasher<ApplicationUser>();
            //modelBuilder.Entity<ApplicationUser>().HasData(
            //    new ApplicationUser
            //    {
            //        Id = "1",
            //        UserName = "admin",
            //        NormalizedUserName = "ADMIN",
            //        Email = "admin@example.com",
            //        NormalizedEmail = "ADMIN@EXAMPLE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "Admin@123"),
            //        SecurityStamp = string.Empty,
            //        IsAdmin = true,
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now
            //    },
            //    new ApplicationUser
            //    {
            //        Id = "2",
            //        UserName = "user",
            //        NormalizedUserName = "USER",
            //        Email = "user@example.com",
            //        NormalizedEmail = "USER@EXAMPLE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "User@123"),
            //        SecurityStamp = string.Empty,
            //        IsAdmin = false,
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now
            //    }
            //);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IHasTimeStamp>().Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            foreach (var entry in ChangeTracker.Entries<IHasTimeStamp>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }


    }
}
