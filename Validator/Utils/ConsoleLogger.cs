namespace MetaValidator {
    using System;

    sealed class ConsoleLogger : ILogger {
        internal static readonly ILogger Default = new ConsoleLogger();
        ConsoleLogger() { }
        //
        public void Log(string message) {
            Console.WriteLine(message);
        }
    }
}