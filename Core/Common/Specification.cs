namespace MetaValidator.Core {
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public abstract class Specification<T> : ISpecification<T> {
        bool ISpecification<T>.Match(T meta) {
            return !object.ReferenceEquals(meta, null)
                && MatchCore(meta);
        }
        protected abstract bool MatchCore(T type);
        #region Operations
        sealed class Invert : Specification<T> {
            readonly Func<T, bool> match;
            internal Invert(ISpecification<T> constraint) {
                this.match = (constraint != null) ?
                    meta => !constraint.Match(meta) : new Func<T, bool>(meta => false);
            }
            protected sealed override bool MatchCore(T meta) { return match(meta); }
        }
        sealed class CombineXor : Specification<T> {
            readonly Func<T, bool> match;
            internal CombineXor(ISpecification<T> constraint1, ISpecification<T> constraint2) {
                this.match = meta => false;
                if(constraint1 != constraint2) {
                    if(constraint1 == null || constraint2 == null)
                        this.match = meta => (constraint1 ?? constraint2).Match(meta);
                    else
                        this.match = meta => constraint1.Match(meta) ^ constraint2.Match(meta);
                }
            }
            protected sealed override bool MatchCore(T meta) { return match(meta); }
        }
        sealed class CombineAnd : Specification<T> {
            readonly Func<T, bool> match;
            internal CombineAnd(IEnumerable<ISpecification<T>> children) {
                this.match = meta => children.All(x => x.Match(meta));
            }
            protected sealed override bool MatchCore(T meta) { return match(meta); }
        }
        sealed class CombineOr : Specification<T> {
            readonly Func<T, bool> match;
            internal CombineOr(IEnumerable<ISpecification<T>> children) {
                this.match = meta => children.Any(x => x.Match(meta));
            }
            protected sealed override bool MatchCore(T meta) { return match(meta); }
        }
        #endregion Operations
        #region Operators
        internal static ISpecification<T> Not(ISpecification<T> constraint) {
            return new Invert(constraint);
        }
        internal static ISpecification<T> Xor(ISpecification<T> constraint1, ISpecification<T> constraint2) {
            return new CombineXor(constraint1, constraint2);
        }
        internal static ISpecification<T> And(IEnumerable<ISpecification<T>> constraints) {
            return new CombineAnd(constraints.NotNull());
        }
        internal static ISpecification<T> Or(IEnumerable<ISpecification<T>> constraints) {
            return new CombineOr(constraints.NotNull());
        }
        #endregion Operators
        internal readonly static IEnumerable<ISpecification<T>> Empty = Enumerable.Empty<ISpecification<T>>();
    }
}