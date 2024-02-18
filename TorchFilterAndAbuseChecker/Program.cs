using TorchFilterStreams;

namespace TorchFilterAndAbuseChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppConfig config = new AppConfig(@"..\config.txt");
            ITorchInput reader = new TorchInput(@"..\input.txt");

            IIPListInput blacklist = new IPInput(@"..\blacklist.txt");
            IIPListInput whitelist = new IPInput(@"..\whitelist.txt");

            var checker = new CheckEndpoint(reader, config, whiteList: whitelist, blackList: blacklist);

            IOutput mainWriter = new IPOutput(@"..\output.txt");
            IOutput blackListWriter = new AppendIPOutput(blacklist);

            var abusedIPs = checker.GetListOfAbusedIps();

            mainWriter.Write(abusedIPs);
            blackListWriter.Write(abusedIPs);
        }
    }
}
