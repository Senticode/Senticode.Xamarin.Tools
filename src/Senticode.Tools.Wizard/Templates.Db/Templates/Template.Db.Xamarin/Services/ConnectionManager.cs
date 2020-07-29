using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Senticode.Base.Interfaces;
using Senticode.Base.Services;
using Senticode.Database.Tools.Interfaces;
using Unity;

namespace Template.Db.Xamarin.Services
{
    internal class ConnectionManager : ServiceBase, IConnectionManager
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<Guid, DbContext> _transactionContexts = new Dictionary<Guid, DbContext>();

        public ConnectionManager(IUnityContainer container)
        {
            _container = container
                .RegisterInstance(this)
                .RegisterInstance<IConnectionManager>(this);
        }

        public override IResult Initialize() => base.Initialize(() =>
        {
            if (!_container.IsRegistered<DatabaseContext>())
            {
                _container.RegisterSingleton<DatabaseContext>();
            }
        });

        public override IResult Release() => base.Release(() =>
        {
            foreach (var context in _transactionContexts.Values)
            {
                try
                {
                    context.Dispose();
                }
                catch
                {
                    // ignored
                }
            }

            _transactionContexts.Clear();
        });

        public DbContext GetDbContext() => new DatabaseContext();

        public DbContext GetDbStrongContext() => _container.Resolve<DatabaseContext>();

        public DbContext GetDbContext(Guid transactionId)
        {
            var shouldCreateNewContext = true;

            if (_transactionContexts.TryGetValue(transactionId, out var context))
            {
                try
                {
                    _ = context.Database; // check if context is not disposed
                    shouldCreateNewContext = false;
                }
                catch
                {
                    // ignored
                }
            }

            if (shouldCreateNewContext)
            {
                context = new DatabaseContext();
                _transactionContexts.Add(transactionId, context);
            }

            return context;
        }

        public void ReleaseDbContext(Guid transactionId)
        {
            if (!_transactionContexts.TryGetValue(transactionId, out var context))
            {
                return;
            }

            _transactionContexts.Remove(transactionId);

            try
            {
                context.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        public Guid CreateNewTransaction()
        {
            var transactionId = Guid.NewGuid();
            _transactionContexts.Add(transactionId, new DatabaseContext());

            return transactionId;
        }
    }
}