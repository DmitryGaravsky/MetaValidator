namespace MetaValidator {
    interface IAssemblyLoader {
        void ProcessAssembly(string assemblyName);
        void ProcessPath(string path);
    }
}