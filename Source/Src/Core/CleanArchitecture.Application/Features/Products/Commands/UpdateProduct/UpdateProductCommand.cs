using MediatR;
using SharedResponse.Wrappers;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProduct
    {
    public class UpdateProductCommand : IRequest<BaseResult>
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string BarCode { get; set; }
        }
    }
