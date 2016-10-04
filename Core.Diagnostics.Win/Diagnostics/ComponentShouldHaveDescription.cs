namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class ComponentShouldHaveDescription : Diagnostic {
        public ComponentShouldHaveDescription() :
            base(errorSpec: TypeHasAttribute<DescriptionAttribute>.Instance.Not()) {
        }
    }
}