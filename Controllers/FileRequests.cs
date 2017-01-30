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
        public FileRequests(EyesOnTheNetRepository repo)
        {
            newRepo = repo;
        }

        // This is for saving a single camera snapshot as a file and in the DB
        public async void SaveCameraPhoto(string sentUserName, int sentCameraId)
        {
            Camera userCamera = newRepo.CanAccessThisCamera(sentUserName, sentCameraId);
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
            newRepo.AddFilesToDatabase(sentUserName, userCamera.CameraId, currentPhoto);
        }

        public List<SimplePhoto> SendPhotoList(string sentUserName)
        {
            return newRepo.GetUserPhotoList(sentUserName);
        }

        public Picture GetDvrPhoto(string sentUserName, int sentPhotoId)
        {
            string currentFilename = newRepo.ReturnFileName(sentUserName, sentPhotoId);

            Picture dvrPhotoPic = new Picture {
                data = File.ReadAllBytes($"{PrivateParameters.savedPhotoFilePath}{currentFilename}"),
                encodeType = "image/jpeg"
            };
            return dvrPhotoPic;
        }

        public Photo DeleteSinglePhoto(string sentUserName, int sentPhotoId)
        {
            Photo deletedPhoto = newRepo.RemoveCameraPhotoFromDatabase(sentUserName, sentPhotoId);

            if (deletedPhoto != null)
            {
                File.Delete($"{PrivateParameters.savedPhotoFilePath}{deletedPhoto.Filename}");
            }
            return deletedPhoto;
        }

    }
}
