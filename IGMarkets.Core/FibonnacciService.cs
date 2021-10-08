using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.Core
{
    public class FibonacciService : IService
    {
        private ILogger<FibonacciService> logger;

        public FibonacciService(ILogger<FibonacciService> logger)
        {
            this.logger = logger;
        }

        public int Generate()
        {
            int a = 0;
            int b = 1;
            int tmp;

            logger.LogInformation("Generate() called.");
            for (int i = 0; i < 12; i++)
            {
                tmp = a;
                a = b;
                b += tmp;
            }

            return a;
        }
    }
}
