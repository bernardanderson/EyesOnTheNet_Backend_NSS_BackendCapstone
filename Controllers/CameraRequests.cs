using System;
using System.Threading.Tasks;
using System.Net.Http;
using EyesOnTheNet.Models;
using System.Text;
using System.Net.Http.Headers;
using EyesOnTheNet.Private;

namespace EyesOnTheNet.Controllers
{
    public class CameraRequests
    {
        // Using a static method for HttpClient reduces the build up of 'waiting' threads which can severely hinder performance
        //  http://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient Client = new HttpClient()
        {
            Timeout = new TimeSpan(0, 0, 15) // Timesout after 15 seconds
        };

        // Gets a single image from a selected camera source
        public async Task<Picture> GetSnapshot(Camera sentCamera)
        {
            string connectionString = "";

            if (sentCamera.LoginName == null)
            {
                sentCamera.LoginName = "";
            } else if (sentCamera.LoginPass == null)
            {
                sentCamera.LoginPass = "";
            }

            switch (sentCamera.Type)
            {
                case 0:
                    // Foscam Webcam
                    connectionString = $"{sentCamera.WebAddress}/snapshot.cgi?resolution=32";
                    BasicAuthentication(sentCamera.LoginName, sentCamera.LoginPass);
                    break;
                case 1:
                    // IPCam Cell Phone App
                    connectionString = $"{sentCamera.WebAddress}/shot.jpg";
                    BasicAuthentication(sentCamera.LoginName, sentCamera.LoginPass);
                    break;
                case 2:
                    // Public Webcam
                    connectionString = $"{sentCamera.WebAddress}";
                    break;
                default:
                    connectionString = "http://www.clipartbest.com/cliparts/yio/eXG/yioeXG4RT.jpeg";
                    break;
            }

            HttpResponseMessage response = await Client.GetAsync(connectionString);

            Picture pictureStream = new Picture
            {
                data = await response.Content.ReadAsByteArrayAsync(),
                encodeType = "image/jpeg",
            };

            return pictureStream;
        }

        // Gets a static Google Map using a protected API key
        public async Task<Picture> GetGoogleMap(Camera sentCamera, int sentZoomLevel) {

            string tempLocationString = sentCamera.Location.Replace(" ", "+");
            string connectionString = $"https://maps.googleapis.com/maps/api/staticmap?zoom={sentZoomLevel}&size=300x500&markers=color:red%7C${tempLocationString}&key={PrivateParameters.GoogleStaticMap}";

            // May Need Cert for HTTPS
            HttpResponseMessage response = await Client.GetAsync(connectionString);

            Picture pictureStream = new Picture
            {
                data = await response.Content.ReadAsByteArrayAsync(),
                encodeType = "image/png",
            };
            return pictureStream;
        }

        // For the Basic Auth header encoding for cameras that require it: https://gist.github.com/bryanbarnard/8102915
        private void BasicAuthentication(string camUsername, string camPassword) 
        {
            Byte[] byteArray = Encoding.ASCII.GetBytes($"{camUsername}:{camPassword}");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}
