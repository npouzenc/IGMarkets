using Autofac;
using IGMarkets.Core;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace IGMarkets.Tests
{
    public class IGTest
    {
        private IContainer container;

        [SetUp]
        public void Setup()
        {
            // Autofac container, registering root object IGMarkets.IG.
            var builder = new ContainerBuilder();
            builder.RegisterType<IG>().AsSelf();

            // Create Logger<T> when ILogger<T> is required.
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            // Use NLogLoggerFactory as a factory required by Logger<T>.
            builder.RegisterType<NLogLoggerFactory>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Finish registrations and prepare the container that can resolve things.
            container = builder.Build();
        }

        [Test]
        public void IsConnected_is_true_once_connected()
        {
            // Arrange
            var ig = container.Resolve<IG>();

            // Act
            ig.Login("identifier", "password", "aaaaabbbbbcccccddddeeee");

            // Assert
            Assert.IsTrue(ig.IsConnected);

        }
    }
}