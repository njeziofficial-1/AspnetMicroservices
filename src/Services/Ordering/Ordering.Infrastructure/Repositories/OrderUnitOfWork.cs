using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderUnitOfWork : IOrderUnitOfWork
    {
        readonly OrderContext _orderContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public OrderUnitOfWork(OrderContext orderContext)
        {
            _orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
        }

        public Dictionary<Type, object> Repositories
        {
            get => _repositories;
            set => Repositories = value;
        }

        public IAsyncRepository<T> Repository<T>() where T : EntityBase 
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IAsyncRepository<T>;
            }
            IAsyncRepository<T> repo = new RepositoryBase<T>(_orderContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public void Save()
        {
            _orderContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _orderContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _orderContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
