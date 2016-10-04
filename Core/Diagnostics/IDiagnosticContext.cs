namespace MetaValidator.Diagnostics {
    using System;
    using System.Reflection;

    public interface IDiagnosticContext {
        Assembly Assembly { get; }
        Type Type { get; }
        MemberInfo Member { get; }
        IDiagnosticResult Result { get; }
    }
}