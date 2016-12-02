using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EyesOnTheNet.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [Required, MinLength(4), MaxLength(16)]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public List<Cameras> CameraList { get; set; }
    }
}
