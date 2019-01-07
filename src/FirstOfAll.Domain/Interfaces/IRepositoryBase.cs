using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FirstOfAll.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity objModel);
        void AddRange(IList<TEntity> objModel);
        TEntity GetId(Guid id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        int Count();
        void Update(TEntity objModel);
        void Remove(TEntity objModel);
        void Remove(Guid id);
        new void Dispose();

    }
}
