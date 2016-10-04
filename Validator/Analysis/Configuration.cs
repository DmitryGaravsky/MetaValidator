namespace MetaValidator {
    using System;
    using System.Reflection;

    class Configuration : IConfiguration {
        readonly Lazy<ILogger> loggerCore;
        readonly Lazy<IAssemblyLoader> loaderCore;
        public Configuration() {
            loggerCore = new Lazy<ILogger>(CreateLogger);
            loaderCore = new Lazy<IAssemblyLoader>(CreateAssemblyLoader);
        }
        //
        protected virtual ILogger CreateLogger() {
            return ConsoleLogger.Default;
        }
        protected virtual IAssemblyLoader CreateAssemblyLoader() {
            return new AssemblyLoader(asm => {
                var cfg = MetaValidator.Core.Configuration.Default;

                var scope = cfg.Resolve<Diagnostics.IDiagnosticScopeFactory>()
                        .Create(asm);
                foreach(var context in scope) {
                    foreach(var item in cfg.Resolve(context)) {
                        if(!item.Validate(context)) { 
                            if(context.Result.HasErrors){
                                foreach(var error in context.Result.Errors) 
                                    Logger.Log(error);
                            }
                        }
                    }
                }
            }, Logger);
        }
        //
        public ILogger Logger {
            get { return loggerCore.Value; }
        }
        public IAssemblyLoader AssemblyLoader {
            get { return loaderCore.Value; }
        }
    }
}