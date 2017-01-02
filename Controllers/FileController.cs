using Microsoft.AspNetCore.Mvc;
using EyesOnTheNet.DAL;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using EyesOnTheNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EyesOnTheNet.Controllers
{
    public class FileController : Controller
    {
        // API Access to have the BackEnd repeat the CamStream Saving
        /*
        // GET api/file/5
        [HttpGet("api/[controller]/{cameraId:int}/{timerInterval:int}")]
        public void SaveSingleCameraPicture(int cameraId, int timerInterval)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            new FileRequests(currentUser, cameraId).StartTimer(timerInterval); // For Backend Timed FileSave
        }
        */

        // API Access point to save a single camera snapshot to the HD and DB
        [HttpGet("api/[controller]/{cameraId:int}")]
        [Authorize]
        public void SaveSingleCameraPicture(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            new FileRequests(currentUser, cameraId).SaveCameraPhoto();
        }

        // API Access point to save a single camera snapshot to the HD and DB
        [HttpGet("api/[controller]/photolist")]
        [Authorize]
        public IActionResult ReturnRecordedCameraInfo()
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            List<SimplePhoto> returnedPhotoList = new FileRequests(currentUser).SendPhotoList();

            if (returnedPhotoList != null)
            {
                return Ok(returnedPhotoList);
            }
            else
            {
                return StatusCode(417, "Photo Data Not Accessible");
            }
        }

        // API Access point to retrieve a single camera snapshot from the HD and DB
        [HttpGet("api/[controller]/{photoId:int}/dvrpics")]
        [Authorize]
        public IActionResult ReturnRecordedCameraPictures(int photoId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Picture dvrPicture = new FileRequests(currentUser).GetDvrPhoto(photoId);

            if (dvrPicture != null)
            {
                return File(dvrPicture.data, dvrPicture.encodeType);
            }
            else
            {
                // Db Error
                return StatusCode(417, "Photo Data Not Accessible");
            }
        }
    }
}
