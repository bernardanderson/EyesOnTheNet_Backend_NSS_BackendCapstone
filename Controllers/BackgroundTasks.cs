using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyesOnTheNet.Models;
using EyesOnTheNet.DAL;
using System.Threading;

namespace EyesOnTheNet.Controllers
{
    public class BackgroundTasks
    {
        public bool StartCameraRecording(RecordCameras sentRecordCameras)
        {
            // Did the user submit no cameras?
            if (sentRecordCameras.recordingCameras.Count == 0) { return false; }

            sentRecordCameras = BuildValidCameraList(sentRecordCameras);

            // Did the user submit no accessable cameras?
            if (sentRecordCameras.fullRecordingCameras.Count == 0 ) { return false; }

            InitializeBackgroundTask(sentRecordCameras);
            return true;
        }
        private RecordCameras BuildValidCameraList(RecordCameras sentRecordCameras)
        {
            EyesOnTheNetRepository tempEOTNR = new EyesOnTheNetRepository();

            // Cycles through the cameras the user is trying to record and if they have access it adds it to the 
            //  valid list to be recorded
            for (var i = 0; i < sentRecordCameras.recordingCameras.Count; i++)
            {
                Camera returnedCamera = tempEOTNR.CanAccessThisCamera(sentRecordCameras.userName, sentRecordCameras.recordingCameras[i]);
                if (returnedCamera != null)
                {
                    sentRecordCameras.fullRecordingCameras.Add(returnedCamera);
                }
            }
            return sentRecordCameras;
        }

        private void InitializeBackgroundTask(RecordCameras sentRecordCameras)
        {
            TaskSingleton myInstance = TaskSingleton.Instance;
            CancellationTokenSource currentCancellationTokenSource = new CancellationTokenSource();

            UserTask currentUserTask = new UserTask()
            {
                userName = sentRecordCameras.userName,
                userCancellationTokenSrc = currentCancellationTokenSource
            };

            // Adds the Task info to the Singleton for later retrieval, and starts the recording Task
            myInstance.AddUserTask(currentUserTask);
            Task userTask = Task.Run(() => MultipleCameraCaptures(currentUserTask, sentRecordCameras), currentCancellationTokenSource.Token);
        }

        private void MultipleCameraCaptures(UserTask sentUserTask, RecordCameras sentRecordCameras)
        {
            while (!sentUserTask.userCancellationTokenSrc.Token.IsCancellationRequested)
            {
                for (var i = 0; i < sentRecordCameras.fullRecordingCameras.Count; i++)
                {
                    new FileRequests(sentRecordCameras.userName, sentRecordCameras.fullRecordingCameras[i].CameraId).SaveCameraPhoto();
                }

                Thread.Sleep(sentRecordCameras.recordDelay);
            }
        }
    }
}
