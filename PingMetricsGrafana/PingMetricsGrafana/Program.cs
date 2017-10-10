using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net.Http;

namespace PingMetricsGrafana
{
    class Program
    {
        static void Main(string[] args)
        {
            // RUN HERE
            // RunMain();
            WriteToInfluxDB("gunwo", 5, "db");
            Console.ReadLine();
        }

        public static void RunMain()
        {
            // load initial configuration
            RootObject json = LoadJSONdata();
            List<Machine> m = json.machines;
            string db = json.database;
            HttpClient httpClient = new HttpClient();

            while (true)
            {
                foreach (var item in m)
                {
                    Thread t = new Thread(() => GetResponse(item.ipaddress, item.alias, db, httpClient));
                    t.Start();
                }
                Thread.Sleep(10000);
                Console.Clear();
            }
        }

        public static RootObject LoadJSONdata()
        {
            string jsonFile = File.ReadAllText("C:\\Temp\\config.json");
            RootObject r = JsonConvert.DeserializeObject<RootObject>(jsonFile);
            return r;
        }

        public static void GetResponse(string ipaddress, string alias, string db, HttpClient httpClient)
        {
            int ms;
            Ping ping = new Ping();
            try
            {
                var reply = ping.Send(ipaddress, 3000);
                ms = (int)reply.RoundtripTime;
                Console.WriteLine("host: {0} alias: {1} ms: {2}", ipaddress, alias, ms);
            }
            catch (Exception e)
            {
                ms = -100;
                Console.WriteLine("host: {0} alias: {1} ms: {2}", ipaddress, alias, ms);
            }
        }

        public static void WriteToInfluxDB(string alias, int ms, string db)
        {
            string measurement = "ping_request";
            string url = measurement + ",servername=" + alias + " ms=" + ms;
            Console.WriteLine(url);
        }
    }
}
