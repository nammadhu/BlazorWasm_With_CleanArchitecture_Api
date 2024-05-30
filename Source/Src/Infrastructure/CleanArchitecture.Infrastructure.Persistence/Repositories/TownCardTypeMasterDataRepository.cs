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
    public class TownCardTypeMasterDataRepository(ApplicationDbContext dbContext) : GenericRepository<TownCardTypeMasterData>(dbContext), ITownCardTypeMasterDataRepository
        {
        const int ResultLimit = 20;
        private readonly DbSet<TownCardTypeMasterData> db = dbContext.Set<TownCardTypeMasterData>();

        public async Task<PagenationResponseDto<TownCardTypeDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name)
            {
            var query = db.OrderBy(p => p.Created).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                {
                query = query.Where(p => p.Name.Contains(name));
                }

            return await Paged(
                query.Select(p => p.To<TownCardTypeMasterData, TownCardTypeDto>()),
                pageNumber,
                pageSize);

            }
        public async Task<IList<TownCardTypeDto>> GetByNameAsync(string name)
            {
            var query = db.OrderBy(p => p.Created).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            return await query.Take(ResultLimit).Select(p => p.To<TownCardTypeMasterData, TownCardTypeDto>()).ToListAsync();
            }

        public async Task<bool> IsNameExistsAsync(string name)
            {
            return await db.AnyAsync(x => x.Name == name);
            }
        }
    }
