using System;
using System.Collections.Generic;
using System.IO;

namespace TorchFilterStreams
{
    public class IPOutput : IDisposable, IOutput
    {
        StreamWriter writer;

        public IPOutput(string path)
        {
            writer = new StreamWriter(path);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Write(List<string> IPs)
        {
            foreach (string IP in IPs)
            {
                writer.WriteLine(IP);
            }
            writer.Flush();
        }
    }
}
