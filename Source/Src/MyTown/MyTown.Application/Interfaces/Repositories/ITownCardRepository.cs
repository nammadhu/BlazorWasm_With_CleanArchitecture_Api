using SharedResponse.DTOs;

namespace MyTown.Application.Interfaces.Repositories
    {
    public interface ITownCardRepository : IGenericRepository<TownCard>
        {

        Task<(List<int> approvedCardIds, List<TownCard> draftCards)> GetUserCardsMoreDetails(Guid userId, bool isCreator, bool isOwner, bool isApprovedCardOwner, bool isApprovedReviewer, int townId = 0);


        Task<PagenationResponseDto<TownCardDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name);

        Task<IList<TownCardDto>> GetByNameAsync(string name);
        Task<bool> IsNameExistsAsync(string name);
        }
    }
