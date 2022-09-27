using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace IGMarkets.Tests
{
    [TestFixture]
    public class CredentialsTest
    {

        [Test]
        public void Credentials_RaisingArgumentExceptionIfNoLogin()
        {
            Assert.Throws<ArgumentException>(() => new Credentials("", "", "", false));   
        }

        [Test]
        public void Credentials_RaisingArgumentExceptionIfNoPass()
        {
            Assert.Throws<ArgumentException>(() => new Credentials("LOGIN", "", "", false));
        }

        [Test]
        public void Credentials_RaisingArgumentExceptionIfNoKey()
        {
            Assert.Throws<ArgumentException>(() => new Credentials("LOGIN", "a_very_long_string", "", false));
        }

        [Test]
        public void Credentials_WithConfiguration()
        {
            // Arrange
            // The equivalent JSON would be:
            // {
            //  "IGMarkets": {
            //    "identifier": "<<your ID>>",
            //    "password": "<<your PASSWORD>>"
            //    "apiKey": "<<your API KEY>>"
            //  }
            var igConfiguration = new Dictionary<string, string>
            {
                {"IGMarkets:identifier", "Nicolas"},
                {"IGMarkets:password", "p@ssw0rd"},
                {"IGMarkets:apiKey", "zzzzzzzzzzzzzzzzzzzzz"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(igConfiguration)
                .Build();

            // Act
            var credentials = new Credentials(configuration, isDemo: true);

            // Assert
            Assert.IsNotNull(credentials);
            Assert.AreEqual(credentials.Identifier, "Nicolas");
            Assert.AreEqual(credentials.Password, "p@ssw0rd");
            Assert.AreEqual(credentials.ApiKey, "zzzzzzzzzzzzzzzzzzzzz");
            Assert.IsTrue(credentials.IsDemo);
        }
    }
}
