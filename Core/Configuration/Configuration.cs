namespace MetaValidator.Core {
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class Configuration : IConfiguration {
        public readonly static IConfiguration Default = new Configuration();
        Configuration() {
            factories = new Dictionary<Type, Lazy<object>> { 
                { typeof(Diagnostics.IDiagnosticContextFactory), new Lazy<object>(() => CreateDiagnosticContextFactory()) },
                { typeof(Diagnostics.IDiagnosticScopeFactory), new Lazy<object>(() => CreateDiagnosticScopeFactory()) },
                { typeof(Diagnostics.IDiagnosticScopeSettingsResolver), new Lazy<object>(() => CreateDiagnosticScopeSettingsResolver()) },
            };
            diagnostics = new Dictionary<Type, Lazy<Diagnostics.IDiagnostic>>(8);
            var memberResolutions = new List<Func<Diagnostics.IDiagnosticContext, Type>>(16);
            resolutionTree = new Dictionary<Type, List<Func<Diagnostics.IDiagnosticContext, Type>>> { 
                { typeof(Assembly), new List<Func<Diagnostics.IDiagnosticContext, Type>>(8) },
                { typeof(Type), new List<Func<Diagnostics.IDiagnosticContext, Type>>(8) },
                { typeof(MemberInfo), memberResolutions },
                { typeof(FieldInfo), memberResolutions },
                { typeof(PropertyInfo), memberResolutions },
                { typeof(EventInfo), memberResolutions },
                { typeof(ConstructorInfo), memberResolutions },
            };
        }
        //
        #region Resolve
        IEnumerable<Diagnostics.IDiagnostic> IConfiguration.Resolve(Diagnostics.IDiagnosticContext context) {
            var result = new List<Diagnostics.IDiagnostic>(4);
            context.@Do(ctx =>
            {
                ctx.Assembly.@Do(_ =>
                    ResolveCore<Assembly>(ctx, result));
                ctx.Type.@Do(_ =>
                    ResolveCore<Type>(ctx, result));
                ctx.Member.@Do(_ =>
                    ResolveCore<MemberInfo>(ctx, result));
            });
            return result;
        }
        void ResolveCore<T>(Diagnostics.IDiagnosticContext context, ICollection<Diagnostics.IDiagnostic> result) {
            ResolveCore(typeof(T), context, result);
        }
        void ResolveCore(Type key, Diagnostics.IDiagnosticContext context, ICollection<Diagnostics.IDiagnostic> result) {
            resolutionTree[key].@Do(resolutions =>
            {
                foreach(var resolveFunc in resolutions) {
                    resolveFunc(context).@Do(
                        diagnosticType =>
                            ResolveCore(diagnosticType)
                                .@Do(result.Add)
                    );
                }
            });
        }
        #endregion Resolve
        #region Registration
        readonly IDictionary<Type, Lazy<Diagnostics.IDiagnostic>> diagnostics;
        void IConfiguration.Register<TDiagnostic>(System.Func<TDiagnostic> create) {
            Lazy<Diagnostics.IDiagnostic> value;
            if(!diagnostics.TryGetValue(typeof(TDiagnostic), out value)) {
                value = new Lazy<Diagnostics.IDiagnostic>(() => create());
                diagnostics.Add(typeof(TDiagnostic), value);
            }
            else diagnostics[typeof(TDiagnostic)] = new Lazy<Diagnostics.IDiagnostic>(() => create());
        }
        Diagnostics.IDiagnostic ResolveCore(Type diagnosticType) {
            Lazy<Diagnostics.IDiagnostic> value;
            return diagnostics.TryGetValue(diagnosticType, out value) ? value.Value : null;
        }
        readonly IDictionary<Type, List<Func<Diagnostics.IDiagnosticContext, Type>>> resolutionTree;
        public void Register<T, TDiagnostic>(ISpecification<T> constraint)
            where TDiagnostic : Diagnostics.IDiagnostic {
            List<Func<Diagnostics.IDiagnosticContext, Type>> resolutions;
            if(resolutionTree.TryGetValue(typeof(T), out resolutions)) {
                if(typeof(Assembly).IsAssignableFrom(typeof(T))) {
                    resolutions.Add(ctx => ((ISpecification<Assembly>)constraint).Match(ctx.Assembly) ? typeof(TDiagnostic) : null);
                    return;
                }
                if(typeof(Type).IsAssignableFrom(typeof(T))) {
                    resolutions.Add(ctx => ((ISpecification<Type>)constraint).Match(ctx.Type) ? typeof(TDiagnostic) : null);
                    return;
                }
                if(typeof(MemberInfo).IsAssignableFrom(typeof(T))) {
                    resolutions.Add(ctx => ((ISpecification<T>)constraint).Match((T)(object)ctx.Member) ? typeof(TDiagnostic) : null);
                    return;
                }
            }
        }
        void IConfiguration.Reset() {
            foreach(var item in resolutionTree)
                item.Value.Clear();
            diagnostics.Clear();
        }
        #endregion Registration
        #region Resolve<TFactory>
        readonly IDictionary<Type, Lazy<object>> factories;
        public TFactory Resolve<TFactory>()
            where TFactory : class {
            Lazy<object> factory;
            return factories.TryGetValue(typeof(TFactory), out factory) ?
                factory.@Get(f => f.Value) as TFactory : null;
        }
        protected virtual Diagnostics.IDiagnosticContextFactory CreateDiagnosticContextFactory() {
            return Diagnostics.DefaultDiagnosticContextFactory.Instance;
        }
        protected virtual Diagnostics.IDiagnosticScopeSettingsResolver CreateDiagnosticScopeSettingsResolver() {
            return Diagnostics.DefaultDiagnosticScopeSettingsResolver.Instance;
        }
        protected virtual Diagnostics.IDiagnosticScopeFactory CreateDiagnosticScopeFactory() {
            return new Diagnostics.DefaultDiagnosticScopeFactory(this);
        }
        #endregion
    }
}