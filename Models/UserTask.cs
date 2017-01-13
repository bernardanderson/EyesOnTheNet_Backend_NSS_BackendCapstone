using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EyesOnTheNet.Models
{
    public class UserTask
    {
        public string userName { get; set; }
        public CancellationTokenSource userCancellationTokenSrc { get; set; }
    }
}
