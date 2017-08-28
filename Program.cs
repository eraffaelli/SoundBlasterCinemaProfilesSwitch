using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SoundBlasterCinemaProfilesSwitch
{
    class Program
    {
        private static void Main(string[] args)
        {
            WarningTitle();
            int i = GetProfiles();
            TerminateProcess();
            int profileChoice = GetProfileChoice(i);
            SetRegistryValue(profileChoice, i);
            LaunchProcess();
            Console.WriteLine("Press Enter to close the console");
            Console.ReadLine();
        }

        private static void WarningTitle()
        {
            string attention = @"                                                                       

            ██╗    ██╗ █████╗ ██████╗ ███╗   ██╗██╗███╗   ██╗ ██████╗                           
            ██║    ██║██╔══██╗██╔══██╗████╗  ██║██║████╗  ██║██╔════╝                           
            ██║ █╗ ██║███████║██████╔╝██╔██╗ ██║██║██╔██╗ ██║██║  ███╗                          
            ██║███╗██║██╔══██║██╔══██╗██║╚██╗██║██║██║╚██╗██║██║   ██║                          
            ╚███╔███╔╝██║  ██║██║  ██║██║ ╚████║██║██║ ╚████║╚██████╔╝                          
             ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═══╝ ╚═════╝                           
                                                                                    
            This will modify a value in the Windows Registry. 
            Although this will be faily safe, please do backup your registry base first !                                                                                                                                     
            ";
            Console.WriteLine(attention);
        }

        private static int GetProfiles()
        {
            Console.WriteLine();
            Console.WriteLine("Searching for profiles");
            const string sourceDirectory = "C:\\Program Files (x86)\\Creative\\Sound Blaster Cinema\\Sound Blaster Cinema\\Profile";
            try
            {
                // LINQ query for all files containing in the sourceDirectory directory
                var files = from file in Directory.EnumerateFiles(sourceDirectory, "*.xml", SearchOption.AllDirectories) select file;

                Console.WriteLine("========================================================================================================================");
                SortedDictionary<int, string> profilesDictionary = new SortedDictionary<int, string>();
                foreach (var file in files)
                {
                    string fileName = file.Substring(sourceDirectory.Length + 1);

                    foreach (XElement infoElement in XElement.Load(file).Elements("info"))
                    {
                        foreach (XElement profileId in infoElement.Elements("profile_id"))
                        {
                            profilesDictionary.Add(int.Parse(profileId.Value), fileName);
                        }
                    }
                }
                foreach (KeyValuePair<int, string> profile in profilesDictionary.OrderBy(key => key.Key))
                {

                    Console.WriteLine("Choice : {0}, file : {1}", profile.Key, profile.Value);

                }
                Console.WriteLine("========================================================================================================================");
                Console.WriteLine("{0} profiles found in the " + sourceDirectory + " folder.", files.Count<string>().ToString());

                return profilesDictionary.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static int GetProfileChoice(int i)
        {
            int profileChoice = 0;
            string userEntry;
            do
            {
                Console.WriteLine("Enter the number for the wanted profile (1-" + i + ")");
                userEntry = Console.ReadLine();
            }
            while (!int.TryParse(userEntry, out profileChoice));

            //We remove one because the index start at 0 in the registry profileIndex but start at 1 in the xml file
            return profileChoice -1;
        }

        private static void TerminateProcess()
        {
            Console.WriteLine("Terminating Sound Blaster Cinema process");

            try
            {
                foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension("SBCinema")))
                {
                    Console.WriteLine("Process name : " + process.ProcessName);
                    process.Kill();
                    Console.WriteLine(!process.HasExited ? "Process not terminated" : "Process terminated");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void SetRegistryValue(int profileChoice, int i)
        {
            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Creative Tech\Sound Blaster Cinema", true);
                if (registryKey == null || profileChoice < 0 | profileChoice > i)
                {
                    Console.WriteLine("Key is null or was not found");
                    return;
                }
                registryKey.SetValue("ProfileIndex", profileChoice.ToString());
                registryKey.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private static void LaunchProcess()
        {
            try
            {
                System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Creative\\Sound Blaster Cinema\\Sound Blaster Cinema\\SBCinema.exe");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
