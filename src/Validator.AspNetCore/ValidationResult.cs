namespace Validator.AspNetCore
{
    public sealed record ValidationResult(params ValidationFailure[] Failures)
    {
        public bool IsValid => this.Failures.Length == 0;

        public static readonly ValidationResult ValidResult = new();

        public static ValidationResult GenericFailure(string propertyName)
        {
            return new(ValidationFailure.GenericFailure(propertyName));
        }

        public static ValidationResult Combine(params IEnumerable<ValidationResult> validationResults)
        {
            return new ValidationResult([.. validationResults.SelectMany(x => x.Failures)]);
        }


        public static implicit operator ValidationResult(bool isValid)
        {
            return isValid
                ? ValidResult
                : throw new InvalidOperationException("Cannot convert 'false' to ValidationResult.");
        }
                

    }
}
