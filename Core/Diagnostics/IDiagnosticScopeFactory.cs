namespace MetaValidator.Diagnostics {
    using System;
    using System.Reflection;

    [Flags]
    public enum DiagnosticScopeSettings {
        Default = 0,
        IncludeAllTypes = 0x01,
        IncludeAllMembers = 0x02,
        Ignore = 0x10,
    }
    public interface IDiagnosticScopeSettingsResolver {
        DiagnosticScopeSettings Resolve(Assembly assembly);
        DiagnosticScopeSettings Resolve(Type type);
        DiagnosticScopeSettings Resolve(MemberInfo member);
    }
    public interface IDiagnosticScopeFactory {
        IDiagnosticScope Create(Assembly assembly, DiagnosticScopeSettings settings = DiagnosticScopeSettings.Default);
        IDiagnosticScope Create(Type type, DiagnosticScopeSettings settings = DiagnosticScopeSettings.Default);
    }
}