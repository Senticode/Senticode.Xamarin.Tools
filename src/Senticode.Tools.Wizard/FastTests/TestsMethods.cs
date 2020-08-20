using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Services.Helpers;

namespace FastTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Test()
        {
            var path = @"C:\Users\vdeviatkin\Desktop\AppSettings.cs";
            FileHelper.UncommentCs(path, ReplacementTokens.WebClientRegistration);
        }
    }
}