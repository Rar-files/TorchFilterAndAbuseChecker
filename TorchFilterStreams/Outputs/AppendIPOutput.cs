using System;
using System.Collections.Generic;
using System.IO;

namespace TorchFilterStreams
{
    public class AppendIPOutput : IDisposable, IOutput
    {
        private IIPListInput _ipList;
        private StreamWriter writer;

        public AppendIPOutput(IIPListInput ipList)
        {
            _ipList = ipList;
            writer = new StreamWriter(ipList.Path);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Write(List<string> IPs)
        {
            foreach(string line in IPs)
            {
                if(!_ipList.Ips.Contains(line))
                {
                    _ipList.Ips.Add(line);
                }
            }

            Write();
        }

        public void Write()
        {
            foreach (string line in _ipList.Ips)
            {
                writer.WriteLine(line);
            }

            writer.Flush();
        }
    }
}
