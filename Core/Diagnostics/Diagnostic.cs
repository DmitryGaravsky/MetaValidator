namespace MetaValidator.Diagnostics {
    using System;
    using System.Reflection;
    using MetaValidator.Core;

    public class Diagnostic : IDiagnostic {
        readonly Func<ISpecification<Assembly>> getAssemblyErrorSpec;
        public Diagnostic(ISpecification<Assembly> errorSpec) {
            getAssemblyErrorSpec = errorSpec.@Get(x => () => x, getAssemblyErrorSpec);
        }
        readonly Func<ISpecification<Type>> getTypeErrorSpec;
        public Diagnostic(ISpecification<Type> errorSpec) {
            getTypeErrorSpec = errorSpec.@Get(x => () => x, getTypeErrorSpec);
        }
        readonly Func<ISpecification<MemberInfo>> getMemberErrorSpec;
        public Diagnostic(ISpecification<MemberInfo> errorSpec) {
            getMemberErrorSpec = errorSpec.@Get(x => () => x, getMemberErrorSpec);
        }
        //
        bool IDiagnostic.Validate(IDiagnosticContext context) {
            var result = context.Result;
            context.Assembly.@Do(assembly =>
                Validate(GetAssemblyError, assembly, x => result.AddAssemblyError(x, string.Empty)));
            context.Type.@Do(type =>
                Validate(GetTypeError, type, x => result.AddTypeError(x, string.Empty)));
            context.Member.@Do(member =>
                Validate(GetMemberError, member, x => result.AddMemberError(x, string.Empty)));
            return !result.HasErrors;
        }
        void Validate<T>(Func<ISpecification<T>> getValidationSpec, T value, Action<T> onError) {
            if(getValidationSpec().@Get(spec => spec.Match(value)))
                onError(value);
        }
        //
        protected virtual ISpecification<Assembly> GetAssemblyError() {
            return getAssemblyErrorSpec.@Get(get => get());
        }
        protected virtual ISpecification<Type> GetTypeError() {
            return getTypeErrorSpec.@Get(get => get());
        }
        protected virtual ISpecification<MemberInfo> GetMemberError() {
            return getMemberErrorSpec.@Get(get => get());
        }
    }
}