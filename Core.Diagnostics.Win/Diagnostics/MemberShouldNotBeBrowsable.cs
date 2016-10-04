namespace MetaValidator.Diagnostics.Win {
    using System.ComponentModel;
    using MetaValidator.Core;
    using MetaValidator.Constraints;

    public class MemberShouldNotBeBrowsable : Diagnostic {
        public MemberShouldNotBeBrowsable() :
            base(errorSpec: MemberHasAttribute<BrowsableAttribute>.Instance.Not()) {
        }
    }
}