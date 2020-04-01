using OpenTokSDK;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TokBoxPOC.ConsoleSever
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenTokSDK.OpenTok openTok = new OpenTok(46617502, "f702511cfae0d823a12e3ee6f358befd8349875c");
            // Create an automatically archived session:
            Session session = openTok.CreateSession(mediaMode: MediaMode.ROUTED, archiveMode: ArchiveMode.ALWAYS);
            // Store this sessionId in the database for later use:
            string sessionId = session.Id;
            string token = session.GenerateToken();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("**************************    TOKBOX SERVER SESSION     ***************************");
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SESSION ID");
            Console.WriteLine("***********************************************************************************");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(sessionId);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("TOKEN");
            Console.WriteLine("***********************************************************************************");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(token);
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.WriteLine("To copy Session id, PRESS S. To copy token press T, To show list of archives press L");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.S)
                {
                    Clipboard.SetText(sessionId);
                    Console.WriteLine("");
                    Console.WriteLine("Session key copied.");
                     
                }
                else if (keyInfo.Key == ConsoleKey.T)
                {
                   
                    Clipboard.SetText(token);
                    Console.WriteLine("");
                    Console.WriteLine("Token copied.");
                    
                }
                else if(keyInfo.Key == ConsoleKey.L)
                {
                   var list= openTok.ListArchives();
                    if (list != null)
                    {
                        int count = 1;
                        foreach(var item in list)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("**************************************    " + count +"     ****************************************");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Created at:" + item.CreatedAt);
                            Console.WriteLine("Duration:  " + item.Duration);
                            Console.WriteLine("Has Audio: " + item.HasAudio);
                            Console.WriteLine("Has Video: " + item.HasVideo);
                            Console.WriteLine("Id:        " + item.Id);
                            Console.WriteLine("Name:      " + item.Name);
                            Console.WriteLine("OutputMode:" + item.OutputMode);
                            Console.WriteLine("Resolution:" + item.Resolution);
                            Console.WriteLine("Session Id:" + item.SessionId);
                            Console.WriteLine("Size:      " + item.Size);
                            Console.WriteLine("Status:    " + item.Status);
                            Console.WriteLine("URL:       " + item.Url);
                            count++;

                        }
                    }
                }
                Console.ReadLine();
            }
        }
      
    }
}
