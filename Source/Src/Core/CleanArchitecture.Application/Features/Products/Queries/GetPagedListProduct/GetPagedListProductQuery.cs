using CleanArchitecture.Domain.Products.Dtos;
using MediatR;
using SharedResponse.Parameters;
using SharedResponse.Wrappers;

namespace CleanArchitecture.Application.Features.Products.Queries.GetPagedListProduct
    {
    public class GetPagedListProductQuery : PagenationRequestParameter, IRequest<PagedResponse<ProductDto>>
        {
        public string Name { get; set; }
        }
    }
