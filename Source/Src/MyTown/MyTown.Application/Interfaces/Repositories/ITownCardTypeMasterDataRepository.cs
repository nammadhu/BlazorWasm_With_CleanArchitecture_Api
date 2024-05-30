using SharedResponse.DTOs;

namespace MyTown.Application.Interfaces.Repositories
    {
    public interface ITownCardTypeMasterDataRepository : IGenericRepository<TownCardTypeMasterData>
        {
        Task<PagenationResponseDto<TownCardTypeDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name);

        Task<IList<TownCardTypeDto>> GetByNameAsync(string name);
        Task<bool> IsNameExistsAsync(string name);
        }
    }
