namespace MetaValidator {
    interface IConfiguration {
        IAssemblyLoader AssemblyLoader { get; }
        ILogger Logger { get; }
    }
}