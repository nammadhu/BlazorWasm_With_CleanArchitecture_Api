using FluentValidation;
using SharedResponse;

namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class CreateTownCardCommandValidator : AbstractValidator<CreateTownCardCommand>
        {
        public CreateTownCardCommandValidator(ITranslator translator)
            {
            //RuleFor(p => p.MyProperty)
            //    .NotNull()
            //    .WithName(p => translator[nameof(p.MyProperty)]);
            }
        }
    }