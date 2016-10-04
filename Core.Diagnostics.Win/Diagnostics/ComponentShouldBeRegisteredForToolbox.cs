namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class ComponentShouldBeRegisteredAsToolboxItem : Diagnostic {
        public ComponentShouldBeRegisteredAsToolboxItem() :
            base(errorSpec: TypeHasAttribute<ToolboxItemAttribute>.Instance.Not()) {
        }
    }
}