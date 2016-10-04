namespace MetaValidator.Diagnostics {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MetaValidator.Core;

    sealed partial class DefaultDiagnosticContextFactory {
        sealed class DiagnosticContext : IDiagnosticContext {
            internal DiagnosticContext(Assembly assembly, Type type, MemberInfo member) {
                this.Assembly = assembly;
                this.Type = type;
                this.Member = member;
            }
            public Assembly Assembly {
                get;
                private set;
            }
            public Type Type {
                get;
                private set;
            }
            public MemberInfo Member {
                get;
                private set;
            }
            readonly IDiagnosticResult result = new DiagnosticResult();
            IDiagnosticResult IDiagnosticContext.Result {
                get { return result; }
            }
            #region DiagnosticResult
            sealed class DiagnosticResult : IDiagnosticResult {
                public bool HasErrors {
                    get {
                        return
                            assemblyErrors.@Get(x => true) ||
                            typeErrors.@Get(x => true) ||
                            memberErrors.@Get(x => true);
                    }
                }
                public IEnumerable<string> Errors {
                    get {
                        if(assemblyErrors != null) {
                            foreach(var item in assemblyErrors) {
                                foreach(var error in item.Value)
                                    yield return error.Value;
                            }
                        }
                        if(typeErrors != null) {
                            foreach(var item in typeErrors) {
                                foreach(var error in item.Value)
                                    yield return error.Value;
                            }
                        }
                        if(memberErrors != null) {
                            foreach(var item in memberErrors) {
                                foreach(var error in item.Value)
                                    yield return error.Value;
                            }
                        }
                    }
                }
                //
                IDictionary<Assembly, IList<Lazy<string>>> assemblyErrors;
                void IDiagnosticResult.AddAssemblyError(Assembly assembly, string errorFormat) {
                    AddError(ref assemblyErrors, assembly, x => string.Format(errorFormat, x.FullName));
                }
                IDictionary<Type, IList<Lazy<string>>> typeErrors;
                void IDiagnosticResult.AddTypeError(Type type, string errorFormat) {
                    AddError(ref typeErrors, type, x => string.Format(errorFormat, x.FullName));
                }
                IDictionary<MemberInfo, IList<Lazy<string>>> memberErrors;
                void IDiagnosticResult.AddMemberError(MemberInfo member, string errorFormat) {
                    AddError(ref memberErrors, member, x => string.Format(errorFormat, x.Name));
                }
                static void AddError<T>(ref IDictionary<T, IList<Lazy<string>>> refErrorsCache, T value, Func<T, string> getString) {
                    if(refErrorsCache == null)
                        refErrorsCache = new Dictionary<T, IList<Lazy<string>>>();
                    IList<Lazy<string>> errors;
                    if(!refErrorsCache.TryGetValue(value, out errors)) {
                        errors = new List<Lazy<string>>();
                        refErrorsCache.Add(value, errors);
                    }
                    errors.Add(new Lazy<string>(() => getString(value)));
                }
            }
            #endregion
        }
    }
}