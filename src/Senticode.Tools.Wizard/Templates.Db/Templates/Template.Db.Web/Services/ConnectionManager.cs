using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Senticode.Base.Interfaces;
using Senticode.Base.Services;
using Senticode.Database.Tools.Interfaces;

namespace Template.Db.Web.Services
{
    internal class ConnectionManager : ServiceBase, IConnectionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _services;
        private readonly Dictionary<Guid, DbContext> _transactionContexts = new Dictionary<Guid, DbContext>();

        public ConnectionManager(IServiceCollection services)
        {
            _services = services;
            _serviceProvider = services.BuildServiceProvider();
        }

        public override IResult Initialize() => base.Initialize(() => _services.TryAddScoped<DatabaseContext>());

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

        public DbContext GetDbStrongContext() => _serviceProvider.GetService<DatabaseContext>();

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