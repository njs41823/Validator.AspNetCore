namespace Validator.AspNetCore
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T? instance);
    }
}
