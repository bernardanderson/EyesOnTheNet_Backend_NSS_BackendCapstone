using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EyesOnTheNet.Controllers
{
    [Route("api/[controller]")]
    public class HttpController : Controller
    {
        // GET api/http
        [HttpGet]
        public Task<Stream> Get()
        {
            HttpCameraAccess currentCameraAccess = new HttpCameraAccess();

            return currentCameraAccess.GetSnapshot();
        }

        // GET api/http/5
        [HttpGet("{id}")]
        public Task<Stream> Get(int id)
        {
            HttpCameraAccess currentCameraAccess = new HttpCameraAccess();

            if (id == 1)
            {
                return currentCameraAccess.GetParameters();
            }

            return null;
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
