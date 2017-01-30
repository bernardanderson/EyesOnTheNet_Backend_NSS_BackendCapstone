using EyesOnTheNet.Models;
using System;
using System.Linq;

namespace EyesOnTheNet.DAL
{
    public class DbInitializer
    {
        //Initializer for the main database, builds a guest account and adds some sample cameras  
        public static void Initialize(EyesOnTheNetContext Context)
        {
            // Checks to see if the Db exists
            Context.Database.EnsureCreated();

            // Look for any Users, if so the Db has already been created
            if (Context.Users.Any())
            {
                return;   // DB has been seeded
            }

            // Adds the guest account
            var user = new User
            {
                Username = "guesteyes",
                Password = "guesteyes",
                Email = "guesteyes@gmail.com",
                LastLoginDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
            };
            Context.Users.Add(user);
            Context.SaveChanges();

            // Adds some sample cameras
            Camera camera1 = new Camera
            {
                Name = "Adventure Science Center",
                Type = 2,
                WebAddress = "http://wwc.instacam.com/instacamimg/NSHV1/NSHV1_l.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "800 Fort Negley Blvd, Nashville, TN 37203",
                CreatedBy = user
            };
            Context.Cameras.Add(camera1);

            Camera camera2 = new Camera
            {
                Name = "Vanderbilt Commons and Dean's Residence",
                Type = 2,
                WebAddress = "http://webcams.vanderbilt.edu/thecommons/deans_ctr/deans_ctr_evocam.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "2201 West End Ave, Nashville, TN 37235",
                CreatedBy = user
            };
            Context.Cameras.Add(camera2);

            Camera camera3 = new Camera
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
            Context.Cameras.Add(camera3);

            Camera camera4 = new Camera
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
            Context.Cameras.Add(camera4);

            Camera camera5 = new Camera
            {
                Name = "Red Door Resort & Motel",
                Type = 2,
                WebAddress = "http://www.mnlakecams.com/reddoor/reddoor.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "38421 MN-18, Aitkin, MN 56431",
                CreatedBy = user
            };
            Context.Cameras.Add(camera5);

            Camera camera6 = new Camera
            {
                Name = "Hawaii (Pu u O O Crater)",
                Type = 2,
                WebAddress = "https://hvo.wr.usgs.gov/cams/POcam/images/M.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "Hawaii",
                CreatedBy = user
            };
            Context.Cameras.Add(camera6);

            Camera camera7 = new Camera
            {
                Name = "Glen Canyon Colorado River Arizona",
                Type = 2,
                WebAddress = "https://www.nps.gov/webcams-glca/po1.jpg",
                LoginName = "",
                LoginPass = "",
                Private = 1,
                Location = "Glen Canyon Colorado River Arizona",
                CreatedBy = user
            };
            Context.Cameras.Add(camera7);

            Context.SaveChanges();
        }
    }
}
