using System.Threading.Tasks;
using NUnit.Framework;
using Senticode.Database.Tools.Interfaces;
using Unity;

namespace Senticode.Database.Tools.Tests.Framework
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IUnityContainer _container;

        public bool WithStrongContext { get; set; }

        public virtual async Task SetUp()
        {
            _container = new UnityContainer();
            new TestInitializer().Initialize(_container);
            await AutoInitDatabase();
        }

        private async Task AutoInitDatabase()
        {
            _container.Resolve<IConnectionManager>().GetDbStrongContext().Database.EnsureCreated();
            await Task.CompletedTask;
        }

        public virtual void TearDown()
        {
            _container?.Dispose();
            _container = null;
        }
    }
}
