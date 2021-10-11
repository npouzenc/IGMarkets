using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;
using IGMarkets;

namespace IGMarkets.Tests
{
    public class IGTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsConnected_is_true_once_connected()
        {
            // Arrange
            // Act
            //var ig = IG.Connect("identifier", "password", "aaaaabbbbbcccccddddeeee", isDemo: true);

            // Assert
            Assert.IsTrue(false);
        }
    }
}