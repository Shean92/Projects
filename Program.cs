using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ServerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //read csv file.
            //change the path to have www2.byui.edu/<filepath>
            //query file but don't recieve, only test server status of url
            //repeat
            //Change!!!!!
            string fileToReadFull = @"\\igxur\igxsites\cms101\pb24\lehi3-scraper\src\New Scraper c#\Collect\Collected_Files_employee.csv";

            Console.WriteLine("This will overwrite all lines in the file you have specified, are you ready?");
            Console.ReadLine();
            int length = 0;
            ArrayList urls = new ArrayList();
            // open the file "Collected_Files.csv"
            IEnumerable<string> lines = File.ReadLines(fileToReadFull);
            lines = lines.Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] delimitedLine = line.Split('|');
                string newLine = delimitedLine[0].Replace(@"\\lehi3\emp$\", @"http://emp.byui.edu/");
                newLine = newLine.Replace(@"\", @"/");
                urls.Add(newLine);
                Console.WriteLine(line);
                length++;
            }
            TestServerStatus(urls, length, lines);
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static void TestServerStatus(ArrayList urls, int length, IEnumerable<string> linesOld)
        {
            string newFullPath = @"\\igxur\igxsites\cms101\pb24\lehi3-scraper\src\New Scraper c#\ServerTest\ServerTester\Server_Status_Final_employee.csv";
            using (StreamWriter sw = File.CreateText(newFullPath))
            {
                sw.WriteLine("Url| Status| Directory| Type| Size| Created| Modified| SubDirectories| Owner");
            }
            string errors = "";

            //get the url
            int i = 0;
            foreach (string line in linesOld)
            {

                int status = 0;
                string[] delimitedLine = line.Split('|');
                var watch = System.Diagnostics.Stopwatch.StartNew();
                if(i > length)
                {
                    watch.Stop();
                    continue;
                }
                //test server
                //use url to test server
                //record status but don't recieve content
                //status change
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urls[i].ToString());
                    request.AllowAutoRedirect = false;
                    request.Method = "HEAD";
                    request.Proxy = null;
                    ServicePointManager.DefaultConnectionLimit = 4;
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                            status = (int)response.StatusCode;
                        response.Close();
                    }
                }
                catch (WebException we)
                {
                    try
                    {
                        HttpWebResponse errorResponse = we.Response as HttpWebResponse;
                        status = (int)errorResponse.StatusCode;
                        errorResponse.Close();
                    }
                    catch (NullReferenceException)
                    {
                        HttpWebResponse errorResponse = we.Response as HttpWebResponse;
                        status = (int)errorResponse.StatusCode;
                        errorResponse.Close();
                    }
                }
                catch (UriFormatException)
                {
                    errors += delimitedLine[0] + "\nformat exception\n";
                }
                catch (NullReferenceException)
                {
                    errors += delimitedLine[0] + "\nNull reference exception\n";
                }

                Console.WriteLine("Status recieved: " + status);
                using (StreamWriter sw = new StreamWriter(newFullPath, true))
                {
                    string newLine = String.Format("{0}| {1}| {2}| {3}| {4}| {5}| {6}| {7}| {8}", delimitedLine[0], status, delimitedLine[1], delimitedLine[2], delimitedLine[3], delimitedLine[4], delimitedLine[5], delimitedLine[6], delimitedLine[7]);
                    sw.WriteLine(newLine);
                }
                i++;
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Line: " + i);
                Console.WriteLine(elapsedMs);
            }
            Console.WriteLine(errors);
        }
    }
}
