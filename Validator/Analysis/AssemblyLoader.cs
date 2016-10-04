namespace MetaValidator {
    using System;
    using System.IO;
    using System.Reflection;

    sealed class AssemblyLoader : IAssemblyLoader {
        readonly ILogger logger;
        readonly Action<Assembly> processor;
        public AssemblyLoader(Action<Assembly> processor, ILogger logger = null) {
            this.logger = logger ?? ConsoleLogger.Default;
            this.processor = processor;
        }
        public void ProcessPath(string path) {
            logger.Log("Start processing (Path):" + Environment.NewLine + path);
            TryLoad(() => Assembly.ReflectionOnlyLoadFrom(path));
        }
        public void ProcessAssembly(string assemblyName) {
            logger.Log("Start processing (Assembly):" + Environment.NewLine + assemblyName);
            TryLoad(() => Assembly.ReflectionOnlyLoad(assemblyName));
        }
        void TryLoad(Func<Assembly> load) {
            try {
                var asm = load();
                if(asm != null) processor(asm);
            }
            catch(FileLoadException) { logger.Log("Can't load Assembly. Skipped."); }
            catch(BadImageFormatException) { logger.Log("Bad Assembly. Skipped."); }
        }
    }
}