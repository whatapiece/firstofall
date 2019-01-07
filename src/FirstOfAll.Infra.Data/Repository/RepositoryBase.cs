using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FirstOfAll.Domain.Interfaces;
using FirstOfAll.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirstOfAll.Infra.Data.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly FirstOfAllContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryBase(FirstOfAllContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity objModel)
        {
            DbSet.Add(objModel);
            _context.SaveChanges();
        }

        public void AddRange(IList<TEntity> objModel)
        {
            DbSet.AddRange(objModel);
            _context.SaveChanges();
        }

        public int Count()
        {
            return DbSet.Count();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public TEntity GetId(Guid id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public void Update(TEntity objModel)
        {
            DbSet.Update(objModel);
            _context.SaveChanges();
        }

        public void Remove(TEntity objModel)
        {
            DbSet.Remove(objModel);
            _context.SaveChanges();
        }
        public void Remove(Guid id)
        {
            var obj = DbSet.Find(id);
            Remove(obj);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
