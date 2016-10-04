namespace MetaValidator.Constraints {
    using System.Reflection;

    public abstract class MemberConstraint<TMember> : MetaConstraint<TMember>
        where TMember : MemberInfo {
    }
}