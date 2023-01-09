using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        //collect all ivalidator objects from the assembly
        //using the reflection of the fluent validator
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        //next means,, after performing the validation behaviour, you can call the next method
        //in order to run another behaviour
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //look for the actual validation pipeline, using the fluent validation
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                //select all the validators one by one and perform the validate Async menthod
                //ValidateAsync runs the validation for any ivalidator classes
                //eg, it will check the statementcommandvalidator and run all the methods in it
                //after that, it moves to the statementcommandvalidator
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                //check if any failures/errors
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                //if error occured
                if (failures.Count > 0)
                    throw new ValidationException(failures);
            }
            return await next();//move to the next validator class
        }


    }

}
