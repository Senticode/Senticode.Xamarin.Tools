using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senticode.Base.Interfaces
{
    /// <summary>
    ///     Generic CRUD-service.
    ///     The base implementation is contained in the assembly Senticode.Database.Tools.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <typeparam name="TKey">Type of entity identifier</typeparam>
    public interface IDatabaseService<T, in TKey> where T : class, IEntity<TKey> where TKey : struct
    {
        IDatabaseTransaction<T, TKey> StartNewTransaction();

        #region Config

        string Config { get; }

        IDatabaseService<T, TKey> SetConfig(string config = null, bool newInstance = false);

        #endregion

        #region CRUD operations

        Task<IResult<T>> FindAsync(TKey id, bool withStrongContext = false, params string[] includes);

        Task<IResult<IEnumerable<T>>> GetAsync(Expression<Func<T, bool>> condition = null,
            bool withStrongContext = false, params string[] includes);

        Task<IResult<T>> SaveAsync(T entity, bool withStrongContext = false);

        Task<IResult<IEnumerable<T>>> SaveAsync(IEnumerable<T> entities, bool withStrongContext = false);

        Task<IResult> DeleteAsync(T entity, bool withStrongContext = false);

        Task<IResult> DeleteAsync(TKey id, bool withStrongContext = false);

        Task<IResult> DeleteAsync(IEnumerable<T> entities, bool withStrongContext = false);

        Task<IResult> DeleteAllAsync(bool withStrongContext = false);

        Task<IResult<T>> UpdateAsync(T entity, bool withStrongContext = false);

        Task<IResult> UpdateAsync(IEnumerable<T> entities, bool withStrongContext = false);

        #endregion
    }

    /// <summary>
    ///     Generic CRUD-service for dedicated transaction.
    ///     The base implementation is contained in the assembly Senticode.Database.Tools.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <typeparam name="TKey">Type of entity identifier</typeparam>
    public interface IDatabaseTransaction<T, in TKey> : IDisposable, IDatabaseService<T, TKey>
        where T : class, IEntity<TKey> where TKey : struct
    {
        Guid TransactionId { get; }
        Task<IResult> EndTransactionAsync();
    }
}