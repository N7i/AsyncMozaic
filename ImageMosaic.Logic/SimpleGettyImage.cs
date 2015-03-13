using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMosaic.Logic
{
    public class SimpleGettyImageThumb
    {
        public string uri { get; set; }
    }

    public class SimpleGettyImage
    {
        public string id { get; set; }
        public string title { get; set; }

        public List<SimpleGettyImageThumb> display_sizes { get; set; }
    }

    public class GettyResult
    {
        public List<SimpleGettyImage> images { get; set; }
    }
}
