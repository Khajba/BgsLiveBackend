using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Entities
{
    public class LogError
    {
        public DateTime LogDate { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
