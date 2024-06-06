using CleanArchitecture.Domain.Products.Dtos;
using CleanArchitecture.Domain.Products.Entities;
using SharedResponse.DTOs;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<PagenationResponseDto<ProductDto>> GetPagedListAsync(int pageNumber, int pageSize, string name);
}
