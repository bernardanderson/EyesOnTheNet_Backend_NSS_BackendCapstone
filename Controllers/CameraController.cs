using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using System.IdentityModel.Tokens.Jwt;

namespace EyesOnTheNet.Controllers
{
    // Returns list of Users Cameras
    [Authorize]
    public class CameraController : Controller
    {
        [HttpGet("api/[controller]")]
        public IEnumerable<SimpleCameraUserAccess> GetListOfUserCameras()
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            return new EyesOnTheNetRepository().ReturnUserCameras(currentUser);
        }

        // Retrives the full Camera Data for a single camera so it can be edited
        [HttpGet("api/[controller]/{cameraId:int}/singlecamera")]
        public IActionResult GetFullSingleCamerasData(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Camera returnedCamera = new EyesOnTheNetRepository().CanAccessThisCamera(currentUser, cameraId);

            if (returnedCamera != null)
            {
                return Ok(returnedCamera);
            } else
            {
                return StatusCode(417, "Camera Data Not Accessible");
            }
        }

        // GET: api/camera/5/snapshot
        // Pulls an image from a single camera (based on it's table ID) for display
        [HttpGet("api/[controller]/{cameraId:int}/snapshot")]
        public async Task<ActionResult> GetSingleCameraImage(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Camera returnedUserCamera = new EyesOnTheNetRepository().CanAccessThisCamera(currentUser, cameraId);

            if (returnedUserCamera == null)
            {
                returnedUserCamera = new Camera { Type = -1 }; 
            }

            Picture cameraPicture = await new CameraRequests().GetSnapshot(returnedUserCamera);
            return File(cameraPicture.data, cameraPicture.encodeType);
        }

        // GET: api/camera/1/7/googlemap
        // Pulls the Google Map for a single camera and it's location
        [HttpGet("api/[controller]/{cameraId:int}/{zoomLevel:int}/googlemap")]
        public async Task<ActionResult> GetGoogleMapsStaticImage(int cameraId, int zoomLevel)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Camera returnedUserCamera = new EyesOnTheNetRepository().CanAccessThisCamera(currentUser, cameraId);

            if (returnedUserCamera != null)
            {
                Picture cameraPicture = await new CameraRequests().GetGoogleMap(returnedUserCamera, zoomLevel);
                return File(cameraPicture.data, cameraPicture.encodeType);
            }
            else
            {
                // Broken Picture, if user can't access Google Map
                Picture cameraPicture = await new CameraRequests().GetSnapshot(new Camera { Type = -1 });
                return File(cameraPicture.data, cameraPicture.encodeType);
            }
        }

        // Get: api/camera/stoprecording
        // Sends a single camera to the database to stop recording
        [HttpPost("api/[controller]/stoprecording")]
        public IActionResult StopRecordingCameraPost([FromBody]RecordCamera sentCameraToStop)
        {
            sentCameraToStop.userName = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            bool cameraStoppedRecording = new BackgroundTasks().StopTask(sentCameraToStop, true);

            if (cameraStoppedRecording)
            {
                return Ok("Recording Stopped");
            } else
            {
                return StatusCode(417, "No Camera / Camera Not Stopped");
            }
        }

        // Post: api/camera/addcamera
        // Receives a single camera to the database to record
        [HttpPost("api/[controller]/recordcamera")]
        public IActionResult RecordCamerasPost([FromBody]RecordCamera sentCameraToRecord)
        {
            if (sentCameraToRecord.recordingCameraId <= 0)
            {
                return StatusCode(417, "Malformed Camera Data");

            }
            else if (sentCameraToRecord.recordDelay * 1000 < 5000)
            {
                return StatusCode(417, "Too Small of a Delay");
            }

            sentCameraToRecord.userName = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            sentCameraToRecord.recordDelay = sentCameraToRecord.recordDelay * 1000; // Convert to ms

            bool isRecording = new BackgroundTasks().StartCameraRecording(sentCameraToRecord);

            if (isRecording)
            {
                return Ok( "{\"Recording Started\": true}");
            }

            return StatusCode(417, "{\"Invalid Camera\": true}");
        }

        [HttpGet("api/[controller]/displaytasks")]
        public IActionResult DisplayTasks()
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            return Ok(new BackgroundTasks().ReturnTasks(currentUser));
        }

        // Post: api/camera/addcamera
        // Posts a new camera to the database
        [HttpPost("api/[controller]/addcamera")]
        public IActionResult AddCamerasToDbPost([FromBody]Camera sentCamera)
        {
            if (sentCamera != null)
            {
                string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
                SimpleCameraUserAccess returnedSimpleUserCamera = new EyesOnTheNetRepository().AddCameraToDatabaseProcess(sentCamera, currentUser);
                return Ok(returnedSimpleUserCamera);
            } else
            {
                return StatusCode(417, "Malformed Camera Data");
            }
        }

        // DELETE api/camera/5
        [HttpDelete("api/[controller]/{cameraId:int}")]
        public IActionResult DeleteCamera(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Camera returnedDeletedCamera = new EyesOnTheNetRepository().RemoveCameraFromDatabase(currentUser, cameraId);

            if (returnedDeletedCamera != null)
            {
                return Ok(returnedDeletedCamera);
            } else
            {
                return StatusCode(417, "Camera Not Removed");
            }
        } 
               
        // Allows the initial creation and population of the database
        // GET: api/camera/build_database
        // Used for initial database build
        /*
        [HttpGet("api/[controller]/build_database")]
        public IActionResult GetBuildDatabase()
        {
            EyesOnTheNetRepository newEotnRepo = new EyesOnTheNetRepository();
            newEotnRepo.AddFakeEverything();
            return Ok("Successful DB Creation");
        }
        */
    }
}
