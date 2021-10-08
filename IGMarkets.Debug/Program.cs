using Autofac;
using IGMarkets.Core;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace IGMarkets.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            // Autofac container.
            var builder = new ContainerBuilder();
            builder.RegisterType<FibonacciService>().As<IService>();

            // Create Logger<T> when ILogger<T> is required.
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            // Use NLogLoggerFactory as a factory required by Logger<T>.
            builder.RegisterType<NLogLoggerFactory>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Finish registrations and prepare the container that can resolve things.
            var container = builder.Build();

            // Entry point. This provides our logger instance to a Service's constructor.
            var service = container.Resolve<IService>();

            // Run.
            int result = service.Generate();
            Console.WriteLine(result);
        }
    }
}
