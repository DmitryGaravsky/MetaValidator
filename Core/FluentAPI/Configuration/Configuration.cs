namespace MetaValidator.Diagnostics.FluentAPI {
    using System;
    using MetaValidator.Constraints;
    using MetaValidator.Core;
    using MetaValidator.Diagnostics;

    public static class ConfigurationExtension {
        public static void Register<T, TDiagnostic>(this IConfiguration configuration, ISpecification<T> constraint, TDiagnostic instance)
            where TDiagnostic : IDiagnostic {
            configuration.@Do(cfg =>
            {
                cfg.Register<TDiagnostic>(() => instance);
                cfg.Register<T, TDiagnostic>(constraint);
            });
        }
        //
        public static FluentConfiguration<T> For<T>(this IConfiguration configuration,
            Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory = null) {
            return new FluentConfiguration<T>(configuration, factory, null);
        }
        public class FluentConfigurationBase<T> {
            protected readonly IConfiguration configuration;
            protected readonly Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory;
            readonly Func<ISpecification<T>, ISpecification<T>> modifier;
            protected internal FluentConfigurationBase(IConfiguration configuration,
                Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory,
                Func<ISpecification<T>, ISpecification<T>> modifier = null) {
                this.configuration = configuration;
                this.factory = factory;
                this.modifier = modifier;
            }
            protected void RegisterCore<TDiagnostic>(TDiagnostic diagnostic)
                where TDiagnostic : IDiagnostic {
                configuration.@Do(cfg =>
                {
                    cfg.Register(() => diagnostic);
                    factory.@Do(f =>
                    {
                        var spec = f(null).Unwrap();
                        modifier.@Do(m =>
                                spec = m(spec));
                        cfg.Register<T, TDiagnostic>(spec);
                    });
                });
            }
        }
        public class FluentConfiguration<T> : FluentConfigurationBase<T> {
            readonly static Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>>
                DefaultFactory = x => MetaConstraint<T>.True.Wrap();
            protected internal FluentConfiguration(IConfiguration configuration,
                Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory,
                Func<ISpecification<T>, ISpecification<T>> modifier = null)
                : base(configuration, factory ?? DefaultFactory, modifier) {
            }
            public FluentConfiguration<T> Register<TDiagnostic>()
                where TDiagnostic : IDiagnostic, new() {
                RegisterCore<TDiagnostic>(new TDiagnostic());
                return this;
            }
            public FluentConfiguration<T> Register<TDiagnostic>(TDiagnostic diagnostic)
                where TDiagnostic : IDiagnostic {
                RegisterCore<TDiagnostic>(diagnostic);
                return this;
            }
            public FluentConfigurationIf<T> If(Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> @ifFactory) {
                if(factory == null || factory == DefaultFactory)
                    return new FluentConfigurationIf<T>(configuration, @ifFactory);
                else {
                    var ifSpec = @ifFactory.@Get(@if =>
                            @if(null).Unwrap());
                    return new FluentConfigurationIf<T>(configuration, factory, s => s.And(ifSpec));
                }
            }
        }
        #region If/Else
        public class FluentConfigurationIf<T> : FluentConfigurationBase<T> {
            internal FluentConfigurationIf(IConfiguration configuration,
                Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory,
                Func<ISpecification<T>, ISpecification<T>> modifier = null)
                : base(configuration, factory, modifier) {
            }
            public FluentConfigurationIf<T> Register<TDiagnostic>()
                where TDiagnostic : IDiagnostic, new() {
                RegisterCore<TDiagnostic>(new TDiagnostic());
                return this;
            }
            public FluentConfigurationIf<T> Register<TDiagnostic>(TDiagnostic diagnostic)
                where TDiagnostic : IDiagnostic {
                base.RegisterCore<TDiagnostic>(diagnostic);
                return this;
            }
            public FluentConfigurationeElse<T> Else() {
                return new FluentConfigurationeElse<T>(configuration, factory, s => s.Not());
            }
        }
        public class FluentConfigurationeElse<T> : FluentConfigurationBase<T> {
            internal FluentConfigurationeElse(IConfiguration configuration,
                Func<IMetaConstraint<T>, MetaValidator.Core.MetaSpecificationExtension.OperatorAware<T>> factory,
                Func<ISpecification<T>, ISpecification<T>> modifier)
                : base(configuration, factory, modifier) {
            }
            public FluentConfigurationeElse<T> Register<TDiagnostic>()
                where TDiagnostic : IDiagnostic, new() {
                base.RegisterCore<TDiagnostic>(new TDiagnostic());
                return this;
            }
            public FluentConfigurationeElse<T> Register<TDiagnostic>(TDiagnostic diagnostic)
                where TDiagnostic : IDiagnostic {
                base.RegisterCore<TDiagnostic>(diagnostic);
                return this;
            }
        }
        #endregion
    }
}