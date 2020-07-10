using System;
using System.Threading.Tasks;
using Senticode.Base;
using Senticode.Base.Interfaces;
using Senticode.Database.Tools.Interfaces;

namespace Senticode.Database.Tools
{
    internal class DatabaseTransaction<T, TKey> : DatabaseService<T, TKey>, IDatabaseTransaction<T, TKey>
        where TKey : struct where T : class, IEntity<TKey>
    {
        private readonly IConnectionManager _connectionManager;

        public DatabaseTransaction(Guid transactionId, IConnectionManager connectionManager) : base(connectionManager)
        {
            _transactionId = transactionId;
            _connectionManager = connectionManager;
        }


        public Guid TransactionId => _transactionId;

        public async Task<IResult> EndTransactionAsync()
        {
            try
            {
                await _connectionManager.GetDbContext(TransactionId).SaveChangesAsync();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connectionManager.ReleaseDbContext(TransactionId);
            }
        }
    }
}