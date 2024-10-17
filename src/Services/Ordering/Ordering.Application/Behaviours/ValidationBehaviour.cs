﻿using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;
namespace Ordering.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResuls = await Task
                .WhenAll(_validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResuls.SelectMany(r => r.Errors)
                .Where(f => f != null).ToList();

            if (failures.Any())
                throw new ValidationException(failures);

        }
        return await next();
    }
}
