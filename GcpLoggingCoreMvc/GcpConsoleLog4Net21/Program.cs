using System;
using Google.Cloud.Logging.Log4Net;
using log4net;

namespace GcpConsoleLog4Net21
{
    //PS  $env:GOOGLE_APPLICATION_CREDENTIALS = "C:\rliu-OneDrive\OneDrive\IT\Google service account key when generate it ask to save  ryantest1-4fd63-447661f46186.json"

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

           var c = log4net.Config.XmlConfigurator.Configure();

            ILog log = LogManager.GetLogger(typeof(Program));
            log.Info("Gcp logging from log4net in console app.");  //not working, just see console log, never write to GCP

            LogManager.Flush(50);
        }
    }
}
