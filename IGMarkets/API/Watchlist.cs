using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    [DebuggerDisplay("{Id}= Name:{Name}, Editable:{Editable}")]
    public class Watchlist
    {
        ///<Summary>
        /// Watchlist identifier
        ///</Summary>
        public string Id { get; set; }
        ///<Summary>
        /// Watchlist name
        ///</Summary>
        public string Name { get; set; }
        ///<Summary>
        /// True if this watchlist can be altered by the user
        ///</Summary>
        public bool Editable { get; set; }
        ///<Summary>
        /// True if this watchlist can be deleted by the user
        ///</Summary>
        public bool Deleteable { get; set; }
        ///<Summary>
        /// True if this watchlist doesn't belong to the user, but rather is a system
        /// Predefined one
        ///</Summary>
        public bool DefaultSystemWatchlist { get; set; }
    }
}
