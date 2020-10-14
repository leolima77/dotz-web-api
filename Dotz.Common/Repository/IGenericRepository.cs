using System;
using System.Linq;

namespace Dotz.Common.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        T Find(Guid Id);

        T Add(T entity);

        T Update(T entityToUpdate);

        void Delete(Guid Id);

        void Delete(T entityToDelete);
    }
}