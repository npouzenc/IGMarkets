using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets
{
    /// <summary>
    /// Timeframe available at IG Markets. Defines the resolution of requested prices data points.
    /// </summary>
    public enum Timeframe
    {
        SECOND,
        MINUTE,
        MINUTE_2,
        MINUTE_3,
        MINUTE_5,
        MINUTE_10,
        MINUTE_15,
        MINUTE_30,
        HOUR,
        HOUR_2,
        HOUR_3,
        HOUR_4,
        DAY,
        WEEK,
        MONTH
    }
}
