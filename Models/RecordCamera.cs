using System.Threading;

namespace EyesOnTheNet.Models
{
    public class RecordCamera
    {
        public string userName { get; set; }
        public int recordDelay { get; set; }
        public int recordingCameraId { get; set; }
        public CancellationTokenSource userCancellationTokenSrc { get; set; }
    }
}
