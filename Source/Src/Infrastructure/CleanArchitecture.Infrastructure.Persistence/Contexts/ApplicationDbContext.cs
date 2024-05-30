using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using MyTown.Domain;
using PublicCommon.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Contexts
    {
    public class ApplicationDbContext : DbContext
        {
        private readonly IAuthenticatedUserService authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUser) : base(options)
            {
            this.authenticatedUser = authenticatedUser;
            }
        public DbSet<Town> Towns { get; set; }

        public DbSet<Product> Products { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
            {
            var userId = string.IsNullOrEmpty(authenticatedUser.UserId)
                ? Guid.Empty : Guid.TryParse(authenticatedUser.UserId, out Guid id) ? id : Guid.Empty;
            //todo had to change this

            var currentTime = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
                {
                switch (entry.State)
                    {
                    case EntityState.Added:
                        entry.Entity.Created = currentTime;
                        entry.Entity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = currentTime;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.LastModified = currentTime;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    }
                }
            return base.SaveChangesAsync(cancellationToken);
            }
        protected override void OnModelCreating(ModelBuilder builder)
            {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                {
                property.SetColumnType("decimal(18,6)");
                }
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
            }
        }
    }
