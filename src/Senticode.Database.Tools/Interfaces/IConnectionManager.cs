using System;
using Microsoft.EntityFrameworkCore;
using Senticode.Base.Interfaces;

namespace Senticode.Database.Tools.Interfaces
{
    /// <summary>
    ///     This service inject DbContext into <see cref="IDatabaseService{T,TKey}" />.
    ///     You should implement this interface and register in your IoC Container before start to use
    ///     <see cref="DatabaseService{T,TKey}" /> or <see cref="DatabaseTransaction{T,TKey}" />.
    /// </summary>
    public interface IConnectionManager : IService
    {
        /// <summary>
        ///     This method gets context which you can use into using-block as Disposable.
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();

        /// <summary>
        ///     This method gets DbContext which you must not Dispose. This is singleton for DBContext.
        /// </summary>
        /// <returns></returns>
        DbContext GetDbStrongContext();

        /// <summary>
        ///     This method gets DbContext for dedicated transaction with specified TransactionId.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        DbContext GetDbContext(Guid transactionId);

        /// <summary>
        ///     Release DbContext for dedicated transaction with specified TransactionId.
        ///     This method also call <see cref="DbContext.Dispose" />.
        /// </summary>
        /// <param name="transactionId"></param>
        void ReleaseDbContext(Guid transactionId);

        /// <summary>
        ///     Creates new DbContext for dedicated transaction with specified Transaction Id.
        ///     Then the created DbContext can be obtained by the <see cref="IConnectionManager.GetDbContext(Guid)" />
        ///     method using the TransactionId that is returned by this method.
        /// </summary>
        Guid CreateNewTransaction();
    }
}