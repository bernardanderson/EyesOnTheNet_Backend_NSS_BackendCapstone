using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace EyesOnTheNet.Controllers
{
    public class CameraController : Controller
    {
        [HttpGet("api/[controller]")]
        [Authorize]
        public IEnumerable<SimpleCameraUserAccess> GetListOfUserCameras()
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            return new EyesOnTheNetRepository().ReturnUserCameras(currentUser);
        }

        // GET: api/camera/5/snapshot
        // Pulls an image from a single camera (based on it's table ID) for display
        [HttpGet("api/[controller]/{cameraId:int}/snapshot")]
        [Authorize]
        public async Task<ActionResult> GetSingleCameraImage(int cameraId)
        {
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            Camera returnedUserCamera = new EyesOnTheNetRepository().CanAccessThisCamera(currentUser, cameraId);

            if (returnedUserCamera != null)
            {
                Picture cameraPicture = await new CameraRequests().GetSnapshot(returnedUserCamera);
                return File(cameraPicture.data, cameraPicture.encodeType);
            } else
            {
                Picture cameraPicture = await new CameraRequests().GetSnapshot(new Camera { Type = -1 });
                return File(cameraPicture.data, cameraPicture.encodeType);
            }
        }
        // Post: api/camera/addcamera
        // Posts a new camera to the database
        [HttpPost("api/[controller]/addcamera")]
        [Authorize]
        public IActionResult Post([FromBody]Camera sentCamera)
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
/*
        // GET: api/camera/build_database
        // Used for initial database build
        [HttpGet("api/[controller]/build_database")]
        public IActionResult GetBuildDatabase()
        {
            EyesOnTheNetRepository newEotnRepo = new EyesOnTheNetRepository();
            newEotnRepo.AddFakeEverything();
            return Ok("Successful DB Creation");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
*/
    }
}
