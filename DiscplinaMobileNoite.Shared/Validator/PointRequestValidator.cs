using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Shared.Enums;
using DiscplinaMobileNoite.Shared.Helpers;
using FluentValidation;

namespace DiscplinaMobileNoite.Shared.Validator
{
    public class PointRequestValidator : AbstractValidator<PointEntity>
    {
        public PointRequestValidator()
        {
            RuleFor(p => p.MorningEntry)
                .NotEmpty()
                    .WithMessage(PointErrors.AttendanceRecord_Error_CheckInCanNotBeNullOrEmpty.Description());
        }
    }
}
