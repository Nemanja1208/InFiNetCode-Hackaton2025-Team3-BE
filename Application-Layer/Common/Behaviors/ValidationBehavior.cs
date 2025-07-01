using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    // Interface for custom validators
    public interface ICustomValidator<TRequest>
    {
        void Validate(TRequest request);
    }

    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<ICustomValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<ICustomValidator<TRequest>> validators = null)
        {
            _validators = validators ?? new List<ICustomValidator<TRequest>>();
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Perform validation using custom validators
            if (_validators != null)
            {
                foreach (var validator in _validators)
                {
                    validator.Validate(request);
                }
            }

            return await next();
        }
    }
}
