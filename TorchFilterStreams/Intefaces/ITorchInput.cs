using System.Collections.Generic;

namespace TorchFilterStreams
{
    public interface ITorchInput : IInput
    {
        List<List<string>> Arguments { get; }
        List<string> Indexs { get; }
    }
}
