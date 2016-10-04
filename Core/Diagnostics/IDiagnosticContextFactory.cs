namespace MetaValidator.Diagnostics {
    using System;
    using System.Reflection;

    public interface IDiagnosticContextFactory {
        IDiagnosticContext Create(Assembly assembly);
        IDiagnosticContext Create(Type type);
        IDiagnosticContext Create(MemberInfo member);
    }
}