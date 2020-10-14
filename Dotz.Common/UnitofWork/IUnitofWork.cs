using Dotz.Common.Repository;
using System;

namespace Dotz.Common.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;

        bool BeginNewTransaction();

        bool RollBackTransaction();

        int SaveChanges(bool ensureAutoHistory);
    }
}