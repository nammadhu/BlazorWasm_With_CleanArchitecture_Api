namespace MyTown.SharedModels.Features.Towns.Commands
    {
    public class DeleteTownCommand : IRequest<BaseResult>
        {
        public int Id { get; set; }
        }
    }
