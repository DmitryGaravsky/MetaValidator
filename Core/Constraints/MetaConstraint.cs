namespace MetaValidator.Constraints {
    public abstract class MetaConstraint<T> : Core.Specification<T>, IMetaConstraint<T> {
        #region Default
        public readonly static IMetaConstraint<T> True = Default.AlwaysTrue;
        public readonly static IMetaConstraint<T> False = Default.AlwaysFalse;
        sealed class Default : MetaConstraint<T> {
            Default(bool result) { this.result = result; }
            internal readonly static IMetaConstraint<T> AlwaysTrue = new Default(true);
            internal readonly static IMetaConstraint<T> AlwaysFalse = new Default(false);
            readonly bool result;
            protected sealed override bool MatchCore(T meta) { return result; }
        }
        #endregion Default
    }
}