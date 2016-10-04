namespace MetaValidator.Diagnostics {
    using System;
    using System.Reflection;
    using MetaValidator.Core;

    sealed partial class DefaultDiagnosticContextFactory : IDiagnosticContextFactory {
        internal readonly static IDiagnosticContextFactory Instance = new DefaultDiagnosticContextFactory();
        DefaultDiagnosticContextFactory() { }
        //
        IDiagnosticContext IDiagnosticContextFactory.Create(Assembly assembly) {
            return new DiagnosticContext(assembly, null, null);
        }
        IDiagnosticContext IDiagnosticContextFactory.Create(Type type) {
            var assembly = type.@Get(x => x.Assembly);
            return new DiagnosticContext(assembly, type, null);
        }
        IDiagnosticContext IDiagnosticContextFactory.Create(MemberInfo member) {
            var type = member.@Get(x => x.DeclaringType);
            var assembly = type.@Get(x => x.Assembly);
            return new DiagnosticContext(assembly, type, member);
        }
    }
}