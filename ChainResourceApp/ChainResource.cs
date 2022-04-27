using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainResourceApp
{


    public class ChainResource : ChainResourceInterface
    {
        private Dictionary<string, string> storageOne = new Dictionary<string, string>();
        private Dictionary<string, string> storageTwo = new Dictionary<string, string>();
        private Dictionary<string, string> storageThree = new Dictionary<string, string>();
        private readonly Dictionary<string, string> storageFour = new Dictionary<string, string>();

        private static readonly string APP_ID = "ab195a1adf624c4cbd0bb99c37d96ff1";

        public static string API_PATH = "https://openexchangerates.org/api/latest.json?app_id=";


        static ChainResource instance;
        string dataResult = "";
        protected ChainResource()
        {

        }
        public static ChainResource Instance()
        {
       
            if (instance == null)
            {
                instance = new ChainResource();
            }
            return instance;
        }

        public Task GetValue()
        {

            List<Dictionary<string, object>> allDictionarios = new List<Dictionary<string, object>>();

            var taskRead =  ReadData().Wait(3600000);          
            var taskWrite =  WriteData().Wait(14400000);
            if (dataResult != null)
            {
                var json = File.ReadAllText(@"c:\temp\MyTest.txt");
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                foreach (var item in result)
                {
                    storageOne.Add(item.Key.ToString(),item.Value.ToString());
                }
                return Task.CompletedTask;

            }
            else
                throw new NotImplementedException();
        }

        private Task WriteData()
        {
            var fileSystemTimer = new PeriodicTimer(TimeSpan.FromSeconds(14400));
            string path = @"c:\temp\MyTest.txt";
           
                using (StreamWriter sw = File.CreateText(path))
                {
                     sw.WriteLine(dataResult);
                     return Task.CompletedTask;
                }
            
              
            throw new NotImplementedException();
        }

        private Task ReadData()
        {
            string path = @"c:\temp\MyTest.txt";
            using (StreamWriter sw4 = File.CreateText(path))
                {
                    HttpClient httpClient = new HttpClient();
                    
                    var responseTask = httpClient.GetAsync(API_PATH + APP_ID);

                    Console.WriteLine("Calling API...");
                    responseTask.Wait();
                    if (responseTask.IsCompleted)
                    {
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var messageTask = result.Content.ReadAsStringAsync();
                            messageTask.Wait();
                            Console.WriteLine("Data from API" + messageTask.Result);
                            dataResult = messageTask.Result;

                        return messageTask;
                        }
                    }
                }
            
            return  null;

        }

        void ChainResourceInterface.GetValue()
        {
            throw new NotImplementedException();
        }
    }
}
    
