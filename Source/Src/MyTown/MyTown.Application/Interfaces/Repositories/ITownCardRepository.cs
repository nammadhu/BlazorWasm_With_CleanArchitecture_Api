using SharedResponse.DTOs;

namespace MyTown.Application.Interfaces.Repositories
    {
    public interface ITownCardRepository : IGenericRepository<TownCard>
        {
        Task<PagenationResponseDto<TownCardDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name);

        Task<IList<TownCardDto>> GetByNameAsync(string name);
        Task<bool> IsNameExistsAsync(string name);
        }
    }
