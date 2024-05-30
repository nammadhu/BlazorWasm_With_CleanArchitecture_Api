using FluentValidation;
using SharedResponse;

namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class UpdateTownCardTypeMasterDataCommandValidator : AbstractValidator<UpdateTownCardTypeMasterDataCommand>
        {
        public UpdateTownCardTypeMasterDataCommandValidator(ITranslator translator)
            {
            //    RuleFor(p => p.MyProperty)
            //        .NotNull()
            //        .WithName(p => translator[nameof(p.MyProperty)]);
            }
        }
    }