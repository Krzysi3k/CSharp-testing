using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net;
using System.Net.Http;

namespace PingMetricsGrafana
{
    class Program
    {
        static void Main(string[] args)
        {

            //run here

            //string db = "ping_request_db";
            //string servername = "W8-PL70003P3";
            //string measurement = "ping_request";
            //int ms = 250;
            //Uri url = new Uri("http://10.24.69.6:8086/write?db="+db);
            //string stringPOST = measurement+",servername="+servername+" "+"ms="+ms;

            //WebClient web = new WebClient();
            //web.UploadString(url, stringPOST);


            RunMain();
            Console.ReadLine();
        }

        public static void RunMain()
        {
            // load initial configuration
            RootObject json = LoadJSONdata();
            List<Machine> m = json.machines;
            string db = json.database;
            Uri url = new Uri("http://10.24.69.6:8086/write?db=" + db);
            WebClient web = new WebClient();

            while (true)
            {
                foreach (var item in m)
                {
                    Thread t = new Thread(() => GetResponse(item.ipaddress, item.alias, db, url, web));
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

        public static void GetResponse(string ipaddress, string alias, string db, Uri url, WebClient web)
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
            // post data to influxdb
            
        }
    }
}
