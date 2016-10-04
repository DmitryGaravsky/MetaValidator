namespace MetaValidator.Constraints.FluentAPI {
    using System;
    using System.Reflection;
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Reflection.PropertyInfo>;

    public static class PropertyConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<PropertyInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicProperty.Instance);
        }
        public static OperatorAware IsReadOnly(this IMetaConstraint<PropertyInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(ReadOnlyProperty.Instance);
        }
        public static OperatorAware HasAttribute<TAttribute>(this IMetaConstraint<PropertyInfo> @this)
            where TAttribute : Attribute {
            return Core.MetaSpecificationExtension.Wrap(MemberHasAttribute<PropertyInfo, TAttribute>.Instance);
        }
    }
}

namespace MetaValidator.Constraints.FluentAPI {
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Reflection.FieldInfo>;

    public static class FieldConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<System.Reflection.FieldInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicField.Instance);
        }
    }
}

namespace MetaValidator.Constraints.FluentAPI {
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Reflection.MethodInfo>;

    public static class MethodConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<System.Reflection.MethodInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicMethod.Instance);
        }
    }
}

namespace MetaValidator.Constraints.FluentAPI {
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Reflection.ConstructorInfo>;

    public static class ConstructorConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<System.Reflection.ConstructorInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicConstructor.Instance);
        }
    }
}

namespace MetaValidator.Constraints.FluentAPI {
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Reflection.EventInfo>;

    public static class EventConstraintExtension {
        public static OperatorAware IsPublic(this IMetaConstraint<System.Reflection.EventInfo> @this) {
            return Core.MetaSpecificationExtension.Wrap(PublicEvent.Instance);
        }
    }
}