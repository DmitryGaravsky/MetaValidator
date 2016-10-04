namespace MetaValidator {
    using System;
    using System.IO;

    static class FileOptionsHelper {
        public static string[] GetFiles(FilesOptions options, ILogger logger) {
            if(!string.IsNullOrEmpty(options.Directory)) {
                try {
                    if(string.IsNullOrEmpty(options.Pattern))
                        return Directory.GetFiles(options.Directory);
                    else
                        return Directory.GetFiles(options.Directory, GetSearchPattern(options, logger), GetSearchOptions(options));
                }
                catch(Exception e) { logger.Log("Error when obtaining files:" + e.Message); }
            }
            return new string[] { };
        }
        static SearchOption GetSearchOptions(FilesOptions options) {
            return options.AllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        }
        static string GetSearchPattern(FilesOptions options, ILogger logger) {
            string pattern = options.Pattern;
            if(pattern.IndexOfAny(Path.GetInvalidPathChars()) != -1) {
                logger.Log("Invalid character in pattern:" + options.Pattern + ". Ignored");
                pattern = "*.dll";
            }
            return pattern;
        }
    }
}