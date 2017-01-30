using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using EyesOnTheNet.Private;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EyesOnTheNet.Controllers
{
    public class FileRequests
    {
        private EyesOnTheNetRepository newRepo;
        private string userName { get; set; }
        private Camera userCamera { get; set; }
        public FileRequests(EyesOnTheNetRepository repo, string sentUserName)
        {
            userName = sentUserName;
            newRepo = repo;
        }

        public FileRequests(EyesOnTheNetRepository repo, string sentUserName, int sentCameraId)
        {
            userName = sentUserName;
            userCamera = newRepo.CanAccessThisCamera(sentUserName, sentCameraId);
        }


        // This is for saving a single camera snapshot as a file and in the DB
        public async void SaveCameraPhoto()
        {
            CameraRequests myCameraRequest = new CameraRequests();
            Picture singleCameraPicture = await myCameraRequest.GetSnapshot(userCamera);
            long currentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string newFileName = $"{userCamera.CameraId.ToString()}_{currentDateTime.ToString()}.jpg";
            string newSavePath = PrivateParameters.savedPhotoFilePath;
            File.WriteAllBytes($"{newSavePath}{newFileName}", singleCameraPicture.data);

            Photo currentPhoto = new Photo {
                FileLocation = newSavePath,
                Filename = newFileName,
                CreationDate = currentDateTime,
            };
            newRepo.AddFilesToDatabase(userName, userCamera.CameraId, currentPhoto);
        }

        public List<SimplePhoto> SendPhotoList()
        {
            return newRepo.GetUserPhotoList(userName);
        }

        public Picture GetDvrPhoto(int sentPhotoId)
        {
            string currentFilename = newRepo.ReturnFileName(userName, sentPhotoId);

            Picture dvrPhotoPic = new Picture {
                data = File.ReadAllBytes($"{PrivateParameters.savedPhotoFilePath}{currentFilename}"),
                encodeType = "image/jpeg"
            };
            return dvrPhotoPic;
        }

        public Photo DeleteSinglePhoto(int sentPhotoId)
        {
            Photo deletedPhoto = newRepo.RemoveCameraPhotoFromDatabase(userName, sentPhotoId);

            if (deletedPhoto != null)
            {
                File.Delete($"{PrivateParameters.savedPhotoFilePath}{deletedPhoto.Filename}");
            }
            return deletedPhoto;
        }

    }
}
