using DiscplinaMobileNoite.Domain.Entity;
using FluentValidation;

namespace DiscplinaMobileNoite.Shared.Validator
{
    public class PointRequestValidator : AbstractValidator<PointEntity>
    {
        public PointRequestValidator()
        {
        }
    }
}
