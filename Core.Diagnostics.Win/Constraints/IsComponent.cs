namespace MetaValidator.Constraints.Win {
    using System;
    using MetaValidator.Constraints;

    public class IsComponent : TypeConstraint {
        public readonly static IMetaConstraint<Type> Instance = new IsComponent();
        protected IsComponent() { }
        //
        protected override bool MatchCore(Type type) {
            return typeof(System.ComponentModel.IComponent).IsAssignableFrom(type);
        }
    }
}

namespace MetaValidator.Constraints.Win.FluentAPI {
    using OperatorAware = Core.MetaSpecificationExtension.OperatorAware<System.Type>;

    public static class TypeConstraintExtension {
        public static OperatorAware IsComponent(this IMetaConstraint<System.Type> @this) {
            return Core.MetaSpecificationExtension.Wrap(MetaValidator.Constraints.Win.IsComponent.Instance);
        }
    }
}