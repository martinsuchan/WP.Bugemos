using System;
using System.Collections.Generic;

namespace Bugemos
{
    public class Strip
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }
        public string LocalImage { get; set; }

        public string Description { get; set; }

        public string Serie { get; set; }

        public List<string> Tags { get; set; }

        public string Link { get; set; }
    }
}
