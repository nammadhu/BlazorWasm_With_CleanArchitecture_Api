using CleanArchitecture.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MyTown.Application.Interfaces.Repositories;
using MyTown.Domain;
using MyTown.SharedModels.DTOs;
using PublicCommon;
using SharedResponse.DTOs;
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
