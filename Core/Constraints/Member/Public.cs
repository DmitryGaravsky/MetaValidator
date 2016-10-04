namespace MetaValidator.Constraints {
    using System;
    using System.Reflection;

    public class PublicMember<TMember> : MemberConstraint<TMember>
        where TMember : MemberInfo {
        readonly Func<TMember, bool> isPublic;
        protected PublicMember(Func<TMember, bool> isPublic) {
            this.isPublic = isPublic;
        }
        protected sealed override bool MatchCore(TMember member) {
            return isPublic(member);
        }
    }
    //
    public class PublicField : PublicMember<FieldInfo> {
        public readonly static IMetaConstraint<FieldInfo> Instance = new PublicField();
        protected PublicField() : base(f => f.IsPublic) { }
    }
    public class PublicMethod : PublicMember<MethodInfo> {
        public readonly static IMetaConstraint<MethodInfo> Instance = new PublicMethod();
        protected PublicMethod() : base(m => m.IsPublic) { }
    }
    public class PublicConstructor : PublicMember<ConstructorInfo> {
        public readonly static IMetaConstraint<ConstructorInfo> Instance = new PublicConstructor();
        protected PublicConstructor() : base(c => c.IsPublic) { }
    }
    public class PublicProperty : PublicMember<PropertyInfo> {
        public readonly static IMetaConstraint<PropertyInfo> Instance = new PublicProperty();
        protected PublicProperty()
            : base(p => p.GetGetMethod() != null || p.GetSetMethod() != null) {
        }
    }
    public class PublicEvent : PublicMember<EventInfo> {
        public readonly static IMetaConstraint<EventInfo> Instance = new PublicEvent();
        protected PublicEvent()
            : base(e => e.GetAddMethod() != null || e.GetRemoveMethod() != null) {
        }
    }
}