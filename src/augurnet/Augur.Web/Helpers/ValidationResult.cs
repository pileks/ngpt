using Microsoft.AspNetCore.Mvc;

namespace Augur.Web.Helpers
{
    public class ValidationResult : ObjectResult, IValidationResult
    {
        public ValidationResult(object data = null, bool hasErrors = false, int statusCode = 200) : base(
            new ValidationModel(data, hasErrors))
        {
            this.StatusCode = statusCode;
        }

        public static implicit operator ValidationResult(StatusCodeResult result)
        {
            return new ValidationResult(data: null, hasErrors: false, result.StatusCode);
        }

        public static implicit operator ValidationResult(OkObjectResult result)
        {
            return new ValidationResult(result.Value, hasErrors: false,
                result.StatusCode.HasValue ? 200 : result.StatusCode.Value);
        }

        public static implicit operator ValidationResult(InvalidValidationResult result)
        {
            return new ValidationResult(result.Data, true, 200);
        }
    }

    public class ValidationResult<TModel> : ValidationResult
    {
        public ValidationResult(TModel data, bool hasErrors = false, int statusCode = 200) : base(data, hasErrors,
            statusCode)
        {
        }

        public static implicit operator ValidationResult<TModel>(TModel value)
        {
            return new ValidationResult<TModel>(value);
        }

        public static implicit operator ValidationResult<TModel>(StatusCodeResult result)
        {
            return new ValidationResult<TModel>(default(TModel), hasErrors: false, result.StatusCode);
        }

        public static implicit operator ValidationResult<TModel>(OkObjectResult result)
        {
            return new ValidationResult<TModel>((TModel) result.Value, hasErrors: false,
                result.StatusCode.HasValue ? 200 : result.StatusCode.Value);
        }

        public static implicit operator ValidationResult<TModel>(InvalidValidationResult result)
        {
            return new ValidationResult<TModel>((TModel) result.Data, true, 200);
        }
    }
}