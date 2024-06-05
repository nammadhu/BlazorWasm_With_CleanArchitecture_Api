using CleanArchitecture.Application.DTOs.Account.Responses;
using CleanArchitecture.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MyTown.Application.Interfaces.Repositories;
using MyTown.Domain;
using MyTown.SharedModels.DTOs;
using PublicCommon;
using SharedResponse.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories
    {
    public class TownCardRepository(ApplicationDbContext dbContext) : GenericRepository<TownCard>(dbContext), ITownCardRepository
        {
        const int ResultLimit = 20;
        private readonly DbSet<TownCard> dbCard = dbContext.Set<TownCard>();
        private readonly DbSet<TownApprovedCard> dbApprovedCard = dbContext.Set<TownApprovedCard>();

        public async Task<(List<int> approvedCardIds, List<TownCard> draftCards)> GetUserCardsMoreDetails(Guid userId, bool isCreator, bool isOwner, bool isApprovedCardOwner, bool isApprovedReviewer, int townId = 0)
            {
            //if creator or owner , then fetch only on Card table
            //if ApprovedCardOwnerowner, then fetch on ApprovedCard with ownerid column
            //returns list<approvedid>,list<Card> drafts,list<Card> waitingforMyApproval

            List<int> approvedCardIds = [];
            if (isApprovedCardOwner)
                approvedCardIds = await dbApprovedCard.Where(x => x.OwnerId == userId && (townId <= 0 || x.TownId == townId))
                    .Select(x => x.Id).ToListAsync();

            List<TownCard> draftCards = [];
            if (isCreator || isOwner)
                draftCards = await dbCard.Where(x => isCreator ? x.CreatedBy == userId : true
                && isOwner ? x.OwnerId == userId : false || townId <= 0 || x.TownId == townId)
                    .Take(ResultLimit)//not sure but better
                    .ToListAsync();

            //if(isApprovedReviewer)//later

            return (approvedCardIds, draftCards);
            }
        public async Task<PagenationResponseDto<TownCardDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name)
            {
            var query = dbCard.OrderBy(p => p.Created).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                {
                query = query.Where(p => p.Name.Contains(name));
                }

            return await Paged(
                query.Select(p => p.To<TownCard, TownCardDto>()),
                pageNumber,
                pageSize);

            }
        public async Task<IList<TownCardDto>> GetByNameAsync(string name)
            {
            var query = dbCard.OrderBy(p => p.Created).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            return await query.Take(ResultLimit).Select(p => p.To<TownCard, TownCardDto>()).ToListAsync();
            }

        public async Task<bool> IsNameExistsAsync(string name)
            {
            return await dbCard.AnyAsync(x => x.Name == name);
            }
        }
    }
