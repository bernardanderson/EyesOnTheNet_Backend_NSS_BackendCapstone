using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using EyesOnTheNet.Models;
using EyesOnTheNet.DAL;
using System.Text;
using System.Net.Http.Headers;

namespace EyesOnTheNet.Controllers
{
    public class CameraRequests
    {
        // Using a static method for HttpClient reduces the build up of 'waiting' threads which can severely hinder performance
        //  http://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient Client = new HttpClient();

        public async Task<string> GetParameters()
        {
            HttpResponseMessage response = await Client.GetAsync("http://192.168.0.223/get_status.cgi");
            var combinedResponse = await response.Content.ReadAsStringAsync();

            return combinedResponse;
        }
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

            // For unique file saving
            long currentDateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            string newSaveString = $"{sentCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            // The below string will write the received image stream the specified file/location  
            // File.WriteAllBytes($"/home/banderso/NSS_Backend/eyesonthenet/images/{newSaveString}", pictureStream.data);

            return pictureStream;
        }

        // For the Basic Auth header encoding: https://gist.github.com/bryanbarnard/8102915
        private void BasicAuthentication(string camUsername, string camPassword) 
        {
            Byte[] byteArray = Encoding.ASCII.GetBytes($"{camUsername}:{camPassword}");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}
