﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Helpers;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckOutOrder;

public class CheckoutOrderCommand : Order, IRequest<int>
{ }

public class CheckOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    readonly IMapper _mapper;
    readonly IOrderRepository _orderRepository;
    readonly IEmailServiceHelper _emailService;
    readonly ILogger<CheckOrderCommandHandler> _logger;

    public CheckOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, IEmailServiceHelper emailService, ILogger<CheckOrderCommandHandler> logger)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var newOrder = await _orderRepository.AddAsync(orderEntity);
        _logger.LogInformation($"Order {newOrder.Id} is successfully created.");
        await _emailService.SendMail(newOrder);
        return newOrder.Id;
    }

    
}