using FluentValidation;
using MyTown.SharedModels.Features.Towns.Commands;

namespace MyTown.SharedModels.Features.Towns.Queries
    {
    public class GetTownByIdQuery : IRequest<BaseResult<TownDto>>
        {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        }

    public class GetTownByIdQueryValidator : AbstractValidator<GetTownByIdQuery>
        {
        public GetTownByIdQueryValidator()//(ITranslator translator)
            {
            RuleFor(p => p.Id>0);
            //.WithName(p => translator[nameof(p.Name)]);
            //RuleFor(p => p.ShortName)
            //    .NotNull();
            //.WithName(p => translator[nameof(p.ShortName)]);
            }
        }
    }
