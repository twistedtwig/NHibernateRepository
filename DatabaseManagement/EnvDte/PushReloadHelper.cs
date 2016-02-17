using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DatabaseManagement.EnvDte
{
    internal class PushReloadHelper
    {
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        //      static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal static void Listen()
        {
            int counter = 0;

            Console.Title = "Waiting...";
            Console.WriteLine("Waiting...");
            IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, "File Modification Detected");
            while ((int)hwnd == 0 && counter < 5)
            {
                counter++;
                Thread.Sleep(500);
                hwnd = FindWindowByCaption(IntPtr.Zero, "File Modification Detected");
            }
            if ((int)hwnd != 0)
            {
                // ShowNormal = 1
                // Show = 5
                ShowWindow(hwnd, 5);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
                hwnd = IntPtr.Zero;    
            }
            else
            {
                Console.WriteLine("didnt find file modifaction dialog");                
            }

        }
    }
}
