namespace MetaValidator {
    using CommandLine;

    [Verb("list", HelpText = "Check assemblies specified by Path or AssemblyName.")]
    sealed class FilesOptions {
        readonly string directory;
        readonly string pattern;
        readonly bool allDirectories;
        public FilesOptions(string directory, string pattern, bool allDirectories) {
            this.directory = directory;
            this.pattern = pattern;
            this.allDirectories = allDirectories;
        }
        [Option('d', "directory", HelpText = "Path to directory to be processed.")]
        public string Directory {
            get { return directory; }
        }
        [Option('p', "pattern", HelpText = "The pattern string to match against the names of files in path.", Default = "*.dll")]
        public string Pattern {
            get { return pattern; }
        }
        [Option('a', "all", HelpText = "Includes the current directory and all the subdirectories in a search operation.")]
        public bool AllDirectories {
            get { return allDirectories; }
        }
    }
}