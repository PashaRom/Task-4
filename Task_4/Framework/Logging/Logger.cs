using System;
using NLog;

namespace Test.Framework.Logging
{
    public static class Log
    {
        private static Logger Logger;
        static Log()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public static void Trace (string message)
        {
            Logger.Trace(message);
        }
        public static void Debug(Exception exception,string message)
        {
            Logger.Debug(exception, message);
        }
        public static void Info(string message)
        {
            Logger.Info(message);
        }
        public static void Error(Exception exception, string message)
        {
            Logger.Error(exception, message);
        }
        public static void Error(string message)
        {
            Logger.Error(message);
        }
        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }
        public static void Fatal (Exception exception, string message)
        {
            Logger.Fatal(exception, message);
        }
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }
    }
}
