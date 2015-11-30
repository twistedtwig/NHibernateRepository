using System;

namespace DatabaseManagement.Logging
{
    public abstract class LoggerBase
    {
        public static bool IsDebugging = false;

        public static void Log(string message, bool isDebugMessage = false)
        {
            if (!isDebugMessage || IsDebugging)
            {
                Console.WriteLine(message);
            }
        }

        public static void Log(Exception ex, bool isDebugMessage = false)
        {
            if (!isDebugMessage || IsDebugging)
            {
                var exception = ex;
                while (exception != null)
                {
                    Console.WriteLine(ex.Message);
                    exception = exception.InnerException;
                }
            }
        }
    }
}
