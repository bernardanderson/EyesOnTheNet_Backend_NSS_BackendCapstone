using Microsoft.AspNetCore.Mvc;
using EyesOnTheNet.DAL;
using System.IdentityModel.Tokens.Jwt;
using EyesOnTheNet.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EyesOnTheNet.Controllers
{
    public class FileController : Controller
    {
        // GET api/file/5
        [HttpGet("api/[controller]/{cameraId:int}/{timerInterval:int}")]
        public void SaveSingleCameraPicture(int cameraId, int timerInterval)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            new FileRequests(currentUser, cameraId).StartTimer(timerInterval); // For Backend Timed FileSave
        }

        [HttpGet("api/[controller]/{cameraId:int}")]
        public void SaveSingleCameraPicture(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            new FileRequests(currentUser, cameraId).SaveCameraPhoto();
        }

    }
}
