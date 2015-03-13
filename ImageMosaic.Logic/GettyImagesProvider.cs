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
                new DirectoryInfo(
                    Path.Combine(
                        Path.GetTempPath(), "ImageMosaic")),
                pubApiKey)
        {
        }

        public GettyImagesProvider(DirectoryInfo downloadDir, string pubApikey)
        {
            if (!downloadDir.Exists)
            {
                downloadDir.Create();
            }

            DownloadDir = downloadDir;
            _apiKey = pubApikey;
        }

        public FileInfo RndLocalPicturePick()
        {
            FileInfo[] localPictures = DownloadDir.GetFiles("*.jpg");
            Random rnd = new Random();

            if (localPictures.Length == 0)
            {
                throw new FileNotFoundException("No matching file found in download directory");
            }

            int position = rnd.Next(0, localPictures.Length - 1);
            return localPictures[position];
        }

        public async void SynchronizeAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                SetAuthenticationHeaders(client);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(SEARCH_IRI);

                    if (response.IsSuccessStatusCode)
                    {
                        String content = await response.Content.ReadAsStringAsync();
                        GettyResult result = await JsonConvert.DeserializeObjectAsync<GettyResult>(content);

                        await Task.Run(() => DownloadMissingImages(result.images));
                    }
                    // Do not care of request failure, it's in the specs :D
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void DownloadMissingImages(List<SimpleGettyImage> images)
        {
            Parallel.ForEach(images, currentImage =>
            {
                if (HaveToSyncFile(currentImage.FileName))
                {
                    DownloadImage(currentImage);
                }
            });
        }

        private bool HaveToSyncFile(string fileName)
        {
            return DownloadDir.GetFiles(fileName).Length == 0; 
        }

        private async void DownloadImage(SimpleGettyImage image)
        {

            using (HttpClient client = new HttpClient())
            using (FileStream fileStream = File.Create(
                                                    Path.Combine(
                                                            DownloadDir.FullName, image.FileName)
                                                            ))
            {
                SetAuthenticationHeaders(client);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(
                                                                    image.FindBestDisplayURI());

                    if (response.IsSuccessStatusCode)
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        await stream.CopyToAsync(fileStream);
                    }
                    // Do not care of request failure, it's in the specs :D
                }
                catch (Exception e) { throw e; }
            }
        } 

        private void SetAuthenticationHeaders(HttpClient client)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Add("Api-Key", _apiKey);
        }
    }
}
