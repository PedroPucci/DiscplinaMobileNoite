using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Shared.Enums;
using DiscplinaMobileNoite.Shared.Helpers;
using FluentValidation;

namespace DiscplinaMobileNoite.Shared.Validator
{
    public class UserRequestValidator : AbstractValidator<UserEntity>
    {
        public UserRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_EmailCanNotBeNullOrEmpty.Description())
                .MinimumLength(4)
                    .WithMessage(UserErrors.User_Error_EmailLenghtLessFour.Description())
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                        .WithMessage(UserErrors.User_Error_InvalidEmailFormat.Description());

            RuleFor(p => p.FullName)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_FullNameCanNotBeNullOrEmpty.Description())
                .MinimumLength(10)
                    .WithMessage(UserErrors.User_Error_FullNameLenghtLessFour.Description());

            RuleFor(p => p.Workload)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_WorkloadCanNotBeNullOrEmpty.Description())
                .GreaterThan(0)
                    .WithMessage(UserErrors.User_Error_WorkloadMustBeGreaterThanZero.Description());

            RuleFor(p => p.Password)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_PasswordCanNotBeNullOrEmpty.Description());

            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_PhoneNumberCanNotBeNullOrEmpty.Description());
        }
    }
}