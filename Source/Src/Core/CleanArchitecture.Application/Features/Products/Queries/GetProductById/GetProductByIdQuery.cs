using CleanArchitecture.Domain.Products.Dtos;
using MediatR;
using SharedResponse.Wrappers;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<BaseResult<ProductDto>>
{
    public long Id { get; set; }
}
