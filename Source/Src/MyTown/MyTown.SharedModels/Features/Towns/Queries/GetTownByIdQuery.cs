namespace MyTown.SharedModels.Features.Towns.Queries
    {
    public class GetTownByIdQuery : IRequest<BaseResult<TownDto>>
        {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        }
    }
