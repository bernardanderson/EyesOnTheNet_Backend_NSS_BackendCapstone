using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class SimpleCameraUserAccess
    {
        public string CameraIdHash { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
