using System.Diagnostics;

namespace IGMarkets.API
{
    [DebuggerDisplay("{Bid} / {Ask}")]
    public class PriceData
    {
        public float? Bid { get; set; }
        public float? Ask { get; set; }

        public float? Spread => Bid - Ask;

        public float? MidPrice => (Bid + Ask) / 2;

        public override string ToString()
        {
            return $"{Bid} / {Ask}";
        }
    }
}
