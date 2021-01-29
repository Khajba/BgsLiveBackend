using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Infrastructure.Models
{
    public abstract class CommandModel
    {
        public string Referrer { get; set; }

        public string CorrelationId { get; set; }
    }
}
