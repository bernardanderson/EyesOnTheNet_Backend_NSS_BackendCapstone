using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using EyesOnTheNet.Models;
using EyesOnTheNet.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using Microsoft.Net.Http.Headers;

// Controls access to the Registration of a new user
namespace EyesOnTheNet.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]User sentUser)
        {
            EyesOnTheNetRepository context = new EyesOnTheNetRepository();
            KeyValuePair<bool, string> registrationResult = context.RegisterUser(sentUser);

            if (registrationResult.Key)
            {
                return Ok(registrationResult.Value);
            }
            else
            {
                return NotFound(registrationResult.Value);
            };
        }
    }
}
