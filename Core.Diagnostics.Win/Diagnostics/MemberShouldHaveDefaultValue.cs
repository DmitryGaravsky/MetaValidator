namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class MemberShouldHaveDefaultValue : Diagnostic {
        public MemberShouldHaveDefaultValue() :
            base(errorSpec: MemberHasAttribute<DefaultValueAttribute>.Instance.Not()) {
        }
    }
}