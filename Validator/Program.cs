namespace MetaValidator {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using CommandLine;

    class Program {
        static readonly IConfiguration configuration = new Configuration();
        //
        static int Main(string[] args) {
            args = new string[] { "list", "--d", @"d:\Code\v16.1\Bin\Framework4\", "--p", "DevExpress.*.dll" };
            //args = new string[] { "asm", "--n", @"DevExpress.Data.v16.1" };
            if(args == null || args.Length == 0)
                args = new string[] { "list", "--d", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) };
            else if(args.Length == 1)
                args = new string[] { "asm", "--p", args[0] };
            //
            var result = CommandLine.Parser.Default.ParseArguments<FileOptions, FilesOptions>(args);
            try {
                return result.MapResult(
                        (FileOptions options) => OnFile(options),
                        (FilesOptions options) => OnFiles(options),
                        (IEnumerable<Error> errors) => OnErrors(errors))
                    .Result;
            }
            catch(AggregateException e) {
                configuration.Logger.Log(e.Message);
                foreach(var item in e.InnerExceptions)
                    configuration.Logger.Log(item.Message);
                return 1;
            }
        }
        //
        static Task<int> OnFile(FileOptions options) {
            return Task.Factory.StartNew(() =>
            {
                var loader = configuration.AssemblyLoader;
                if(!string.IsNullOrEmpty(options.Path))
                    loader.ProcessPath(options.Path);
                if(!string.IsNullOrEmpty(options.AssemblyName))
                    loader.ProcessAssembly(options.AssemblyName);
                return 0;
            });
        }
        static Task<int> OnFiles(FilesOptions options) {
            return Task.Factory.StartNew(() =>
            {
                var logger = configuration.Logger;
                var files = FileOptionsHelper.GetFiles(options, logger);
                var partitioner = Partitioner.Create(files, true);
                var loader = configuration.AssemblyLoader;
                return Parallel.ForEach(partitioner,
                        (path) => loader.ProcessPath(path)
                    ).IsCompleted ? 0 : 1;
            });
        }
        static Task<int> OnErrors(IEnumerable<Error> errors) {
            return Task.Factory.StartNew(() =>
            {
                var logger = configuration.Logger;
                foreach(var error in errors)
                    logger.Log(error.ToString());
                return 1;
            });
        }
    }
}