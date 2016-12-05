using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class Camera
    {
        [Key]
        public Guid CameraId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Type { get; set; }
        public string Location { get; set; }
        public string LoginName { get; set; }
        public string LoginPass { get; set; }
        public bool Private { get; set; }
        public User CreatedBy { get; set; }
    }
}
