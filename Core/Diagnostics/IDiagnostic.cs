namespace MetaValidator.Diagnostics {
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IDiagnostic {
        bool Validate(IDiagnosticContext context);
    }
    //
    public interface IDiagnosticResult {
        bool HasErrors { get; }
        IEnumerable<string> Errors { get; }
        //
        void AddAssemblyError(Assembly type, string errorFormat);
        void AddTypeError(Type type, string errorFormat);
        void AddMemberError(MemberInfo member, string errorFormat);
    }
}