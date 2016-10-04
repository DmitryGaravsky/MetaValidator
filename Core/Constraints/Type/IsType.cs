namespace MetaValidator.Constraints {
    using System;

    public class IsType<T> : TypeConstraint {
        public readonly static IMetaConstraint<Type> Instance = new IsType<T>();
        protected IsType() { }
        //
        protected override bool MatchCore(Type type) {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}