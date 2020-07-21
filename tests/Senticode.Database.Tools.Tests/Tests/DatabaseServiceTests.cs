using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Senticode.Base.Interfaces;
using Senticode.Database.Tools.Tests.Entities;
using Senticode.Database.Tools.Tests.Framework;
using Unity;

namespace Senticode.Database.Tools.Tests.Tests
{
    [TestFixture]
    class DatabaseServiceTests : TestBase
    {
        [SetUp]
        public override async Task SetUp()
        {
            await base.SetUp();
            await CreateTestDataAsync();
        }

        public DatabaseServiceTests()
        {
            WithStrongContext = false;
        }

        [Test]
        public async Task Test0001_FindAsync()
        {
            var projectGuid = new Guid("da2c70af-8962-42a4-a061-77af45d01912");
            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();

            var result = await projectRepository.FindAsync(projectGuid, WithStrongContext);
            Assert.AreEqual(null, result.Object.User);
        }

        [Test]
        public async Task Test0002_FindAsyncWithIncludes()
        {
            var projectGuid = new Guid("da2c70af-8962-42a4-a061-77af45d01912");
            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();

            var result = await projectRepository.FindAsync(projectGuid, WithStrongContext,
                $"{nameof(Project.User)}.{nameof(Project.User.UserProfile)}");
            Assert.AreEqual(projectGuid, result.Object.Id);
            Assert.IsInstanceOf<User>(result.Object.User);
        }

        [Test]
        public async Task Test0003_GetAsync() 
        {
            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();
            var projectResults = await projectRepository.GetAsync(null, WithStrongContext);

            Assert.AreEqual(null, projectResults.Object.FirstOrDefault().User);
        }

        [Test]
        public async Task Test0004_GetAsyncWithInclude() {

            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();

            var projectResults = await projectRepository.GetAsync(null, WithStrongContext,
                $"{nameof(Project.User)}.{nameof(Project.User.UserProfile)}");

            Assert.IsInstanceOf<User>(projectResults.Object.FirstOrDefault().User);
        }

        [Test]
        public async Task Test0005_GetAsyncWithCondition()
        {
            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();

            var projectResults = await projectRepository.GetAsync(
                project => project.Title == "Project2", WithStrongContext);

            Assert.AreEqual(projectResults.Object.FirstOrDefault().Title, "Project2");
        }

        [Test]
        public async Task Test0006_SaveExistingEntity()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();

            var notifResults = await notificationRepository.GetAsync();
            var notifCount = notifResults.Object.Count();

            var notification = new Notification
            {
                Id = new Guid("1a729ea8-0897-422f-a9a8-796ec922343b"),
                Message = "Message Edited"
            };
            await notificationRepository.SaveAsync(notification, WithStrongContext);

            var notifResults2 = await notificationRepository.GetAsync();
            var notifCount2 = notifResults2.Object.Count();
            Assert.AreEqual(notifCount2, notifCount);
        }

