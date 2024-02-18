using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TorchFilterStreams
{
    public class TorchInput : IDisposable, ITorchInput
    {
        StreamReader reader;
        public List<List<string>> Arguments {  get; private set; }
        public List<string> Indexs { get; private set; }
        public string Path { get; private set; }

        public TorchInput(string path) {
            Path = path;
            reader = new StreamReader(path);
            Arguments = new List<List<string>>();

            Indexs = new List<string>() { "MAC-PROTOCOL", "IP-PROTOCOL", "SRC-PORT", "DST-ADDRESS", "DST-PORT", "TX", "RX", "TX-PACKETS", "RX-PACKETS" };

            LoadFile();
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        public void LoadFile()
        {
            Arguments.Clear();
            string line;

            List<string> SplitInput(string input) => Regex.Split(input, @"\s{1,}").ToList();


            if ((line = reader.ReadLine()) != null && line.StartsWith("MAC"))
            {
                Indexs.Clear();
                var splittedLine = SplitInput(line);
                foreach (var item in splittedLine)
                {
                    Indexs.Add(item);
                }
            }

            int protocolIndex = Indexs.IndexOf("IP-PROTOCOL");
            int srcPortIndex =Indexs.IndexOf("SRC-PORT");
            int dstPortIndex = Indexs.IndexOf("DST-PORT");

            do
            {
                var splittedLine = SplitInput(line);

                if (splittedLine[protocolIndex].ToLower() == "icmp")
                {
                    splittedLine.Insert(srcPortIndex, "");
                    splittedLine.Insert(dstPortIndex, "");
                }
                else
                {

                    if (splittedLine[(srcPortIndex + 1)][0] == '(')
                    {
                        var surfix = splittedLine[(srcPortIndex + 1)];
                        splittedLine.RemoveAt(srcPortIndex + 1);
                        splittedLine[srcPortIndex] = splittedLine[srcPortIndex] + surfix;
                    }
                    if (splittedLine[(dstPortIndex + 1)][0] == '(')
                    {
                        var surfix = splittedLine[(dstPortIndex + 1)];
                        splittedLine.RemoveAt(dstPortIndex + 1);
                        splittedLine[dstPortIndex] = splittedLine[dstPortIndex] + surfix;
                    }
                }

                Arguments.Add(splittedLine);
            } while ((line = reader.ReadLine()) != null);
        }
    }
}
