using SharedResponse.DTOs;

namespace MyTown.Application.Interfaces.Repositories
    {
    public interface ITownRepository : IGenericRepository<Town>
        {
        Task<PagenationResponseDto<TownDto>> GetPagedListAsync(int pageNumber, int pageSize, string? name);

        Task<IList<TownDto>> GetByNameAsync(string name);
        Task<bool> IsNameExistsAsync(string name);
        }
    }
