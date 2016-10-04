namespace MetaValidator.Constraints.FluentAPI {
    using System;
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Type>;

    public static class TypeConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<System.Type> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicType.Instance);
        }
        public static OperatorAware IsStatic(this IMetaConstraint<System.Type> @this) {
            return Core.MetaSpecificationExtension.Wrap(StaticType.Instance);
        }
        public static OperatorAware Is<T>(this IMetaConstraint<System.Type> @this) {
            return Core.MetaSpecificationExtension.Wrap(IsType<T>.Instance);
        }
        public static OperatorAware HasAttribute<TAttribute>(this IMetaConstraint<System.Type> @this)
            where TAttribute : Attribute {
            return Core.MetaSpecificationExtension.Wrap(TypeHasAttribute<TAttribute>.Instance);
        }
    }
}