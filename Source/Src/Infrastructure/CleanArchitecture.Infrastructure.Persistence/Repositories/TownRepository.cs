using CleanArchitecture.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MyTown.Application.Interfaces.Repositories;
using MyTown.Domain;
using PublicCommon;
using MyTown.SharedModels.DTOs;
using SharedResponse.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories
    {
    public class TownRepository(ApplicationDbContext dbContext) : GenericRepository<Town>(dbContext), ITownRepository
        {
        const int ResultLimit = 20;
        private readonly DbSet<Town> db = dbContext.Set<Town>();

        public async Task<PagenationResponseDto<TownDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name)
            {
            var query = db.OrderBy(p => p.Created).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                {
                query = query.Where(p => p.Name.Contains(name));
                }

            return await Paged(
                query.Select(p => p.To<Town, TownDto>()),
                pageNumber,
                pageSize);

            }
        public override async Task<Town> GetByIdIntAsync(int id, Guid? userId=null)
        //public async Task<Town> GetTownByIdAsync(int id, Guid? userId)
            {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var townQuery = db.Where(t => t.Id == id)
         .Select(t => new
             {
             Town = t,
             ApprovedCards = t.ApprovedCards
                 .Where(ac => ac.SelectedDates.Any(sd => sd.Date == today))
                 .Select(ac => new
                     {
                     ApprovedCard = ac,
                     IsOwner = userId.HasValue && ac.Card.UserId == userId.Value
                     })
             })
         .AsNoTracking();
            var result = await townQuery.FirstOrDefaultAsync();
            if (result != null)
                {
                // If a UserId is provided and matches the owner of the card, include it in the result
                foreach (var approvedCard in result.ApprovedCards)
                    {
                    if (approvedCard.IsOwner)
                        {
                        approvedCard.ApprovedCard.UserId = userId.Value;
                        }
                    }

                return result.Town;
                //return towns.To<Town, TownDto>();
                }
            return null;
            }
        public async Task<IList<TownDto>> GetByNameAsync(string name)
            {
            var query = db.OrderBy(p => p.Created).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            return await query.Take(ResultLimit).Select(p => p.To<Town, TownDto>()).ToListAsync();
            }

        public async Task<bool> IsNameExistsAsync(string name)
            {
            return await db.AnyAsync(x => x.Name == name);
            }
        }
    }
