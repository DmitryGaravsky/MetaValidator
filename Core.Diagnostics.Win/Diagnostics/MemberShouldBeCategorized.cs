namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class MemberShouldBeCategorized : Diagnostic {
        public MemberShouldBeCategorized() :
            base(errorSpec: MemberHasAttribute<CategoryAttribute>.Instance.Not()) {
        }
    }
}