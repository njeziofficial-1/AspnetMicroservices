using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;
using System.Runtime.InteropServices;

namespace Ordering.API.Mapping
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckOutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
