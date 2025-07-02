using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain_Layer.Models;

namespace Application_Layer.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IOperationResult // Constraint to the new interface
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    // Get the generic type argument of TResponse (e.g., IdeaSessionDto from OperationResult<IdeaSessionDto>)
                    var responseDataType = typeof(TResponse).GenericTypeArguments.FirstOrDefault();
                    if (responseDataType == null)
                    {
                        // Fallback if TResponse is not a generic OperationResult<T>
                        throw new InvalidOperationException("ValidationBehavior expects TResponse to be a generic OperationResult<T>.");
                    }

                    // Use the static Failure method that creates a generic OperationResult<T>
                    return (TResponse)OperationResult<object>.Failure(responseDataType, failures.Select(e => e.ErrorMessage).ToArray());
                }
            }
            return await next();
        }
    }
}
