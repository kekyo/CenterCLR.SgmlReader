using System;
using System.IO;
using System.Net.Http;
using System.Text;
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
                using (var stream = await httpClient.GetStreamAsync("https://github.com/"))
                {
                    var tr = new StreamReader(stream, Encoding.UTF8, true);
                    var document = SgmlReader.Parse(tr);

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
