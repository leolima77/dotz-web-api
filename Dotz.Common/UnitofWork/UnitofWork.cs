using Dotz.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Dotz.Common.UnitofWork
{

    public class UnitofWork : IUnitofWork
    {
        #region Variables

        private readonly DbContext _context;

        private IDbContextTransaction _transation;
        private bool _disposed;

        #endregion Variables

        #region Constructor

        public UnitofWork(DbContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region BusinessSection

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public bool BeginNewTransaction()
        {
            try
            {
                _transation = _context.Database.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RollBackTransaction()
        {
            try
            {
                _transation.Rollback();
                _transation = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            var transaction = _transation != null ? _transation : _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    if (_context == null)
                        throw new ArgumentException("Context is null");

                    if (ensureAutoHistory)
                        _context.EnsureAutoHistory();

                    int result = _context.SaveChanges();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }

        #endregion BusinessSection

        #region DisposingSection

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
                if (disposing)
                    _context.Dispose();

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DisposingSection
    }
}