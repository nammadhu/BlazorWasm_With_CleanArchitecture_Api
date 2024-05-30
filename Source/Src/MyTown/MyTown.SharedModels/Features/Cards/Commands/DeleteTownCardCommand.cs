namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class DeleteTownCardCommand : IRequest<BaseResult>
        {
        public int Id { get; set; }
        }
    }
