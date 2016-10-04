namespace MetaValidator.Core {
    using System.Linq;

    public static partial class MetaSpecificationExtension {
        public static ISpecification<T> Not<T>(this ISpecification<T> constraint) {
            return Specification<T>.Not(constraint);
        }
        public static ISpecification<T> Xor<T>(this ISpecification<T> constraint1, ISpecification<T> constraint2) {
            return Specification<T>.Xor(constraint1, constraint2);
        }
        //
        public static ISpecification<T> And<T>(this ISpecification<T> constraint,
            params ISpecification<T>[] constraints) {
            return Specification<T>.And(constraint.Yeild().Concat(constraints.NotNull()));
        }
        public static ISpecification<T> Or<T>(this ISpecification<T> constraint,
            params ISpecification<T>[] constraints) {
            return Specification<T>.Or(constraint.Yeild().Concat(constraints.NotNull()));
        }
        //
        public static ISpecification<T> AndNot<T>(this ISpecification<T> constraint,
            params ISpecification<T>[] constraints) {
            return And(constraint, Specification<T>.And(constraints).Not());
        }
        public static ISpecification<T> OrNot<T>(this ISpecification<T> constraint,
            params ISpecification<T>[] constraints) {
            return Or(constraint, Specification<T>.And(constraints).Not());
        }
    }
}