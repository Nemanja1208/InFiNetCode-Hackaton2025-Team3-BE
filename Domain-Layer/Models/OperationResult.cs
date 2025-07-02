
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain_Layer.Models
{
    public interface IOperationResult
    {
        bool IsSuccess { get; set; } // Make settable
        List<string> Errors { get; set; } // Make settable
    }

    public class OperationResult<T> : IOperationResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = [];
        public T? Data { get; set; }

        public static OperationResult<T> Success(T data)
            => new() { IsSuccess = true, Data = data };

        public static OperationResult<T> Failure(params string[] errors)
            => new() { IsSuccess = false, Errors = errors.ToList() };

        // Non-generic static Failure method for convenience in ValidationBehavior
        public static IOperationResult Failure(Type dataType, params string[] errors)
        {
            // Create a generic OperationResult<T> instance using reflection
            // This is a workaround to create a generic instance when T is not known at compile time
            var genericType = typeof(OperationResult<>).MakeGenericType(dataType);
            var instance = Activator.CreateInstance(genericType) as IOperationResult;
            if (instance != null)
            {
                instance.IsSuccess = false;
                instance.Errors.AddRange(errors);
            }
            return instance!;
        }
    }
}
