namespace AddisonWesley.Michaelis.EssentialCSharp.Chapter18.Listing18_15
{
    using System;
    using System.IO;
    using System.Net;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading;

    public class Program
    {
        private static async Task WriteWebRequestSizeAsync(
            string url)
        {
            try
            {
                WebRequest webRequest =
                    WebRequest.Create(url);

                Console.WriteLine(string.Format("WebRequest.Create(url) in Thread {0}", Thread.CurrentThread.ManagedThreadId));

                WebResponse response =
                    await webRequest.GetResponseAsync();
                using(StreamReader reader =
                    new StreamReader(
                        response.GetResponseStream()))
                {
                    Console.WriteLine(string.Format("response.GetResponseStream() in Thread {0}", Thread.CurrentThread.ManagedThreadId));

                    string text =
                        await reader.ReadToEndAsync();

                    Console.WriteLine(string.Format("after reader.ReadToEndAsync() in Thread {0}", Thread.CurrentThread.ManagedThreadId));

                    Console.WriteLine(
                        FormatBytes(text.Length));
                    throw new WebException();
                }
            }
            catch(WebException ex)
            {
                // ...
                Console.WriteLine(string.Format("Exception Handle in Thread {0}", Thread.CurrentThread.ManagedThreadId));

                Console.WriteLine(ex.Message);
            }
            catch(IOException)
            {
                // ...
            }
            catch(NotSupportedException)
            {
                // ...
            }
        }

        public static void Main(string[] args)
        {
            string url = "http://www.IntelliTect.com";
            if(args.Length > 0)
            {
                url = args[0];
            }

            Console.Write(url);

            Task task = WriteWebRequestSizeAsync(url);

            while(!task.Wait(100))
            {
                Console.Write(".");
            }
        }

        static public string FormatBytes(long bytes)
        {
            string[] magnitudes =
                new string[] { "GB", "MB", "KB", "Bytes" };
            long max =
                (long)Math.Pow(1024, magnitudes.Length);

            return string.Format("{1:##.##} {0}",
                magnitudes.FirstOrDefault(
                    magnitude =>
                        bytes > (max /= 1024)) ?? "0 Bytes",
                    (decimal)bytes / (decimal)max).Trim();
        }
    }
}





