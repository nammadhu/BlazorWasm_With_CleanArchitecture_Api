namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    public class DeleteTownCardTypeCommand : IRequest<BaseResult>
        {
        public int Id { get; set; }
        }
    }
