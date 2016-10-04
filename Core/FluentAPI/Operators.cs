namespace MetaValidator.Core {
    using System.Diagnostics;

    public static partial class MetaSpecificationExtension {
        [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
        public static OperatorAware<T> Wrap<T>(this ISpecification<T> @this) {
            return @this as OperatorAware<T> ?? new OperatorAware<T>(@this);
        }
        [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
        public static ISpecification<T> Unwrap<T>(this OperatorAware<T> @this) {
            return @this.@Get(x => x.specification);
        }
        //
        public sealed class OperatorAware<T> : ISpecification<T> {
            internal readonly ISpecification<T> specification;
            internal OperatorAware(ISpecification<T> specification) {
                this.specification = (specification as OperatorAware<T>)
                    .@Get(x => x.specification, specification);
            }
            bool ISpecification<T>.Match(T metadata) {
                return specification
                    .@Get(x => x.Match(metadata));
            }
            // Operators
            [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
            public static OperatorAware<T> operator !(OperatorAware<T> specification) {
                return Wrap(specification.Unwrap().Not());
            }
            [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
            public static OperatorAware<T> operator &(OperatorAware<T> s1, OperatorAware<T> s2) {
                return Wrap(s1.Unwrap().And(s2.Unwrap()));
            }
            [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
            public static OperatorAware<T> operator |(OperatorAware<T> s1, OperatorAware<T> s2) {
                return Wrap(s1.Unwrap().Or(s2.Unwrap()));
            }
            [DebuggerStepThrough, System.Runtime.CompilerServices.MethodImpl(256)]
            public static OperatorAware<T> operator ^(OperatorAware<T> s1, OperatorAware<T> s2) {
                return Wrap(s1.Unwrap().Xor(s2.Unwrap()));
            }
        }
    }
}