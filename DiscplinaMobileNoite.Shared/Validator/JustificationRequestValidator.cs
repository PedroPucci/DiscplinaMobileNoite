using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Shared.Enums;
using DiscplinaMobileNoite.Shared.Helpers;
using FluentValidation;

namespace DiscplinaMobileNoite.Shared.Validator
{
    public class JustificationRequestValidator : AbstractValidator<JustificationEntity>
    {
        public JustificationRequestValidator()
        {
            RuleFor(p => p.Reason)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_EmailCanNotBeNullOrEmpty.Description());
        }
    }
}