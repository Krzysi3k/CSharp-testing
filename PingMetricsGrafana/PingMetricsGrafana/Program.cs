using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using System.Net;


namespace PingMetricsGrafana
{
    class Program
    {
        static void Main(string[] args)
        {
            // initial configuration:
            RootObject json = LoadJSONdata();
            List<Machine> m = json.machines;
            string db = json.database;

            while (true)
            {
                foreach (var item in m)
                {
                    Thread t = new Thread(() => GetResponse(item.ipaddress, item.alias, db));
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

        public static void GetResponse(string ipaddress, string alias, string db)
        {
            int ms;
            Ping ping = new Ping();
            try
            {
                var reply = ping.Send(ipaddress, 4000);
                ms = (int)reply.RoundtripTime;
                //Console.WriteLine("host: {0} alias: {1} ms: {2}", ipaddress, alias, ms);
                WriteToInfluxDB(alias, ms, db);
            }
            catch (Exception e)
            {
                ms = -100;
                //Console.WriteLine("host: {0} alias: {1} ms: {2}", ipaddress, alias, ms);
                WriteToInfluxDB(alias, ms, db);
            }
        }

        public static void WriteToInfluxDB(string alias, int ms, string db)
        {
            Uri url = new Uri("http://10.24.69.6:8086/write?db=" + db);
            WebClient web = new WebClient();
            string measurement = "ping_request";
            string strPOST = measurement + ",servername=" + alias + " " + "ms=" + ms;
            web.UploadString(url, strPOST);
        }
    }
}
