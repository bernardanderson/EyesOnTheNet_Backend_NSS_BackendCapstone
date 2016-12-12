using EyesOnTheNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EyesOnTheNet.DAL
{
    public class EyesOnTheNetRepository
    {
        public EyesOnTheNetContext Context { get; set; }

        public EyesOnTheNetRepository()
        {
            Context = new EyesOnTheNetContext();

            // The following checks to see if the database exists.  If not, this makes it and returns 'true'.
            //  If the Db exists, this does nothing and returns false. I'm not using regular migrations since I'm
            //  attaching to a stand-alone MySQL server. 
            Context.Database.EnsureCreated();
        }

        private void AddUser(User sentUser)
        {
            sentUser.LastLoginDate = DateTime.Now;
            sentUser.RegistrationDate = DateTime.Now;

            Context.Add(sentUser);
            Context.SaveChanges();
        }

        public void AddFakeEverything()
        {
            var user = new User
            {
                Username = "qweqwe",
                Password = "qweqwe",
                Email = "qweqwe@gmail.com",
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
            };
            Context.Add(user);

            var user1 = new User
            {
                Username = "wewe",
                Password = "wewe",
                Email = "wewe@gmail.com",
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
            };

            Context.SaveChanges();

            var fakeCamera = new Camera
            {
                Name = "Adventure Science Center",
                Type = 2,
                WebAddress = "http://wwc.instacam.com/instacamimg/NSHV1/NSHV1_l.jpg",
                LoginName= "",
                LoginPass= "",
                Private = 1,
                Location = "800 Fort Negley Blvd, Nashville, TN 37203",
                CreatedBy = user
            };
            Context.Add(fakeCamera);

            var fakeCamera1 = new Camera
            {
                Name = "Vanderbilt Commons and Dean's Residence",
                Type = 2,
                WebAddress = "http://webcams.vanderbilt.edu/thecommons/deans_ctr/deans_ctr_evocam.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "2201 West End Ave, Nashville, TN 37235",
                CreatedBy = user1
            };
            Context.Add(fakeCamera1);
            
            var fakeCamera2 = new Camera
            {
                Name = "Wicker Guesthouse",
                Type = 2,
                WebAddress = "http://www.floridakeyswebcams.tv/axiscam/wickerguesthouse/wickerguesthouse.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "913 Duval Street, Key West, FL 33040",
                CreatedBy = user
            };
            Context.Add(fakeCamera2);

            var fakeCamera3 = new Camera
            {
                Name = "Southern-most Point in the USA",
                Type = 2,
                WebAddress = "http://www.floridakeyswebcams.tv/axiscam/southernmostpoint/southernmostpoint.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "Whitehead St & South St, Key West, FL 33040",
                CreatedBy = user
            };
            Context.Add(fakeCamera3);
            Context.SaveChanges();
        }
        // Checks just the UserName
        public bool CheckUserLogin(string sentUserName)
        {
            var foundUser = Context.Users.FirstOrDefault(u => u.Username == sentUserName);

            if (foundUser != null)
            {
                return true; // The user is found, therefore already exists.
            }
            return false; // The user is not found and can be added.
        }

        // Checks both the UserName and Password
        public bool CheckUserLogin(string sentUserName, string sentPassword)
        {
            var foundUser = Context.Users.FirstOrDefault(u => u.Username == sentUserName);
            if (foundUser == null)
            {
                return false;
            } else if (foundUser.Password != sentPassword)
            {
                return false;
            }
            return true;
        }
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

        public List<SimpleCameraUserAccess> ReturnUserCameras(string sentUser)
        {
            //Returns the List of Cameras Associated with that User
            List<Camera> foundCameras = Context.Cameras.Where(x => x.CreatedBy.Username == sentUser).ToList();
            List<SimpleCameraUserAccess> hashedUserCameras = new List<SimpleCameraUserAccess>();

            if (foundCameras == null)
            {
                return hashedUserCameras;
            }

            for (int i = 0; i < foundCameras.Count; i++)
            {
                SimpleCameraUserAccess currentCamera = new SimpleCameraUserAccess
                {
                    CameraIdHash = foundCameras[i].CameraId.ToString(),
                    Name = foundCameras[i].Name,
                    Location = foundCameras[i].Location
                };
                hashedUserCameras.Add(currentCamera);
            }
            return hashedUserCameras;
        }

        public Camera CanAccessThisCamera(string sentUser, int sentCameraId)
        {
            Camera foundCamera = new Camera();
            foundCamera = Context.Cameras
                .Where(x => x.CreatedBy.Username == sentUser)
                .ToList()
                .FirstOrDefault(x => x.CameraId == sentCameraId);

            return foundCamera;
        }
    }
}
