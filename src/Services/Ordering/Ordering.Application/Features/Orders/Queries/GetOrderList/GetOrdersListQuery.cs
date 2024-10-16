﻿using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class GetOrdersListQuery : IRequest<List<OrdersVm>>
{
    public string UserName { get; set; }

    public GetOrdersListQuery(string userName)
    {
        UserName = userName;
    }
}

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
{
    readonly IOrderRepository _orderRepository;
    readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
        return _mapper.Map<List<OrdersVm>>(orderList);
    }
}