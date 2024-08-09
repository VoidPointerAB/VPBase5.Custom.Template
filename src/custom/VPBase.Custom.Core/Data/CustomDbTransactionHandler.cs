using Microsoft.EntityFrameworkCore.Storage;
using VPBase.Shared.Core.Data;

namespace VPBase.Custom.Core.Data
{
    public class CustomDbTransactionHandler : IDbTransactionHandler
    {
        private readonly ICustomDbFactory _customDbFactory;
        private ICustomStorage _storage;

        public CustomDbTransactionHandler(ICustomDbFactory customDbFactory)
        {
            _customDbFactory = customDbFactory;
        }

        #region IDbTransactionHandler

        public void CreateContext()
        {
            CreateDbContext();
        }

        public void SaveChanges()
        {
            _storage.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            Transaction = _storage.BeginTransaction();
            return Transaction;
        }

        public IDbContextTransaction Transaction { get; private set; }

        #endregion

        public void ApplyContext(ICustomStorage storage)
        {
            _storage = storage;
        }

        public ICustomStorage GetStorage()
        {
            return _storage;
        }

        private void CreateDbContext()
        {
            _storage = _customDbFactory.Create();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _storage.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            Transaction = await _storage.BeginTransactionAsync(cancellationToken);
            return Transaction;
        }
    }
}
