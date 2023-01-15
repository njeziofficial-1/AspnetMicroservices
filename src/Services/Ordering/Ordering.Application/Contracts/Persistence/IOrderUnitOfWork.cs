using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderUnitOfWork
    {
        IAsyncRepository<T> Repository<T>() where T : EntityBase;
        Task SaveAsync();
        void Save();
    }
}
