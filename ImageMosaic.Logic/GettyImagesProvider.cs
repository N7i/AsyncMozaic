using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace ImageMosaic.Logic
{
    public class GettyImagesProvider
    {
        const string SEARCH_IRI = "https://api.gettyimages.com/v3/search/images?";

        public DirectoryInfo DownloadDir { get; private set; }
        string _apiKey;

        public GettyImagesProvider(string pubApiKey)
            : this(
                new DirectoryInfo(Path.Combine(Path.GetTempPath(), "ImageMosaic")),
                pubApiKey)
        {
        }

        public GettyImagesProvider(DirectoryInfo downloadDir, string pubApikey)
        {
            if (!downloadDir.Exists)
            {
                downloadDir.Create();
            }

            _apiKey = pubApikey;
        }


        public void SynchronizeAsync()
        {

            using (HttpClient client = new HttpClient())
            {
                var headers = client.DefaultRequestHeaders;
                headers.Add("Api-Key", _apiKey);

                try
                {
                    HttpResponseMessage response = client.GetAsync(SEARCH_IRI).Result;
                    GettyResult result = JsonConvert.DeserializeObject<GettyResult>(response.Content.ToString());
                }
                catch (Exception e)
                {

                }
            }
        }

    }
}
