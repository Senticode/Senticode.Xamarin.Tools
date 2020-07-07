using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Senticode.Base;
using Senticode.Base.Exceptions;
using Senticode.Base.Interfaces;
using Senticode.Database.Tools.Interfaces;

namespace Senticode.Database.Tools
{
    public class DatabaseService<T, TKey> : IDatabaseService<T, TKey>
        where T : class, IEntity<TKey> where TKey : struct
    {
        private readonly IConnectionManager _connectionManager;
        private protected Guid _transactionId = Guid.Empty;
        private protected bool IsTransaction = false;

        public DatabaseService(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task<IResult<T>> FindAsync(TKey id, bool withStrongContext = false, params string[] includes)
        {
            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    if (includes == null || includes?.Length == 0)
                    {
                        return new Result<T>(await db.Set<T>().FindAsync(id));
                    }

                    var query = db.Set<T>();

                    foreach (var include in includes)
                    {
                        query = (DbSet<T>) query.Include(include);
                    }

                    return new Result<T>(await query.FirstOrDefaultAsync(x => x.Id.Equals(id)));
                }

                using (var db = _connectionManager.GetDbContext(Config))
                {
                    if (includes == null || includes?.Length == 0)
                    {
                        return new Result<T>(await db.Set<T>().FindAsync(id));
                    }

                    var query = db.Set<T>();

                    foreach (var include in includes)
                    {
                        query = (DbSet<T>) query.Include(include);
                    }

                    return new Result<T>(await query.FirstOrDefaultAsync(x => x.Id.Equals(id)));
                }
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public async Task<IResult<IEnumerable<T>>> GetAsync(Expression<Func<T, bool>> condition = null,
            bool withStrongContext = false, params string[] includes)
        {
            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    IQueryable<T> query = db.Set<T>();
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                    if (condition != null)
                    {
                        query = query.Where(condition);
                    }

                    return new Result<IEnumerable<T>>(await query.ToListAsync());
                }

                using (var db = _connectionManager.GetDbContext(Config))
                {
                    IQueryable<T> query = db.Set<T>();
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                    if (condition != null)
                    {
                        query = query.Where(condition);
                    }

                    return new Result<IEnumerable<T>>(await query.ToListAsync());
                }
            }
            catch (Exception e)
            {
                return new Result<IEnumerable<T>>(e);
            }
        }

        public async Task<IResult<T>> SaveAsync(T entity, bool withStrongContext = false)
        {
            if (entity == null)
            {
                return new Result<T>(new ArgumentNullException(nameof(entity)));
            }

            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    if (!entity.Id.Equals(default(TKey)))
                    {
                        var findResult = await db.Set<T>().FindAsync(entity.Id);

                        if (findResult != null)
                        {
                            return await UpdateAsync(entity, true);
                        }
                    }

                    await db.Set<T>().AddAsync(entity);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }

