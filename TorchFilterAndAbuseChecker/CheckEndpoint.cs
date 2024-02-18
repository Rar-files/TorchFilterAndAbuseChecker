using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using TorchFilterStreams;

namespace TorchFilterAndAbuseChecker
{
    internal class CheckEndpoint
    {
        ITorchInput _input;
        AppConfig _appConfig;
        IIPListInput _whiteList;
        IIPListInput _blackList;

        public CheckEndpoint(ITorchInput input, AppConfig appConfig, IIPListInput whiteList = null, IIPListInput blackList = null)
        {
            _input = input;
            _appConfig = appConfig;
            _whiteList = whiteList;
            _blackList = blackList;
        }

        public dynamic CheckInGlobal(string ip)
        {
            var client = new RestClient();
            var request = new RestRequest("https://api.abuseipdb.com/api/v2/check",Method.Get);
            request.AddHeader("Key", _appConfig.ApiKey);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("ipAddress", ip);
            request.AddParameter("maxAgeInDays", "90");
            request.AddParameter("verbose", "");
            Console.WriteLine($"Request to AbuseDB was sent...");

            var response = client.Execute(request);
            dynamic result = JsonConvert.DeserializeObject(response.Content);

            Console.WriteLine($"Recived response. Abuse score: {result.data.abuseConfidenceScore}%");

            return result;
        }

        public bool Check(string ip)
        {
            Console.WriteLine($"Check ip: {ip}");

            if (_whiteList.Ips.Contains(ip))
            {
                Console.WriteLine($"Whitelisted IP");
                return false;
            }

            if (_blackList.Ips.Contains(ip))
            {
                Console.WriteLine($"Blacklisted IP");
                return true;
            }

            var response = CheckInGlobal(ip);

            if (isAbussed(response))
            {
                _blackList.Ips.Add(ip);
                return true;
            }

            Console.WriteLine($"Non abuse IP - skipped.");
            return false;
        }

        public List<string> GetListOfAbusedIps()
        {
            var result = new List<string>();

            int ipsToCheckIndex;
            if (_appConfig.IpToCheck == "src")
                ipsToCheckIndex = _input.Indexs.IndexOf("SRC-ADDRESS");
            else
                ipsToCheckIndex = _input.Indexs.IndexOf("DST-ADDRESS");

            foreach (var item in _input.Arguments)
            {
                var ipToCheck = item[ipsToCheckIndex];
                if (Check(ipToCheck))
                {
                    Console.WriteLine($"Marked as abuse IP");
                    result.Add(ipToCheck);
                }
            }

            return result;
        }

        private bool isAbussed(dynamic response) => response.data.abuseConfidenceScore > _appConfig.MinAbuseScore;
    }
}
