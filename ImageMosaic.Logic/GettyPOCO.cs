using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageMosaic.Logic
{
    class SimpleGettyImageThumb
    {
        public string uri { get; set; }
    }

    class SimpleGettyImage
    {
        // Default image thumb
        const string CAT_PICTURE_IRI = "http://s3.amazonaws.com/digitaltrends-uploads-prod/2013/10/grumpy-confused-cat1.jpg";

        public string id { get; set; }
        public string title { get; set; }

        public List<SimpleGettyImageThumb> display_sizes { get; set; }

        public string FindBestDisplayURI()
        {
            if (HaveAtLeastOneAvailableThumb())
            {
                return display_sizes[0].uri;
            }

            return CAT_PICTURE_IRI;
        }

        public string FileName
        {
            get
            {
                return id + ".jpg";
            }
        }

        private bool HaveAtLeastOneAvailableThumb()
        {
            return display_sizes.Count > 0;
        }
    }

    class GettyResult
    {
        public List<SimpleGettyImage> images { get; set; }
    }
}
