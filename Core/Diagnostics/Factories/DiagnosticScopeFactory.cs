namespace MetaValidator.Diagnostics {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using MetaValidator.Core;
    using BF = System.Reflection.BindingFlags;

    sealed class DefaultDiagnosticScopeFactory : IDiagnosticScopeFactory {
        readonly IConfiguration configuration;
        public DefaultDiagnosticScopeFactory(IConfiguration configuration) {
            this.configuration = configuration ?? Configuration.Default;
        }
        IDiagnosticScope IDiagnosticScopeFactory.Create(Assembly assembly, DiagnosticScopeSettings settings) {
            return new AssemblyDiagnosticScope(assembly, configuration, settings);
        }
        IDiagnosticScope IDiagnosticScopeFactory.Create(Type type, DiagnosticScopeSettings settings) {
            return new TypeDiagnosticScope(type, configuration, settings);
        }
        //
        abstract class DiagnosticScope : IDiagnosticScope {
            readonly IConfiguration configuration;
            readonly DiagnosticScopeSettings settings;
            protected DiagnosticScope(IConfiguration configuration, DiagnosticScopeSettings settings) {
                this.configuration = configuration;
                this.settings = settings;
            }
            static bool HasFlag(DiagnosticScopeSettings settings, DiagnosticScopeSettings flag) {
                return (settings & flag) == flag;
            }
            static bool IgnoreOrInclude(DiagnosticScopeSettings settings, DiagnosticScopeSettings flag) {
                return HasFlag(settings, flag) &&
                        !HasFlag(settings, DiagnosticScopeSettings.Ignore);
            }
            protected bool Ignore(Assembly asm) {
                var actualSettings = settings;
                configuration.Resolve<IDiagnosticScopeSettingsResolver>()
                    .@Do(resolver =>
                        actualSettings = resolver.Resolve(asm));
                return HasFlag(actualSettings, DiagnosticScopeSettings.Ignore);
            }
            protected bool Ignore(Type type) {
                var actualSettings = settings;
                configuration.Resolve<IDiagnosticScopeSettingsResolver>()
                    .@Do(resolver =>
                        actualSettings = resolver.Resolve(type));
                return HasFlag(actualSettings, DiagnosticScopeSettings.Ignore);
            }
            protected bool Ignore(MemberInfo member) {
                var actualSettings = settings;
                configuration.Resolve<IDiagnosticScopeSettingsResolver>()
                    .@Do(resolver =>
                        actualSettings = resolver.Resolve(member));
                return HasFlag(actualSettings, DiagnosticScopeSettings.Ignore);
            }
            protected bool IncludeAllTypes(Assembly asm) {
                var actualSettings = settings;
                configuration.Resolve<IDiagnosticScopeSettingsResolver>()
                    .@Do(resolver =>
                        actualSettings = resolver.Resolve(asm));
                return IgnoreOrInclude(actualSettings, DiagnosticScopeSettings.IncludeAllTypes);
            }
            protected bool IncludeAllMembers(Type type) {
                var actualSettings = settings;
                configuration.Resolve<IDiagnosticScopeSettingsResolver>()
                    .@Do(resolver =>
                        actualSettings = resolver.Resolve(type));
                return IgnoreOrInclude(actualSettings, DiagnosticScopeSettings.IncludeAllMembers);
            }
            IEnumerator<IDiagnosticContext> IEnumerable<IDiagnosticContext>.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator<IDiagnosticContext> GetEnumeratorCore() {
                return GetEnumeratorCore(configuration.Resolve<IDiagnosticContextFactory>());
            }
            protected abstract IEnumerator<IDiagnosticContext> GetEnumeratorCore(IDiagnosticContextFactory factory);
        }
        sealed class AssemblyDiagnosticScope : DiagnosticScope {
            readonly Assembly assembly;
            internal AssemblyDiagnosticScope(Assembly assembly, IConfiguration configuration, DiagnosticScopeSettings settings)
                : base(configuration, settings) {
                this.assembly = assembly;
            }
            protected sealed override IEnumerator<IDiagnosticContext> GetEnumeratorCore(IDiagnosticContextFactory factory) {
                if(assembly == null || Ignore(assembly))
                    yield break;
                yield return factory.Create(assembly);
                var assemblyTypes = new AssemblyTypes(assembly, IncludeAllTypes(assembly));
                foreach(var type in assemblyTypes) {
                    if(type == null || Ignore(type))
                        continue;
                    yield return factory.Create(type);
                    var typeMembers = new TypeMembers(type, IncludeAllMembers(type));
                    foreach(var member in typeMembers) {
                        if(member == null || Ignore(member))
                            continue;
                        yield return factory.Create(member);
                    }
                }
            }
        }
        sealed class TypeDiagnosticScope : DiagnosticScope {
            readonly Type type;
            internal TypeDiagnosticScope(Type type, IConfiguration configuration, DiagnosticScopeSettings settings)
                : base(configuration, settings) {
                this.type = type;
            }
            protected sealed override IEnumerator<IDiagnosticContext> GetEnumeratorCore(IDiagnosticContextFactory factory) {
                if(type == null || Ignore(type))
                    yield break;
                yield return factory.Create(type);
                var typeMembers = new TypeMembers(type, IncludeAllMembers(type));
                foreach(var member in typeMembers) {
                    if(type == null || Ignore(member))
                        continue;
                    yield return factory.Create(member);
                }
            }
        }
        sealed class AssemblyTypes : IEnumerable<Type> {
            readonly Type[] types;
            public AssemblyTypes(Assembly assembly, bool allTypes = false) {
                types = assembly
                    .@Get(asm => GetTypes(asm, allTypes), Type.EmptyTypes);
            }
            static Type[] GetTypes(Assembly asm, bool allTypes) {
                try {
                    return allTypes || asm.IsDynamic ?
                        asm.GetTypes() :
                        asm.GetExportedTypes();
                }
                catch(ReflectionTypeLoadException e) { return e.Types; }
            }
            IEnumerator<Type> IEnumerable<Type>.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator<Type> GetEnumeratorCore() {
                for(int i = 0; i < types.Length; i++)
                    yield return types[i];
            }
        }
        sealed class TypeMembers : IEnumerable<MemberInfo> {
            readonly static MemberInfo[] EmptyMembers = new MemberInfo[] { };
            readonly MemberInfo[] members;
            public TypeMembers(Type type, bool allMembers = false) {
                members = type
                    .@Get(t => GetMembers(t, allMembers), EmptyMembers);
            }
            static MemberInfo[] GetMembers(Type type, bool allMembers) {
                try {
                    return allMembers ?
                        type.GetMembers(BF.Public | BF.NonPublic | BF.Static | BF.Instance) :
                        type.GetMembers(BF.Public | BF.Static | BF.Instance);
                }
                catch { return EmptyMembers; }
            }
            IEnumerator<MemberInfo> IEnumerable<MemberInfo>.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumeratorCore();
            }
            IEnumerator<MemberInfo> GetEnumeratorCore() {
                for(int i = 0; i < members.Length; i++)
                    yield return members[i];
            }
        }
    }
}