                    return new Result<T>(entity);
                }

                using (var db = _connectionManager.GetDbContext(Config))
                {
                    if (!entity.Id.Equals(default(TKey)))
                    {
                        var findResult = await db.Set<T>().FindAsync(entity.Id);

                        if (findResult != null)
                        {
                            return await UpdateAsync(entity);
                        }
                    }

                    await db.Set<T>().AddAsync(entity);
                    await db.SaveChangesAsync();
                }

                return new Result<T>(entity);
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public async Task<IResult<IEnumerable<T>>> SaveAsync(IEnumerable<T> entities, bool withStrongContext = false)
        {
            if (entities == null)
            {
                return new Result<IEnumerable<T>>(new ArgumentNullException(nameof(entities)));
            }

            try
            {
                var enumeratedItems = entities as T[] ?? entities.ToArray();

                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    await db.Set<T>().AddRangeAsync(enumeratedItems);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        await db.Set<T>().AddRangeAsync(enumeratedItems);
                        await db.SaveChangesAsync();
                    }
                }

                return new Result<IEnumerable<T>>(enumeratedItems);
            }
            catch (Exception e)
            {
                return new Result<IEnumerable<T>>(e);
            }
        }

        public async Task<IResult> DeleteAsync(T entity, bool withStrongContext = false)
        {
            if (entity == null)
            {
                return new Result(new ArgumentNullException(nameof(entity)));
            }

            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    db.Set<T>().Remove(entity);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        db.Set<T>().Remove(entity);
                        await db.SaveChangesAsync();
                    }
                }

                return Result.Successful;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        public async Task<IResult> DeleteAsync(TKey id, bool withStrongContext = false)
        {
            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    var dbSet = db.Set<T>();
                    var foundObj = await dbSet.FindAsync(id);

                    if (foundObj == null)
                    {
                        return new Result<IEnumerable<T>>(new NotFoundException($"Id {id} not found in database"));
                    }

                    dbSet.Remove(foundObj);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        var dbSet = db.Set<T>();
                        var foundObj = await dbSet.FindAsync(id);

                        if (foundObj == null)
                        {
                            return new Result<IEnumerable<T>>(new NotFoundException($"Id {id} not found in database"));
                        }

                        dbSet.Remove(foundObj);
                        await db.SaveChangesAsync();
                    }
                }

                return Result.Successful;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        public async Task<IResult> DeleteAsync(IEnumerable<T> entities, bool withStrongContext = false)
        {
            if (entities == null)
            {
                return new Result(new ArgumentNullException(nameof(entities)));
            }

            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    var dbSet = db.Set<T>();
                    dbSet.RemoveRange(entities);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        var dbSet = db.Set<T>();
                        dbSet.RemoveRange(entities);
                        await db.SaveChangesAsync();
                    }
                }

                return Result.Successful;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        public async Task<IResult> DeleteAllAsync(bool withStrongContext = false)
        {
            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    var dbSet = db.Set<T>();
                    dbSet.RemoveRange(dbSet.ToList());

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        var dbSet = db.Set<T>();
                        dbSet.RemoveRange(dbSet.ToList());
                        await db.SaveChangesAsync();
                    }
                }

                return Result.Successful;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        public async Task<IResult<T>> UpdateAsync(T entity, bool withStrongContext = false)
        {
            if (entity == null)
            {
                return new Result<T>(new ArgumentNullException(nameof(entity)));
            }

            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    db.Set<T>().Update(entity);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        db.Set<T>().Update(entity);
                        await db.SaveChangesAsync();
                    }
                }

                return new Result<T>(entity);
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public async Task<IResult> UpdateAsync(IEnumerable<T> entities, bool withStrongContext = false)
        {
            if (entities == null)
            {
                return new Result(new ArgumentNullException(nameof(entities)));
            }

            try
            {
                if (withStrongContext)
                {
                    var db = _transactionId != Guid.Empty
                        ? _connectionManager.GetDbContext(_transactionId, Config)
                        : _connectionManager.GetDbStrongContext(Config);

                    db.Set<T>().UpdateRange(entities);

                    if (!IsTransaction)
                    {
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    using (var db = _connectionManager.GetDbContext(Config))
                    {
                        db.Set<T>().UpdateRange(entities);
                        await db.SaveChangesAsync();
                    }
                }

                return Result.Successful;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        #region Transactions

        public IDatabaseTransaction<T, TKey> StartNewTransaction()
        {
            var transactionId = _connectionManager.CreateNewTransaction();
            return new DatabaseTransaction<T, TKey>(transactionId, _connectionManager);
        }

        #endregion

        #region Config

        public string Config { get; private protected set; }

        public IDatabaseService<T, TKey> SetConfig(string config = null, bool newInstance = false)
        {
            Config = config;
            if (newInstance)
            {
                var service = new DatabaseService<T, TKey>(_connectionManager) {Config = config};
                return service;
            }

            return this;
        }

        #endregion
    }
}