using Autofac;
using IGMarkets.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace IGMarkets.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var identifier = configuration["IGMarkets:Identifier"];
            var password = configuration["IGMarkets:Password"];
            var apiKey = configuration["IGMarkets:APIKey"];

            var container = BuildContainer();
            var ig = container.Resolve<IG>();

            ig.Login(identifier, password, apiKey, demo: true);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets<Program>()
                .Build();
        }

        private static IContainer BuildContainer()
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
            var container = builder.Build();
            return container;
        }
    }
}
