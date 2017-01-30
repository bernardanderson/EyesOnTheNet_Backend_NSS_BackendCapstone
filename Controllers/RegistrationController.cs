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
        private EyesOnTheNetRepository newRepo;
        public RegistrationController(EyesOnTheNetRepository repo)
        {
            newRepo = repo;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]User sentUser)
        {
            KeyValuePair<bool, string> registrationResult = newRepo.RegisterUser(sentUser);

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
