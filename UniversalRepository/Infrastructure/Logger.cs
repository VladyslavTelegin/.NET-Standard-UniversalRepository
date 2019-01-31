namespace UniversalRepository.Infrastructure
{
    using System;
    using System.IO;
    using System.Reflection;

    public sealed class Logger
    {
        #region Constants

        private const string FileName = "UniversalRepository.log";

        #endregion

        #region PrivateFields

        private static readonly object _syncRoot = new object();

        #endregion

        #region Singleton Impl.

        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

        public static Logger Instance => _instance.Value;

        private Logger() { }

        #endregion

        #region Properties

        public string FileFullQualifiedPath => Path.Combine(this.AssemblyDirectory, FileName);

        public string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        #endregion

        public void CreateLog(string errorMessage)
        {
            lock (_syncRoot)
            {
                var pathToWrite = this.FileFullQualifiedPath;

                try
                {
                    File.AppendAllText(pathToWrite, $"{errorMessage}{Environment.NewLine}{Environment.NewLine}");
                }
                catch (Exception)
                {
                    /* Ignored. */
                }
            }
        }
    }
}