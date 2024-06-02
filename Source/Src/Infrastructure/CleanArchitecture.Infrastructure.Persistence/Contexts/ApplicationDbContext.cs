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
        public DbSet<TownCardType> CardTypes { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<TownCard> Cards { get; set; }
        public DbSet<TownCardApproval> CardApprovals { get; set; }
        public DbSet<TownApprovedCardSelectedDate> SelectedDates { get; set; }


        //not included in db,as builder.Ignore<Product>();
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
            builder.Ignore<Product>();

            //master datas id identity disabling
            builder.Entity<Town>().Property(et => et.Id).ValueGeneratedNever();
            builder.Entity<TownCardType>().Property(et => et.Id).ValueGeneratedNever();

            // Configure TPT inheritance
            builder.Entity<TownCard>().ToTable("Cards").HasKey(t => t.Id);
            //if this not mentioned,due to inheritance it creates single table(Table-Per-Hierarchy (TPH)) itself with discriminator,so to avoid these statements
            builder.Entity<TownApprovedCard>().ToTable("ApprovedCards");

            //builder.Entity<TownCard>().HasOne(tac => tac.ApprovedCard).WithOne().
            //    HasForeignKey<TownApprovedCard>(tc => tc.CardId);

            builder.Entity<TownApprovedCard>().HasOne<TownCard>(t => t.Card).WithOne()
                .HasForeignKey<TownCard>(t => t.ApprovedCardId)
                .OnDelete(DeleteBehavior.Restrict)
                ; // This will prevent cascade delete;

            
            // Configure the foreign key for TownApprovedCard
            builder.Entity<TownApprovedCard>()
                .HasOne<Town>() // Specify the navigation property in Town
                .WithMany(t => t.ApprovedCards) // Specify the collection property in Town
                .HasForeignKey(tac => tac.TownId); // Specify the foreign key property in TownApprovedCard

            // Configure the foreign key for TownCard
            builder.Entity<TownCard>()
                .HasOne<Town>() // Specify the navigation property in Town
                .WithMany(t => t.TownCards) // Specify the collection property in Town
                .HasForeignKey(tc => tc.TownId); // Specify the foreign key property in TownCard



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
