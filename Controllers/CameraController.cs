using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EyesOnTheNet.DAL;
using EyesOnTheNet.Models;
using System.IdentityModel.Tokens.Jwt;

namespace EyesOnTheNet.Controllers
{
    public class CameraController : Controller
    {
        // GET: api/camera
        // Gets the list of a users cameras
        [HttpGet("api/[controller]")]
        [Authorize]
        public IEnumerable<Camera> GetListOfUserCameras()
        {
            EyesOnTheNetRepository newEotnRepo = new EyesOnTheNetRepository();
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;
            return newEotnRepo.ReturnUserCameras(currentUser);
        }

        // GET: api/camera/5/snapshot
        // Pulls an image from a single camera (based on it's table ID) for display
        [HttpGet("api/[controller]/{cameraId:int}/snapshot")]
        public async Task<ActionResult> GetSingleCameraImage(int cameraId)
        {
            EyesOnTheNetRepository newEotnRepo = new EyesOnTheNetRepository();
            string currentUser = new JwtSecurityToken(Request.Cookies["access_token"]).Subject;

            if (newEotnRepo.CanAccessThisCamera(currentUser, cameraId))
            {
                Picture cameraPicture = await new CameraRequests().GetSnapshot();
                return File(cameraPicture.data, cameraPicture.encodeType);
            } else
            {
                return Unauthorized();
            }
        }

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
    }
}
