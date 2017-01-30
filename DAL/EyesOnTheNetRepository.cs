using EyesOnTheNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EyesOnTheNet.DAL
{
    public class EyesOnTheNetRepository
    {
        private EyesOnTheNetContext Context { get; set; }
        public EyesOnTheNetRepository(EyesOnTheNetContext _context)
        {
            Context = _context;
        }

        // Adds a User to the Db
        private void AddUser(User sentUser)
        {
            sentUser.LastLoginDate = DateTime.Now;
            sentUser.RegistrationDate = DateTime.Now;

            Context.Add(sentUser);
            Context.SaveChanges();
        }

        // Adds a Camera to the Db
        private void AddOrUpdateCamera(Camera sentCamera, User sentUser)
        {
            var originalCamera = Context.Cameras.Find(sentCamera.CameraId);

            if (originalCamera != null)
            {
                originalCamera.CameraId = sentCamera.CameraId;
                originalCamera.Name = sentCamera.Name;
                originalCamera.Type = sentCamera.Type;
                originalCamera.WebAddress = sentCamera.WebAddress;
                originalCamera.LoginName = sentCamera.LoginName;
                originalCamera.LoginPass = sentCamera.LoginPass;
                originalCamera.Private = sentCamera.Private;
                originalCamera.Location = sentCamera.Location;
                originalCamera.CreatedBy = sentUser;
            } else
            {
                Camera newCamera = new Camera
                {
                    Name = sentCamera.Name,
                    Type = sentCamera.Type,
                    WebAddress = sentCamera.WebAddress,
                    LoginName = sentCamera.LoginName,
                    LoginPass = sentCamera.LoginPass,
                    Private = sentCamera.Private,
                    Location = sentCamera.Location,
                    CreatedBy = sentUser
                };
                Context.Add(newCamera);
            }
                Context.SaveChanges();
        }

        
        // Checks just the UserName, for initial registration
        public bool CheckUserLogin(string sentUserName)
        {
            User foundUser = ReturnUser(sentUserName);

            if (foundUser != null)
            {
                return true; // The user is found, therefore already exists.
            }
            return false; // The user is not found and can be added.
        }

        // Checks both the UserName and Password, for login
        public bool CheckUserLogin(string sentUserName, string sentPassword)
        {
            User foundUser = ReturnUser(sentUserName);

            if (foundUser == null)
            {
                return false;
            } else if (foundUser.Password != sentPassword)
            {
                return false;
            }
            return true;
        }

        // For returing an error during the registration process
        public KeyValuePair<bool, string> RegisterUser(User sentUser)
        {
            if (sentUser.Username == "" || sentUser.Password == "")
            {
                return new KeyValuePair<bool, string> (false, "Blank User Name or Password");
            } else if (this.CheckUserLogin(sentUser.Username)) {
                return new KeyValuePair<bool, string>(false, "User Already Exists!");
            }
            this.AddUser(sentUser);
            return new KeyValuePair<bool, string>(true, "Successfully Registered");
        }

        //Returns the List of Cameras Associated with that User
        public List<SimpleCameraUserAccess> ReturnUserCameras(string sentUser)
        {
            List<Camera> foundCameras = Context.Cameras.Where(x => x.CreatedBy.Username == sentUser).ToList();
            List<SimpleCameraUserAccess> hashedUserCameras = new List<SimpleCameraUserAccess>();

            if (foundCameras == null)
            {
                return hashedUserCameras;
            }

            for (int i = 0; i < foundCameras.Count; i++)
            {
                SimpleCameraUserAccess currentCamera = MakeSimpleUserCameraAccess(foundCameras[i]);
                hashedUserCameras.Add(currentCamera);
            }
            return hashedUserCameras;
        }

        // Builds a single simple camera to send to the user 
        public SimpleCameraUserAccess MakeSimpleUserCameraAccess(Camera sentCamera)
        {
            SimpleCameraUserAccess currentSimpleCamera = new SimpleCameraUserAccess
            {
                CameraIdHash = sentCamera.CameraId.ToString(),
                Name = sentCamera.Name,
                Location = sentCamera.Location
            };
            return currentSimpleCamera;
        }

        // Is the user authorized to see these cameras?
        public Camera CanAccessThisCamera(string sentUser, int sentCameraId)
        {
            Camera foundCamera = new Camera();
            foundCamera = Context.Cameras
                .Where(x => x.CreatedBy.Username == sentUser)
                .FirstOrDefault(x => x.CameraId == sentCameraId);

            return foundCamera;
        }

        // Find and return a single camera from the Db
        public Camera FindAndReturnASingleCamera(string sentUser, string cameraName)
        {
            Camera foundCamera = new Camera();
            foundCamera = Context.Cameras
                .Where(x => x.CreatedBy.Username == sentUser)
                .ToList()
                .FirstOrDefault(x => x.Name == cameraName);

            return foundCamera;
        }

        public Camera FindAndReturnASingleCamera(string sentUser, int cameraId)
        {
            Camera foundCamera = new Camera();
            foundCamera = Context.Cameras
                .Where(x => x.CreatedBy.Username == sentUser)
                .ToList()
                .FirstOrDefault(x => x.CameraId == cameraId);

            return foundCamera;
        }

        public User ReturnUser (string sentUserName)
        {
            return Context.Users.FirstOrDefault(u => u.Username == sentUserName);
        }

        public SimpleCameraUserAccess AddCameraToDatabaseProcess(Camera sentCamera, string sentUserName)
        {
            User currentUser = ReturnUser(sentUserName);
            Camera newCameraEntry = FindAndReturnASingleCamera(sentUserName, sentCamera.CameraId);

            AddOrUpdateCamera(sentCamera, currentUser);
            newCameraEntry = FindAndReturnASingleCamera(sentUserName, sentCamera.Name);

            return MakeSimpleUserCameraAccess(newCameraEntry);
        }

        public Camera RemoveCameraFromDatabase(string sentUserName, int sentCameraId)
        {
            User currentUser = ReturnUser(sentUserName);
            Camera cameraToDelete = CanAccessThisCamera(sentUserName, sentCameraId);

            if (cameraToDelete != null)
            {
                Context.Cameras.Remove(cameraToDelete);
                Context.SaveChanges();
            }

            return cameraToDelete;
        }

        public Photo AddFilesToDatabase(string sentUserName, int sentCameraId, Photo sentPhotoToAddToDatabase) 
        {
            // Trying to build the complete Photo object for entry into the DB using entity confuses entity into thinking 
            //  that the Camera and User are new things and trys to add them as such.  You need to "show" entity that the
            //  Camera and User already exist (by finding it in the DB) and adding it the the photo object just before 
            //  adding it to the DB.  See Holland Risley's answer below.
            //  http://stackoverflow.com/questions/15394207/entityframework-duplicating-when-calling-savechanges
            sentPhotoToAddToDatabase.User = ReturnUser(sentUserName);
            sentPhotoToAddToDatabase.Camera = Context.Cameras.FirstOrDefault(c => c.CameraId == sentCameraId);

            Context.Add(sentPhotoToAddToDatabase);
            Context.SaveChanges();

            return sentPhotoToAddToDatabase;
        }

        // Builds the Object of Cameras and their photos with capture times
        public List<SimplePhoto> GetUserPhotoList(string userName)
        {
            // Gets the List of Photos and groups them by CameraId
            List<IGrouping<int, Photo>> tempGrouping = Context.Photos.Where(u => u.User.Username == userName).GroupBy(m => m.CameraId).ToList();
            // Get the complete list of Cameras (to get the Camera Names)
            IQueryable<Camera> usersCameraList = Context.Cameras.Where(c => c.CreatedBy.Username == userName);
            List<SimplePhoto> userSimplePhotoList = new List<SimplePhoto>();

            for (int i = 0; i < tempGrouping.Count(); i++)
            {
                SimplePhoto tempSimplePhoto = new SimplePhoto
                {
                    CameraId = tempGrouping[i].Key,
                    CameraName = usersCameraList.FirstOrDefault(c => c.CameraId == tempGrouping[i].Key).Name,
                };

                var tempList = new List<KeyValuePair<int, long>>();

                foreach (var singlePhoto in tempGrouping[i])
                {
                    KeyValuePair<int, long> tempKeyValue = new KeyValuePair<int, long>(singlePhoto.PhotoId, singlePhoto.CreationDate);
                    tempList.Add(tempKeyValue);
                }

                tempSimplePhoto.PhotoIdTime = tempList;
                userSimplePhotoList.Add(tempSimplePhoto);
            }
            return userSimplePhotoList;
        }

        // Returns a single photo from the Db
        public string ReturnFileName(string sentUserName, int sentPhotoId)
        {
            // Check to see if the user has access to a picture.  Also catches for PhotoIds that don't exist.
            Photo singlePhoto = Context.Photos.Where(p => p.PhotoId == sentPhotoId).FirstOrDefault(u => u.User.Username == sentUserName);

            if (singlePhoto != null)
            {
                return singlePhoto.Filename;
            }
            return "faulty.jpg";
        }

        public Photo RemoveCameraPhotoFromDatabase(string sentUserName, int sentPhotoId)
        {
            Photo singlePhotoToDelete = Context.Photos.Where(p => p.PhotoId == sentPhotoId).FirstOrDefault(u => u.User.Username == sentUserName);

            if (singlePhotoToDelete != null)
            {
                Context.Photos.Remove(singlePhotoToDelete);
                Context.SaveChanges();
            }

            return singlePhotoToDelete;
        }



        
    }
}
