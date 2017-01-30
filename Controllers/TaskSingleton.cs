using EyesOnTheNet.Models;
using System.Collections.Generic;
using System.Linq;

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
        private List<RecordCamera> _userCameraTasks { get; set; } = new List<RecordCamera>();

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

        //Adds a camera to the singleton list for later retrieval
        public void AddRecordCameraTask(RecordCamera sentRecordCamera)
        {
            _userCameraTasks.Add(sentRecordCamera);
        }

        //Checks to see if a user is already recording a specific camera
        public RecordCamera CheckCameraRecordTask(RecordCamera sentRecordCamera)
        {
            RecordCamera foundRecordingCameraTask = _userCameraTasks.Where(ut => ut.userName == sentRecordCamera.userName).FirstOrDefault(ut => ut.recordingCameraId == sentRecordCamera.recordingCameraId);
            return foundRecordingCameraTask;
        }

        //Removes a camera from the singleton list as it is being removed
        public void RemoveUserTask(RecordCamera sentRecordCamera)
        {
            _userCameraTasks.Remove(sentRecordCamera);
        }

        //Retrieves a list of cameras being recorded by the user
        public List<RecordCamera> RetrieveUserTasks(string sentUserName)
        {
            return _userCameraTasks.Where(ut => ut.userName == sentUserName).ToList();
        }
    }
}
