using System.Collections.Generic;

namespace TorchFilterStreams
{
    public interface IIPListInput : IInput
    {
        List<string> Ips { get; }
    }
}
