namespace Senticode.Database.Tools.Tests.Tests
{
    class DatabaseTestsWithStrongContext : DatabaseServiceTests
    {
        public DatabaseTestsWithStrongContext()
        {
            WithStrongContext = true;
        }
    }
}
