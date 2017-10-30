using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SessionsGen
{

    // SessionsGen generates "well formed" XML files that can be imported by
    // SuperPutty to create a large number (<500) of putty sessions quickly. 
    // These sessions can then be used to monitor large (or small) outages.


    class Program
    {
        public static string[] tempStore;
        public static string ticket;

        public class SessionData 
        {
            public static string hostName = "";

            public static string RowOpen = "<SessionData ";
            public static string SessionId = "SessionId=" + hostName;
            public static string SessionName = "SessionName=" + hostName;
            public static string ImageKey = "ImageKey=" + '"' + "computer" + '"' + " ";
            public static string Host = "Host=" + '"' + "" + '"' + " ";
            public static string Port = "Port=" + '"' + "22" + '"' + " " + " ";
            public static string Proto = "Proto=" + '"' + "SSH" + '"' + " ";
            public static string PuttySession = "PuttySession=" + '"' + "Default Settings" + '"' + " ";
            //public static string Username = "Username=" + '"' + "bob"+'"' + " ";
            //public static string ExtraArgs = "ExtraArgs=" + '"' + "-pw Fly2Mars" + '"' + " ";
            public static string Username = "Username=" + '"' + "" + '"' + " ";
            public static string ExtraArgs = "ExtraArgs=" + '"' + "" + '"' + " ";
            public static string SPSLFileName = "SPSLFileName=" + '"' + '"' + " ";
            public static string RowClose = "/>" + "\n";
                       
        }

        static void Main(string[] args)
        {
            // various input (pathX) and output (pathZ) file paths
            //string pathX = @"C:\Users\Dragon\Documents\Visual Studio 2017\Projects\SessionsGen\SessionsGen\data\SiteList.txt";
            //string pathZ = @"C:\Users\Dragon\Documents\Visual Studio 2017\Projects\SessionsGen\SessionsGen\data\sessions.xml";

            //string pathX = @"C:\Sessions\SiteList.txt";
            //string pathZ = @"C:\Sessions\sessions.xml";

            string desktop1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pathX = System.IO.Path.Combine(desktop1, "SiteList.txt");

            string desktop2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pathZ = System.IO.Path.Combine(desktop2, "sessions.XML");
            
            string[] rowArray = new string[500];

            int index = 1;      // Must be 1 for TempStore to copy

            //Console.Write(pathZ);

            try
            {
                // get the ticket number
                Console.Write("Ticket number please... ");
                ticket = Console.ReadLine();

                // Open the text file (pathX) using a stream reader.
                using (StreamReader sr = new StreamReader(pathX))
                {
                    String line;
                    // Read the stream to a string, and then store it in an array of strings.
                    while ((line = sr.ReadLine()) != null)
                    {

                    SessionData.hostName = "Host=" + '"' + line + '"' + " ";
                    SessionData.SessionId = "SessionId=" + '"' + ticket + '/' + line + '"' + " ";
                    SessionData.SessionName = "SessionName=" + '"' + line + '"' + " ";
                    String row = SessionData.RowOpen + SessionData.SessionId;
                    row += SessionData.SessionName;
                    row += SessionData.ImageKey;
                    row += SessionData.hostName;
                    row += SessionData.Port;
                    row += SessionData.PuttySession;
                    row += SessionData.Username;
                    row += SessionData.ExtraArgs;
                    row += SessionData.SPSLFileName;
                    row += SessionData.RowClose;
                    row = row.ToString().TrimEnd('\r', '\n');

                        rowArray[index] = row;

                    //Console.WriteLine(row);
                    //Console.WriteLine(index);

                    index = index + 1;
                    }

                    // Create 2nd array of the correct size and copy sessionArray into it
                    string[] sessionArray = new string[index];

                    for (int a = 0; a < index; a = a + 1)
                    {
                        //Console.WriteLine(rowArray[a]);
                        sessionArray[a] = rowArray[a];
                        tempStore = sessionArray;
                        //Console.WriteLine(a);
                        //Console.WriteLine(index);
                    }
                    
                    //Display how many sessions created
                    Console.Write("Sessions created: ");
                    Console.WriteLine(index-1);
                   
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            // Create a file (using pathZ) and write the session data in tempStore to it.
            try
            {
                string ArrayOfSessionDataOpen = "<ArrayOfSessionData ";
                string xmlns1 = "xmlns:xsi=" +'"' + "http://www.w3.org/2001/XMLSchema-instance" + '"' + "  ";
                string xmlns2 = "xmlns:xsd=" +'"' + "http://www.w3.org/2001/XMLSchema" + '"' +">";
                string header;
                string OpenArray;
                string CloseArray = "</ArrayOfSessionData>";

                // #1 write xml version header
                header = "<?xml version=" +'"' + "1.0" + '"' + " "+ "encoding="+ '"' + "utf-8" + '"' + "?> "+ Environment.NewLine;
                System.IO.File.WriteAllText(pathZ, header);
                
                // #2 assemble string and write "open array" xml           
                OpenArray = ArrayOfSessionDataOpen + xmlns1 + xmlns2;
                System.IO.File.AppendAllText(pathZ, OpenArray);

                // #3 write session data rows
                System.IO.File.AppendAllLines(pathZ, tempStore);

                // #4 write session data "close array" xml
                System.IO.File.AppendAllText(pathZ, CloseArray);

                // report file creation was successfull
                Console.Write("sessions.XML file is ready, press ENTER to continue... ");
                string waste = Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
