using System;
using Senticode.Base.Interfaces;
using Senticode.Database.Tools.Interfaces;
using Senticode.Database.Tools.Tests.Entities;
using Unity;

namespace Senticode.Database.Tools.Tests.Framework
{
    public class TestInitializer
    {
        public IUnityContainer Initialize(IUnityContainer container)
        {
            container.RegisterType<IDatabaseService<Notification, Guid>, DatabaseService<Notification, Guid>>()
                .RegisterType<IDatabaseService<User, Guid>, DatabaseService<User, Guid>>()
                .RegisterType<IDatabaseService<Project, Guid>, DatabaseService<Project, Guid>>()
                .RegisterType<IDatabaseService<UserProfile, Guid>, DatabaseService<UserProfile, Guid>>()
                .RegisterType<IConnectionManager, ConnectionManager>();

            return container;
        }
    }
}
