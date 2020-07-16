using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Senticode.Xamarin.Tools.Core.Configuration;

namespace Senticode.Xamarin.Tools.Core.Tests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var configPath = $"{typeof(Tests).Assembly.GetName().Name}.TestData.Test.config";
            using (var config = typeof(Tests).GetTypeInfo().Assembly.GetManifestResourceStream(configPath))
            {
                ConfigurationManager.Initialize(config);
            }
        }

        [Test]
        public void Test0001_GetSettingByKey()
        {
            var value = ConfigurationManager.AppSettings["setting1"];
            Assert.AreEqual("1", value);
        }

        [Test]
        public void Test0002_GetSettingByMethod()
        {
            var value = ConfigurationManager.GetSection("setting2");
            Assert.AreEqual("2",value);
        }

        [Test]
        public void Test0003_GetDefaultConnectionString()
        {
            var value = ConfigurationManager.GetConnectionString();
            Assert.AreEqual("Default connection string", value);
        }

        [Test]
        public void Test0004_GetConnectionString()
        {
            var value = ConfigurationManager.GetConnectionString("AnotherConnection");
            Assert.AreEqual("Another connection string", value);
        }

        [Test]
        public void Test0005_InitializeTwice()
        {
            Assert.Throws<TypeInitializationException>(() => 
                ConfigurationManager.Initialize(Stream.Null));
        }
    }
}