using MediatR;
using SharedResponse.Wrappers;

namespace CleanArchitecture.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<BaseResult>
{
    public long Id { get; set; }
}
