using MyTown.SharedModels.Features.Cards.Commands;
using SharedResponse;

namespace MyTown.Application.Features.Cards.Commands
    {
    public class DeleteTownCardCommandHandler(ITownCardRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork, ITranslator translator) : IRequestHandler<DeleteTownCardCommand, BaseResult>
        {
        public async Task<BaseResult> Handle(DeleteTownCardCommand request, CancellationToken cancellationToken)
            {
            var data = await townCardTypeMasteDataRepository.GetByIdIntAsync(request.Id);

            if (data is null)
                {
                return new BaseResult(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            townCardTypeMasteDataRepository.Delete(data);
            await unitOfWork.SaveChangesAsync();

            return new BaseResult();
            }
        }
    }
