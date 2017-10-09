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

namespace PingMetricsGrafana
{
    class Program
    {
        static void Main(string[] args)
        {
            // ping hosts and get miliseconds to grafana
            // :)
            // :D
            RunMain();
            Console.ReadLine();
        }

        public static void RunMain()
        {
            // load initial configuration
            RootObject r = LoadJSONdata();
            List<Machine> m = r.machines;
            string db = r.database;
            foreach(var item in m)
            {
                Thread t = new Thread(() => GetResponse(item.ipaddress, item.alias, db));
                t.Start();
            }
        }

        public static RootObject LoadJSONdata()
        {
            string jsonFile = File.ReadAllText("C:\\Temp\\config.json");
            RootObject r = JsonConvert.DeserializeObject<RootObject>(jsonFile);
            return r;
        }

        public static void GetResponse(string ipaddress, string alias, string db)
        {
            int ms;
            Ping ping = new Ping();
            try
            {
                var reply = ping.Send(ipaddress, 3000);
                ms = (int)reply.RoundtripTime;
                Console.WriteLine("host: {0} miliseconds: {1}", ipaddress, ms);
            }
            catch (Exception e)
            {
                ms = -100;
                Console.WriteLine("host: {0} miliseconds: {1}", ipaddress, ms);
            }
        }

        public static void WriteToInfluxDB()
        {
            // write data into influxdb
        }
    }
}
