
namespace Domain_Layer.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = [];
        public T? Data { get; set; }

        public static OperationResult<T> Success(T data)
            => new() { IsSuccess = true, Data = data };

        public static OperationResult<T> Failure(params string[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };
    }
}
