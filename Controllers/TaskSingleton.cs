using EyesOnTheNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// The singleton pattern allows the creatation of a single instance of class and then the passing of that one instance to anything 
//  else that looks to "instantiate" it. Singletons allow info to be stored in-memory and accessed by subsequent actions without loss
//  of that data. Singltons are great for WebAPI calls where you might look to start something on the server-side but allow the user 
//  access to that thread after the API request completes.  
namespace EyesOnTheNet.Controllers
{
    public class TaskSingleton
    {
        // Holds Tasks (userName and Cancellation Token) created when a user chooses to start a background Thread
        //  to record video streams 
        private List<UserTask> _userCameraTasks { get; set; } = new List<UserTask>();

        private TaskSingleton()
        {
        }

        public static TaskSingleton Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly TaskSingleton instance = new TaskSingleton();
        }

        public void AddUserTask(UserTask sentUserTask)
        {
            _userCameraTasks.Add(sentUserTask);
        }

        public UserTask StopUserTask(string sentUserName)
        {
            UserTask foundUserTask = _userCameraTasks.FirstOrDefault(ut => ut.userName == sentUserName);

            if (foundUserTask != null)
            {
                _userCameraTasks.Remove(foundUserTask);
            }

            return foundUserTask;
        }

        public int RetrieveUserTask()
        {
            return _userCameraTasks.Count;
        }
    }
}
