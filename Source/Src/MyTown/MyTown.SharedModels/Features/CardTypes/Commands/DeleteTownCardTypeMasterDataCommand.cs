namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    public class DeleteTownCardTypeMasterDataCommand : IRequest<BaseResult>
        {
        public int Id { get; set; }
        }
    }
