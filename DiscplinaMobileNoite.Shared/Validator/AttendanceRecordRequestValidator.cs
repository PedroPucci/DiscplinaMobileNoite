using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Shared.Enums;
using DiscplinaMobileNoite.Shared.Helpers;
using FluentValidation;

namespace DiscplinaMobileNoite.Shared.Validator
{
    public class AttendanceRecordRequestValidator : AbstractValidator<PointEntity>
    {
        public AttendanceRecordRequestValidator()
        {
            RuleFor(p => p.MorningEntry)
                .NotEmpty()
                    .WithMessage(AttendanceRecordErrors.AttendanceRecord_Error_CheckInCanNotBeNullOrEmpty.Description());
        }
    }
}
