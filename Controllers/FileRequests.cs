using EyesOnTheNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Controllers
{
    public class FileRequests
    {
        public async Task<string> SaveCameraPhoto(Camera sentCamera)
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(sentCamera);

            long currentDateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            string newSaveString = $"{sentCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            File.WriteAllBytes($"/home/banderso/NSS_Backend/eyesonthenet/images/{newSaveString}", singleCameraPicture.data);

            return newSaveString;
        }
    }
}
