using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public sealed class ChangeFilePathMessage
    {
        public string FilePath { get; set; }
    }
}
