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

        public void AddFakeUser()
        {
            var user = new User
            {
                Username = "qweqwe",
                Password = "qweqwe",
                Email = "testcamuser@gmail.com",
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
            };

            Context.Add(user);
            Context.SaveChanges();
        }

        public void AddFakeCamera()
        {
            var fakeCamera = new Camera
            {
                Name = "BigCam",
                Type = 2
            };

            Context.Add(fakeCamera);
            Context.SaveChanges();
        }

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
        public bool RegisterUser(string sentUserName, string sentPassword)
        {
            return true;
        }



    }
}
