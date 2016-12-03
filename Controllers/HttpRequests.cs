using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EyesOnTheNet.Controllers
{
    public class HttpCameraAccess
    {
        // Using a static method for HttpClient reduces the build up of 'waiting' threads which can severely hinder performance
        //  http://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient Client = new HttpClient();

        public async Task<Stream> GetParameters()
        {
            Stream response = await Client.GetStreamAsync("http://192.168.0.223/get_status.cgi");

            return response;
        }

        public async Task<Stream> GetSnapshot()
        {
            Stream response = await Client.GetStreamAsync("http://192.168.0.223/snapshot.cgi?user=mover&pwd=");

            return response;
        }
    }
}
