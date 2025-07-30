namespace Validator.AspNetCore
{
    public sealed record ValidationFailure(string PropertyName, string ErrorCode, string ErrorMessage)
    {
        public static ValidationFailure GenericFailure(string propertyName)
        {
            return new ValidationFailure(propertyName, "GenericError", $"{propertyName} is invalid.");
        }

        public static implicit operator ValidationResult(ValidationFailure validationFailure)
        {
            return new(validationFailure);
        }
    }
}
