using System.Threading.Tasks;

namespace CommandHandler.Handlers
{
    public record Result
    {
        public bool Succeeded { get; init; }
        public string ErrorMessage { get; init; }

        public static Result Success() => new() { Succeeded = false };
        public static Result Failed(string errorMessage) => new() { Succeeded = false, ErrorMessage = errorMessage };
    }

    public record Result<T> : Result
    {
        public T ResultObject { get; init; }
    }

    public static class ObjectResultExtensions
    {
        public static Result<TResultObj> ToSuccessfulResult<TResultObj>(this TResultObj result)
        {
            return new()
            {
                Succeeded = true,
                ResultObject = result
            };
        }
    }
}