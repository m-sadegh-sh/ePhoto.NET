using System;
using System.IO;
using System.Web.Hosting;

namespace ePhoto.NET.Helpers {
    public static class Logger {
        private const string LOG_FILE_NAME = "Logs.{0}.log";

        public static void Write(string message) {
            DoWrite(message);
        }

        public static void Write(Exception exception) {
            do {
                DoWrite($"{exception.Message}\t{exception.StackTrace}\t{exception.Source}");
            } while ((exception = exception.InnerException) != null);
        }

        private static void DoWrite(string message) {
            File.AppendAllText(GetLogFilePath(), $"{DateTime.UtcNow}\t{message}");
        }

        private static string GetLogFilePath() {
            var logFileName = string.Format(LOG_FILE_NAME, DateTime.UtcNow.ToString("yyyy-MM-dd-HH"));

            var fullLogFilePath = HostingEnvironment.MapPath("~/Logs/" + logFileName);

            var logFileDirectory = Path.GetDirectoryName(fullLogFilePath);
            if (logFileDirectory != null && !Directory.Exists(logFileDirectory))
                Directory.CreateDirectory(logFileDirectory);

            return fullLogFilePath;
        }
    }
}