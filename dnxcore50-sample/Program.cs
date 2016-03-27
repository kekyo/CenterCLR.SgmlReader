using System;
using System.Net.Http;
using System.Threading.Tasks;
using CenterCLR.Sgml;

namespace ConsoleApplication
{
    public class Program
    {
        private static async Task<int> MainAsync(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                using (var stream = await httpClient.GetStreamAsync("http://www.kekyo.net/"))
                {
                    var document = SgmlReader.Parse(stream);
                    
                    Console.WriteLine(document);
                    
                    return 0;
                }
            }
        }
        public static int Main(string[] args)
        {
            return MainAsync(args).Result;
        }
    }
}
