using System.Collections.Generic;
using System.Threading.Tasks;
using EyesOnTheNet.Models;
using EyesOnTheNet.DAL;
using System.Threading;

namespace EyesOnTheNet.Controllers
{
    /// <summary>
    /// This is a collection of methods for the background recording of single cameras. These cameras will be recorded server-side
    ///  on a Background thread
    /// </summary>
    public class BackgroundTasks
    {
        private TaskSingleton myInstance = TaskSingleton.Instance;
        private EyesOnTheNetRepository newRepo;
        public BackgroundTasks(EyesOnTheNetRepository repo)
        {
            newRepo = repo;
        }
        //Gets the full camera data and intializes the recording
        public bool StartCameraRecording(RecordCamera sentRecordCamera)
        {
            sentRecordCamera.fullRecordingCamera = new Camera();
            sentRecordCamera.fullRecordingCamera = newRepo.CanAccessThisCamera(sentRecordCamera.userName, sentRecordCamera.recordingCameraId);

            if (sentRecordCamera.fullRecordingCamera == null)
            {
                return false;
            }
            InitializeBackgroundTask(sentRecordCamera);
            return true;
        }

        //Build the cancellationsource, stops the camera from being recorded if its already being recorded
        // adds the camera to the singleton cameratask list and starts the Background task
        private void InitializeBackgroundTask(RecordCamera sentRecordCamera)
        {
            sentRecordCamera.userCancellationTokenSrc = new CancellationTokenSource();
            StopTask(sentRecordCamera, false);
            myInstance.AddRecordCameraTask(sentRecordCamera);
            Task userTask = Task.Run(() => TimedCameraCapture(sentRecordCamera), sentRecordCamera.userCancellationTokenSrc.Token);
        }

        // Takes a single camera and records its photo in a background thread 
        private void TimedCameraCapture(RecordCamera sentUserTask)
        {
            while (!sentUserTask.userCancellationTokenSrc.Token.IsCancellationRequested)
            {
                new FileRequests(sentUserTask.userName, sentUserTask.fullRecordingCamera.CameraId).SaveCameraPhoto();
                Thread.Sleep(sentUserTask.recordDelay);
            }
        }

        // Stops a task that is running
        public bool StopTask(RecordCamera sentAlreadyRecordingCamera, bool needToFindCamera)
        {
            if (sentAlreadyRecordingCamera != null && needToFindCamera)
            {
                sentAlreadyRecordingCamera.fullRecordingCamera = new Camera();
                sentAlreadyRecordingCamera.fullRecordingCamera = newRepo.CanAccessThisCamera(sentAlreadyRecordingCamera.userName, sentAlreadyRecordingCamera.recordingCameraId);
            }

            RecordCamera foundRecordCamera = myInstance.CheckCameraRecordTask(sentAlreadyRecordingCamera);

            if (foundRecordCamera != null)
                {
                    myInstance.RemoveUserTask(foundRecordCamera);
                    foundRecordCamera.userCancellationTokenSrc.Cancel();
                    foundRecordCamera.userCancellationTokenSrc.Dispose();
                    return true;
                }
            return false;
        }
        
        //Gets the current list of user recorded cameras from the singleton
        public List<RecordCamera> ReturnTasks(string currentUser)
        {
            return myInstance.RetrieveUserTasks(currentUser);
        }
    }
}
