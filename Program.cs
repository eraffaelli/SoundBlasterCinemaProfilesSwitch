using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

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
            Although this will be faily safe, please do backup your registry first !                                                                                                                                     
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
                int i = -1;
                foreach (var file in files)
                {
                    i++;
                    string fileName = file.Substring(sourceDirectory.Length + 1);
                    Console.WriteLine(i + " {0}", fileName);
                }
                Console.WriteLine("========================================================================================================================");
                Console.WriteLine("{0} profiles found in the " + sourceDirectory + " folder.", files.Count<string>().ToString());

                return i;
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
                Console.WriteLine("Enter the number for the wanted profile (0-" + i + ")");
                userEntry = Console.ReadLine();
            }
            while (!int.TryParse(userEntry, out profileChoice));

            return profileChoice;
        }

        private static void TerminateProcess()
        {
            Console.WriteLine("Terminating Sound Blaster Cinema process");

            try
            {
                foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension("SBCinema")))
                {
                    Console.WriteLine(process.ProcessName);
                    process.Kill();
                    if (!process.HasExited)
                    {
                        Console.WriteLine("Process not terminated");
                    }
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
