namespace MetaValidator.Core {
    using System;
    using System.Collections.Generic;
    using MetaValidator.Diagnostics;

    public interface IConfiguration {
        TFactory Resolve<TFactory>() 
            where TFactory : class;
        IEnumerable<IDiagnostic> Resolve(IDiagnosticContext context);
        //
        void Register<TDiagnostic>(Func<TDiagnostic> create)
            where TDiagnostic : IDiagnostic;
        void Register<T, TDiagnostic>(ISpecification<T> constraint)
            where TDiagnostic : IDiagnostic;
        void Reset();
    }
    //
    public interface IConfigurator {
        void Configure(IConfiguration configuration);
    }
}