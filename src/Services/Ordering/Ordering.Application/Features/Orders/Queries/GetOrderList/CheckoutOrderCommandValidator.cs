using FluentValidation;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;
using Ordering.Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("{UserName} is required")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters");

        RuleFor(p => p.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required")
            .Must(p => ValidatorHelper.EmailValidator(p)).WithMessage("{EmailAddress} format is wrong");

        RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");
    }   
}
