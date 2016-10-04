namespace MetaValidator.Core {
    using System.Collections.Generic;

    public static partial class MetaSpecificationExtension {
        internal static IEnumerable<ISpecification<T>> Yeild<T>(
            this ISpecification<T> constraint) {
            return (constraint != null) ?
                new ISpecification<T>[] { constraint } : Specification<T>.Empty;
        }
        internal static IEnumerable<ISpecification<T>> NotNull<T>(
            this IEnumerable<ISpecification<T>> constraints) {
            return constraints ?? Specification<T>.Empty;
        }
    }
}