namespace MetaValidator.Constraints {
    using System.Reflection;

    public class ReadOnlyProperty : MetaConstraint<PropertyInfo> {
        public readonly static IMetaConstraint<PropertyInfo> Instance = new ReadOnlyProperty();
        protected ReadOnlyProperty() { }
        //
        protected override bool MatchCore(PropertyInfo property) {
            return property.GetGetMethod(true) != null && property.GetSetMethod(true) == null;
        }
    }
}