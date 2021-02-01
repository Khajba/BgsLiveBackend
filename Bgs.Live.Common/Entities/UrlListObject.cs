using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Entities
{
    public class UrlListObject
    {
        public int Id { get; set; }        

        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsPrimary { get; set; }
    }
}
