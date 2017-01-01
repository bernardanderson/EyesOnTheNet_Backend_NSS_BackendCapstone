﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public string Filename { get; set; }
        public string FileLocation{ get; set; }
        public long CreationDate { get; set; }
        public int CameraId { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
