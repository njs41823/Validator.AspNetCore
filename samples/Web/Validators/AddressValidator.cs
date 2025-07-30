using Validator.AspNetCore;
using Validator.AspNetCore.Rules;
using Web.Contracts;

namespace Web.Validators
{
    internal sealed class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            this
                .RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            this
                .RuleFor(x => x.Line1)
                .NotEmpty()
                .MaximumLength(100);

            this
                .RuleFor(x => x.Line2)
                .MaximumLength(100);

            this
                .RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);

            this
                .RuleFor(x => x.State)
                .NotEmpty()
                .MaximumLength(100);

            this
                .RuleFor(x => x.PostalCode)
                .NotEmpty()
                .MaximumLength(10);

            this
                .RuleFor(x => x.CountryCode)
                .NotEmpty()
                .MaximumLength(2);
        }
    }
}
