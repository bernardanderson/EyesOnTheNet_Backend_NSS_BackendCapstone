﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace EyesOnTheNet.Controllers
{
    public class HttpRequests
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
        public async Task<byte[]> GetSnapshot()
        {
            HttpResponseMessage response = await Client.GetAsync("http://192.168.0.223/snapshot.cgi?user=mover&pwd="); // Fosacam Camera
            //HttpResponseMessage response = await Client.GetAsync("http://192.168.0.202:8080/shot.jpg"); // For IPCam Cell Camera

            var combinedResponse = await response.Content.ReadAsByteArrayAsync();

            // The below string will write the received image stream the specified file/location  
            // File.WriteAllBytes("/home/banderso/NSS_Backend/eyesonthenet/images/image.jpg", stringedResponse);

            return combinedResponse;
        }
    }
}
