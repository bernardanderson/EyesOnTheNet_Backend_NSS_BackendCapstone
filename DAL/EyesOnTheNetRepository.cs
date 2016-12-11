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

        public void AddFakeUser()
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
            Context.SaveChanges();
        }

        public void AddFakeCameras(int sentUserId)
        {
            var fakeCamera = new Camera
            {
                Name = "Adventure Science Center",
                Type = 2,
                WebAddress = "https://instacam.weatherbug.com/instacamimg/NSHV1/12102016/121020161546_l.jpg",
                LoginName= "",
                LoginPass= "",
                Private = false,
                UserId = sentUserId
            };
            Context.Add(fakeCamera);

            var fakeCamera1 = new Camera
            {
                Name = "Vanderbilt Dean Cam",
                Type = 2,
                WebAddress = "http://webcams.vanderbilt.edu/thecommons/deans_ctr/deans_ctr_evocam.jpg",
                LoginName = "",
                LoginPass = "",
                Private = false,
                UserId = sentUserId
            };
            Context.Add(fakeCamera1);

            var fakeCamera2 = new Camera
            {
                Name = "Vanderbilt New Olin Building",
                Type = 2,
                WebAddress = "http://webcams.vanderbilt.edu/newolin/newolin_evocam.jpg",
                LoginName = "",
                LoginPass = "",
                Private = false,
                UserId = sentUserId
            };
            Context.Add(fakeCamera2);

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

        public List<Camera> ReturnUserCameras(string sentUser)
        {
            // Finds the user and sets it's ID
            int userId = Context.Users.FirstOrDefault(x => x.Username == sentUser).UserId;
            
            //Returns the List of Cameras Associated with that User
            return Context.Cameras.Where(x => x.UserId == userId).ToList();
        }
    }
}
