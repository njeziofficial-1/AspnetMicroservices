using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository 
    {
        readonly IOrderUnitOfWork _unitOfWork;

        public OrderRepository(IOrderUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
           var orderList = await _unitOfWork.Repository<Order>()
                .AsQueryable()
                .Where(o => o.UserName == userName)
                .ToListAsync();

            return orderList;
        }
    }
}
