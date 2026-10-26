using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TroopKDR
{
    /// <summary>
    /// The mod's own diagnostics, in Configs\ModLogs\TroopKDR.log. Errors from the same place are logged a few times only, so a
    /// broken hook can't flood the file.
    /// </summary>
    internal static class Log
    {
        private const long MaxBytes = 1024 * 1024;
        private const int MaxRepeats = 5;

        private static readonly object Gate = new object();
        private static readonly Dictionary<string, int> ErrorCounts = new Dictionary<string, int>();
        private static string _path;

        public static void Info(string message) => Write("INFO", message);

        /// <summary>Logs an error from <paramref name="where"/>; after a few repeats from the same place it goes quiet.</summary>
        public static void Error(string where, Exception e)
        {
            int count;
            lock (Gate)
            {
                ErrorCounts.TryGetValue(where, out count);
                ErrorCounts[where] = ++count;
            }

            if (count <= MaxRepeats)
            {
                Write("ERROR", $"{where}: {e}" + (count == MaxRepeats ? " (further errors from here are not logged)" : ""));
            }
        }

        private static void Write(string level, string message)
        {
            try
            {
                lock (Gate)
                {
                    if (_path == null)
                    {
                        string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string folder = Path.Combine(docs, "Mount and Blade II Bannerlord", "Configs", "ModLogs");
                        Directory.CreateDirectory(folder);
                        _path = Path.Combine(folder, "TroopKDR.log");
                        if (File.Exists(_path) && new FileInfo(_path).Length > MaxBytes)
                        {
                            File.Delete(_path);
                        }
                    }

                    File.AppendAllText(_path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}", Encoding.UTF8);
                }
            }
            catch
            {
                // Logging must never take the game down.
            }
        }
    }
}
