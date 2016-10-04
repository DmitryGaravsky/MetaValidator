namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class MemberShouldHaveDescription : Diagnostic {
        public MemberShouldHaveDescription() :
            base(errorSpec: MemberHasAttribute<DescriptionAttribute>.Instance.Not()) {
        }
    }
}