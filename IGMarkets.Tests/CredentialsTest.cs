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
    }
}
