using FluentValidation;
using SharedResponse;

namespace MyTown.SharedModels.Features.Towns.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCommandValidator : AbstractValidator<CreateTownCommand>
        {
        public CreateTownCommandValidator(ITranslator translator)
            {
            //RuleFor(p => p.MyProperty)
            //    .NotNull()
            //    .WithName(p => translator[nameof(p.MyProperty)]);
            }
        }
    }