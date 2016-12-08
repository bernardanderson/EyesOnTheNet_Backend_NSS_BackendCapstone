using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EyesOnTheNet.DAL;
using System.Net.Http;
using EyesOnTheNet.Models;
using Microsoft.AspNetCore.Authorization;

namespace EyesOnTheNet.Controllers
{
    [Route("api/[controller]")]
    public class HttpController : Controller
    {
        // GET api/http
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            Picture cameraPicture = await new HttpRequests().GetSnapshot();
            return File(cameraPicture.data, cameraPicture.encodeType);
        }

        /*
        // GET api/http/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await new HttpRequests().GetParameters();
        }
        */

        // GET api/http/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //EyesOnTheNetRepository myEyes = new EyesOnTheNetRepository();

            // myEyes.AddFakeUser();
            //   OR
            // myEyes.AddFakeCamera();

            return "Int Route is Working";
        }

        // GET api/http/
        [HttpGet("{id:bool}")]
        [Authorize]
        public string Get(bool id)
        {
            //EyesOnTheNetRepository myEyes = new EyesOnTheNetRepository();

            // myEyes.AddFakeUser();
            //   OR
            // myEyes.AddFakeCamera();

            return "Bool Route is Working";
        }

        // POST api/http
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/http/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/http/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
