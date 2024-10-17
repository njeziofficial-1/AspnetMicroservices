using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckOutOrder;

public class CheckoutOrderCommand : Order, IRequest<int>
{ }

public class CheckOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    readonly IMapper _mapper;
    readonly IOrderRepository _orderRepository;
    readonly IEmailService _emailService;
    readonly ILogger<CheckOrderCommandHandler> _logger;

    public CheckOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, IEmailService emailService, ILogger<CheckOrderCommandHandler> logger)
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
        await SendMail(newOrder);
        return newOrder.Id;
    }

    private async Task SendMail(Order newOrder)
    {
        try
        {
            string message = "Order was created";
            var email = new Models.Email
            {
                To = "njezichigozie@yahoo.com",
                Body = message,
                Subject = message
            };
            await _emailService.SendEmailAsync(email);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Order {newOrder.Id} failed due to an error with the mail server: {ex.Message}");
        }
    }
}