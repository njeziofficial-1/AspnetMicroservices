using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    readonly IOrderRepository _orderRepository;
    readonly ILogger<DeleteOrderCommandHandler> _logger;
    readonly IMapper _mapper;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Order), request.Id);
        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation($"Order {orderToDelete.Id} deleted successfully");
        return Unit.Value;
    }
}
