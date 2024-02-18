using System;
using System.IO;

namespace TorchFilterAndAbuseChecker
{

    internal class AppConfig : IDisposable
    {
        StreamReader reader;
        public string ApiKey { get; private set; }
        public int MinAbuseScore { get; private set; }
        public string IpToCheck { get; private set; }

        public AppConfig(string path)
        {
            reader = new StreamReader(path);

            ApiKey = "";
            MinAbuseScore = 0;
            IpToCheck = "dst";

            Console.WriteLine($"Loading config from {path}");

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!line.Contains("#"))
                {
                    string[] param = line.Split('=');
                    switch (param[0])
                    {
                        case "API_KEY":
                            ApiKey = param[1];
                            Console.WriteLine($"Loaded API Key from file");
                            break;

                        case "MINIMAL_ABUSE_SCORE":
                            MinAbuseScore = int.Parse(param[1]);
                            Console.WriteLine($"Setted minimal abuse score as {param[1]}%");
                            break;

                        case "IP_TO_CHECK":
                            var value = param[1].ToLower();
                            if(value == "src") 
                                value = "src";
                            Console.WriteLine($"Setted ip to check as {value}");
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
