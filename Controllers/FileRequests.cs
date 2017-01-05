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
        private string savedPhotoFilePath = "/eotn-images/"; 

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

        // This is for saving a single camera snapshot as a file and in the DB
        public async void SaveCameraPhoto()
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(userCamera);
            EyesOnTheNetRepository newEOTN = new EyesOnTheNetRepository();
            long currentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string newFileName = $"{userCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            string newSavePath = savedPhotoFilePath;
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
            return new EyesOnTheNetRepository().GetUserPhotoList(userName);
        }

        public Picture GetDvrPhoto(int sentPhotoId)
        {
            string currentFilename = new EyesOnTheNetRepository().ReturnFileName(userName, sentPhotoId);

            Picture dvrPhotoPic = new Picture {
                data = File.ReadAllBytes($"{savedPhotoFilePath}{currentFilename}"),
                encodeType = "image/jpeg"
            };

            return dvrPhotoPic;
        }

        public Photo DeleteSinglePhoto(int sentPhotoId)
        {
            Photo deletedPhoto = new EyesOnTheNetRepository().RemoveCameraPhotoFromDatabase(userName, sentPhotoId);

            if (deletedPhoto != null)
            {
                File.Delete($"{savedPhotoFilePath}{deletedPhoto.Filename}");
            }

            return deletedPhoto;
        }

    }
}
