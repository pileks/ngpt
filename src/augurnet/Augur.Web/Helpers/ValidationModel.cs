namespace Augur.Web.Helpers
{
    public class ValidationModel
    {
        public ValidationModel(object data, bool hasErrors = false)
        {
            this.Data = data;
            this.HasErrors = hasErrors;
        }

        public object Data { get; }
        public bool HasErrors { get; }
    }

    public class ValidationModel<TModel>
    {
        public ValidationModel(TModel data, bool hasErrors = false)
        {
            this.Data = data;
            this.HasErrors = hasErrors;
        }

        public TModel Data { get; }
        public bool HasErrors { get; }
    }
}