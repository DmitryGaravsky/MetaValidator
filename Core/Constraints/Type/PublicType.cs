namespace MetaValidator.Constraints {
    using System;

    public class PublicType : TypeConstraint {
        public readonly static IMetaConstraint<Type> Instance = new PublicType();
        protected PublicType() { }
        //
        protected override bool MatchCore(Type type) { 
            return type.IsPublic; 
        }
    }
}