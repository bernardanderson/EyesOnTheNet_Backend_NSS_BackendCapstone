using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using System;
using System.IO;
using System.Threading;

namespace EyesOnTheNet.Controllers
{
    public class FileRequests
    {
        public FileRequests(string sentUserName, int sentCameraId)
        {
            userName = sentUserName;
            userCamera = new EyesOnTheNetRepository().CanAccessThisCamera(sentUserName, sentCameraId);
        }

        private string userName { get; set; }
        private Camera userCamera { get; set; }

        public void StartTimer(int timerInterval)
        {
            TimerCallback newCallback = new TimerCallback(SaveCameraPhoto);
            Timer newTimer = new Timer(newCallback, null, 0, timerInterval);

        }
        private async void SaveCameraPhoto(object obj)
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(userCamera);

            long currentDateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            string newSaveString = $"{userCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            File.WriteAllBytes($"/home/banderso/NSS_Backend/eyesonthenet/images/{newSaveString}", singleCameraPicture.data);
        }

        public async void SaveCameraPhoto()
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(userCamera);

            long currentDateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            string newSaveString = $"{userCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            File.WriteAllBytes($"/home/banderso/NSS_Backend/eyesonthenet/images/{newSaveString}", singleCameraPicture.data);
        }


    }
}
