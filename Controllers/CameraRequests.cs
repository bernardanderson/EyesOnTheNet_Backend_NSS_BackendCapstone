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

            switch (sentCamera.Type)
            {
                case 0:
                    // Foscam Webcam
                    connectionString = $"{sentCamera.WebAddress}/snapshot.cgi?user={sentCamera.LoginName}&pwd={sentCamera.LoginPass}";
                    break;
                case 1:
                    // IPCam Cell Phone App
                    connectionString = $"{sentCamera.WebAddress}/shot.jpg";
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
                encodeType = "image/jpeg"
            };

            // The below string will write the received image stream the specified file/location  
            //File.WriteAllBytes("/home/banderso/NSS_Backend/eyesonthenet/images/image.jpg", pictureStream.data);

            return pictureStream;
        }
    }
}
