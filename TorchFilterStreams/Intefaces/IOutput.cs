using System.Collections.Generic;

namespace TorchFilterStreams
{
    public interface IOutput
    {
        void Write(List<string> IPs);
    }
}
