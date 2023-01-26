namespace Augur.Web.Helpers
{
    public class InvalidValidationResult
    {
        public InvalidValidationResult(object data = null)
        {
            this.Data = data;
        }

        public object Data { get; }
    }
}