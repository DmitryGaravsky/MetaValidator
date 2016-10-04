namespace MetaValidator {
    using CommandLine;

    [Verb("asm", HelpText = "Check assembly specified by specific path or assembly name.")]
    sealed class FileOptions {
        readonly string path;
        readonly string assemblyName;
        public FileOptions(string path, string assemblyName) {
            this.path = path;
            this.assemblyName = assemblyName;
        }
        [Option('p', "path", HelpText = "Path to assembly to be processed.")]
        public string Path {
            get { return path; }
        }
        [Option('n', "name", HelpText = "AssemblyName of assembly to be processed.")]
        public string AssemblyName {
            get { return assemblyName; }
        }
    }
}