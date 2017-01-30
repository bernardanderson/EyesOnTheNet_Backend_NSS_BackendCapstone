using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyesOnTheNet.Models
{
    public class Camera
    {
        [Key]
        public int CameraId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Type { get; set; }
        public string WebAddress { get; set; }
        public string LoginName { get; set; }
        public string LoginPass { get; set; }
        public int Private { get; set; } 
        public string Location { get; set; } 
        [ForeignKey("User")]
        public virtual User CreatedBy { get; set; }
    }

}
