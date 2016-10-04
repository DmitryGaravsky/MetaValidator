namespace MetaValidator.Diagnostics {
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class DiagnosticScopeAttribute : Attribute {
        public DiagnosticScopeAttribute(DiagnosticScopeSettings settings) {
            this.Settings = settings;
        }
        public DiagnosticScopeSettings Settings {
            get;
            private set;
        }
    }
}