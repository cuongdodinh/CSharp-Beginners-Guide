using NLog;
using System;

namespace nlog_01
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Trace("trace message");
            _logger.Debug("debug message");
            _logger.Info("info message");
            _logger.Warn("warn message");
            _logger.Error("error message");
            _logger.Fatal("fatal message");
        }
    }
}
