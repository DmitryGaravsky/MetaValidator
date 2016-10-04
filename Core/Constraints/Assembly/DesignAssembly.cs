namespace MetaValidator.Constraints {
    using System.Reflection;
    
    public class DesignAssembly : AssemblyConstraint {
        public readonly static IMetaConstraint<Assembly> Instance = new DesignAssembly();
        protected DesignAssembly() { }
        //
        protected override bool MatchCore(Assembly assembly) {
            var name = assembly.GetName();
            return (name != null) && name.Name.EndsWith(".Design");
        }
    }
}