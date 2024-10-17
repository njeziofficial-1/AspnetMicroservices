using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Helpers;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : Order, IRequest<Unit>
{
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    readonly IMapper _mapper;
    readonly IOrderRepository _orderRepository;
    readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToUpdate == null)
        {
            _logger.LogError("Order does not exist in database");
            //throw new ArgumentException(nameof(Order), request.Id };
            return Unit.Value;
        }

        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
        await _orderRepository.UpdateAsync(orderToUpdate);
        _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated");
        return Unit.Value;
    }
}

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
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