        [Test]
        public async Task Test0007_SaveEntityAsync()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Message = "Saved Message"
            };
            var result = await notificationRepository.SaveAsync(notification, WithStrongContext);

            Assert.IsTrue(result.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public async Task Test0008_SaveEntity_should_be_failed()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var notification = new Notification();
            notification = null;
            var result = await notificationRepository.SaveAsync(notification, WithStrongContext);
            Assert.IsFalse(result.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public async Task Test0009_SaveToDatabase()
        {
            var messages = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    Message = "Msg3",
                },
                new Notification
                {
                    Id = Guid.NewGuid(),
                    Message = "Msg4",
                }
            };
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.SaveAsync(messages, WithStrongContext);
            Assert.IsTrue(result.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public async Task Test0010_SaveToDatabase_should_be_failed()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var list = new List<Notification>();
            list = null;
            var result = await notificationRepository.SaveAsync(list, WithStrongContext);
            Assert.IsFalse(result.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public async Task Test0011_DeleteAllAsync()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.DeleteAllAsync(WithStrongContext);
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task Test0012_UpdateEntityAsync()
        {
            var notificationGuid = new Guid("0a0a57de-820c-4abd-b58a-a6423cf9a18c");
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.FindAsync(notificationGuid, WithStrongContext);
            result.Object.Message = "New Title";
            await notificationRepository.UpdateAsync(result.Object, WithStrongContext);
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task Test0013_DeleteEntity()
        {
            var notificationGuid = new Guid("0a0a57de-820c-4abd-b58a-a6423cf9a18c");
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.FindAsync(notificationGuid, WithStrongContext);
            var deletionResult = await notificationRepository.DeleteAsync(result.Object, WithStrongContext);
            Assert.IsTrue(deletionResult.IsSuccessful);
        }

        [Test]
        public async Task Test0014_DeleteEntityById()
        {
            var notificationGuid = new Guid("1a729ea8-0897-422f-a9a8-796ec922343b");
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.DeleteAsync(notificationGuid, WithStrongContext);
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task Test0015_DeleteAsync()
        {
            var notificationGuid = new Guid("1a729ea8-0897-422f-a9a8-796ec922343b");
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.GetAsync();

            var notifsToDelete = result.Object.Take(2);
            var deletionResult = await notificationRepository.DeleteAsync(notifsToDelete, WithStrongContext);
            Assert.IsTrue(deletionResult.IsSuccessful);
        }

        [Test]
        public async Task Test0016_UpdateAsync_should_be_failed()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var list = new List<Notification>();
            list = null;
            var result = await notificationRepository.UpdateAsync(list, WithStrongContext);
            Assert.IsFalse(result.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public async Task Test0017_UpdateAsync()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var result = await notificationRepository.GetAsync();
            var notifsToUpdate = result.Object.Take(2);
            notifsToUpdate.First().Message = "Updated message!";
            var updateResult = await notificationRepository.UpdateAsync(notifsToUpdate, WithStrongContext);
            Assert.IsTrue(updateResult.IsSuccessful, result?.Exception?.Message);
        }

        [Test]
        public void Test0018_StartNewTransaction()
        {
            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();
            var transaction = notificationRepository.StartNewTransaction();
            Assert.IsInstanceOf<IDatabaseTransaction<Notification, Guid>>(transaction);
        }

        protected async Task CreateTestDataAsync()
        {
            //create user
            var guid = new Guid("7987ad64-531c-4fa3-8a17-6fd52b2defcd");
            var userRepository = _container.Resolve<IDatabaseService<User, Guid>>();
            var result = await userRepository.FindAsync(guid);
            if (result.Object == null)
            {
                var user = new User
                {
                    Id = guid,
                    Username = "User1",
                    UserProfile = new UserProfile
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Name",
                        LastName = "Surname",
                        UserId = guid.ToString()
                    }
                };
                
                await userRepository.SaveAsync(user);
            }
            
            //create projects
            var projectGuid1 = new Guid("da2c70af-8962-42a4-a061-77af45d01912");
            var projectGuid2 = new Guid("c76b304c-f871-48d4-a69d-0a1fa05a8aa2");

            var projectRepository = _container.Resolve<IDatabaseService<Project, Guid>>();

            var result1 = await projectRepository.FindAsync(projectGuid1);
            if (result1.Object == null)
            {
                var project1 = new Project
                {
                    Id = projectGuid1,
                    Title = "Project2",
                    UserId = guid
                };
                await projectRepository.SaveAsync(project1);
            }

            var result2 = await projectRepository.FindAsync(projectGuid2);
            if (result2.Object == null)
            {
                var project2 = new Project
                {
                    Id = projectGuid2,
                    Title = "Project1",
                    UserId = guid,
                };
                await projectRepository.SaveAsync(project2);
            }

            //create notifications
            var notifGuid1 = new Guid("0a0a57de-820c-4abd-b58a-a6423cf9a18c");
            var notifGuid2 = new Guid("1a729ea8-0897-422f-a9a8-796ec922343b");
            var notifGuid3 = new Guid("8399fce8-46b6-4706-a81d-6bf552e899af");

            var notificationRepository = _container.Resolve<IDatabaseService<Notification, Guid>>();

            var notifResult1 = await notificationRepository.FindAsync(notifGuid1);
            if (notifResult1.Object == null)
            {
                var notification1 = new Notification
                {
                    Id = notifGuid1,
                    Message = "Message One"
                };
                await notificationRepository.SaveAsync(notification1);
            }

            var notifResult2 = await notificationRepository.FindAsync(notifGuid2);
            if (notifResult2.Object == null)
            {
                var notification2 = new Notification
                {
                    Id = notifGuid2,
                    Message = "Message Two"
                };
                await notificationRepository.SaveAsync(notification2);
            }

            var notifResult3 = await notificationRepository.FindAsync(notifGuid3);
            if (notifResult3.Object == null)
            {
                var notification3 = new Notification
                {
                    Id = notifGuid3,
                    Message = "Message Three"
                };
                await notificationRepository.SaveAsync(notification3);
            }
        }
    }
}
