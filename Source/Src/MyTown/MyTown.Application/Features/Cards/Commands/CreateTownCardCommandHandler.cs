using MyTown.SharedModels.Features.Cards.Commands;

namespace MyTown.Application.Features.Cards.Commands
    {
    public class CreateTownCardCommandHandler(ITownCardRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTownCardCommand, BaseResult<int>>
        {
        public async Task<BaseResult<int>> Handle(CreateTownCardCommand request, CancellationToken cancellationToken)
            {
            var obj = request.To<CreateTownCardCommand, TownCard>();
            //todo should modify above 
            //var product = new TownCard(request.Name, request.Price, request.BarCode);

            await townCardTypeMasteDataRepository.AddAsync(obj);
            bool success = await unitOfWork.SaveChangesAsync();

            return new BaseResult<int>(obj.Id) { Success = success };
            }
        }
    }