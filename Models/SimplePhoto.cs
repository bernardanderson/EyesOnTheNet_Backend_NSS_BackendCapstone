using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class SimplePhoto
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public List<KeyValuePair<int, long>> PhotoIdTime { get; set; }
    }
}
