using System.Diagnostics;

namespace IGMarkets.API
{
    [DebuggerDisplay("Bid = {Bid}, Ask = {Ask}")]
    public class PriceData
    {
        public float Bid { get; set; }
        public float Ask { get; set; }

        public override string ToString()
        {
            return $"Bid:{Bid} Ask:{Ask}";
        }
    }
}
