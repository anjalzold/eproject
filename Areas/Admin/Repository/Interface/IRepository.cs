using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace eproject.Areas.Admin.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task SaveChangesAsync();
    }
}