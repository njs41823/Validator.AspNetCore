using System.ComponentModel.DataAnnotations;
using Validator.AspNetCore;
using Validator.AspNetCore.Rules;
using Web.Contracts;
using Web.Services;

namespace Web.Validators
{
    internal sealed class PersonValidator : AbstractValidator<Person>
    {
        // inject services into custom validators
        private readonly IForbiddenIdAccessor forbiddenIdAccessor;

        public PersonValidator(IForbiddenIdAccessor forbiddenIdAccessor)
        {
            this.forbiddenIdAccessor = forbiddenIdAccessor;

            this
                .RuleFor(x => x.Id)
                .NotNull()
                // using a custom rule
                .RejectHugeIds()
                .GreaterThan(0);

            this
                .RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            this
                .RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            // validate a property using a specific IValidator
            this
                .RuleFor(x => x.MailingAddress)
                .SetValidator(new AddressValidator());

            this
                .RuleFor(x => x.Id)
                .AddRule(validationContext =>
                {
                    var forbiddenId = this.forbiddenIdAccessor.GetForbiddenId();

                    if (validationContext.PropertyValue != forbiddenId)
                    {
                        return true;
                    }

                    return new ValidationFailure(
                        PropertyName: validationContext.PropertyName,
                        ErrorCode: "ForbiddenIdRule",
                        ErrorMessage: $"{validationContext.PropertyName} cannot be equal to {forbiddenId}.");
                });
        }
    }
}
