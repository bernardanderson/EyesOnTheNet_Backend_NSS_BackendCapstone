using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class CameraAccess
    {
        public int CameraId { get; set; }
        public string CameraURI { get; set; }
        public bool HasAccess { get; set; }
        public string UserName { get; set; }
    }
}
