using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EyesOnTheNet.Controllers
{
    public class FileRequests
    {
        public FileRequests(string sentUserName)
        {
            userName = sentUserName;
        }

        public FileRequests(string sentUserName, int sentCameraId)
        {
            userName = sentUserName;
            userCamera = new EyesOnTheNetRepository().CanAccessThisCamera(sentUserName, sentCameraId);
        }

        private string userName { get; set; }
        private Camera userCamera { get; set; }

        // The below commented code is for repeatedly saving a camera snapshot as a file using the BackEnd as the timer 
        /*
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
        */

        // This is for saving a single camera snapshot as a file and in the DB
        public async void SaveCameraPhoto()
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(userCamera);
            EyesOnTheNetRepository newEOTN = new EyesOnTheNetRepository();
            long currentDateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            string newFileName = $"{userCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            string newSavePath = $"/home/banderso/NSS_Backend/eyesonthenet/images/";
            File.WriteAllBytes($"{newSavePath}{newFileName}", singleCameraPicture.data);

            Photo currentPhoto = new Photo {
                FileLocation = newSavePath,
                Filename = newFileName,
                CreationDate = currentDateTime,
            };

            newEOTN.AddFilesToDatabase(userName, userCamera.CameraId, currentPhoto);
        }

        public List<SimplePhoto> SendPhotoList()
        {
            EyesOnTheNetRepository newEOTN = new EyesOnTheNetRepository();
            return newEOTN.GetUserPhotoList(userName);
        }
    }
}
