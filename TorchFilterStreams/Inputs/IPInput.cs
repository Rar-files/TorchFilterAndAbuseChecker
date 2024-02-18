
using System.Collections.Generic;
using System.IO;

namespace TorchFilterStreams
{
    public class IPInput : IIPListInput
    {
        StreamReader reader;
        public List<string> Ips { get; private set; }

        public string Path { get; private set; }

        public IPInput(string path)
        {
            Path = path;
            reader = new StreamReader(path);
            Ips = new List<string>();
            LoadFile();
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        public void LoadFile()
        {
            Ips.Clear();
            string line;

            while ((line = reader.ReadLine()) != null)
                Ips.Add(line);

            reader.Close();
        }
    }
}
