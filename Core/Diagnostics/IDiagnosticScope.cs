namespace MetaValidator.Diagnostics {
    using System;
    using System.Collections.Generic;

    public interface IDiagnosticScope :
        IEnumerable<IDiagnosticContext> {
    }
}