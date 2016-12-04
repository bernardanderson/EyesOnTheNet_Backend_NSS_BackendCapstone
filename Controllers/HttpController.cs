using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EyesOnTheNet.DAL;
using System.Net.Http;

namespace EyesOnTheNet.Controllers
{
    [Route("api/[controller]")]
    public class HttpController : Controller
    {
        // GET api/http
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var stream = await new HttpRequests().GetSnapshot();
            return File(stream, "image/jpeg");
        }

        // GET api/http/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await new HttpRequests().GetParameters();
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
