using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class RecordCameras
    {
        public string userName { get; set; }
        public int recordDelay { get; set; }
        public List<int> recordingCameras { get; set; }
        public List<Camera> fullRecordingCameras { get; set; }
    }
}
