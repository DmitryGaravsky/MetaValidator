namespace MetaValidator.Constraints {
    using System;

    public class StaticType : TypeConstraint {
        public readonly static IMetaConstraint<Type> Instance = new StaticType();
        protected StaticType() { }
        //
        protected override bool MatchCore(Type type) {
            return type.IsSealed && type.IsAbstract;
        }
    }
}