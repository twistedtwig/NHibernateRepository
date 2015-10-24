using System;

namespace DatabaseManagement
{
    public static class Logger
    {
        public static bool IsDebugging = false;

        public static void Log(string message, bool isDebugMessage = false)
        {
            if (!isDebugMessage || IsDebugging)
            {
                Console.WriteLine(message);
            }
        }
    }
}
