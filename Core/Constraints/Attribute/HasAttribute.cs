namespace MetaValidator.Constraints {
    using System;
    using System.Reflection;

    public class HasAttribute<T, TAttribute> : MetaConstraint<T>
        where TAttribute : Attribute {
        readonly Func<T, bool> match;
        protected HasAttribute(Func<T, Type, bool> isDefined) {
            match = x => isDefined(x, typeof(TAttribute));
        }
        protected sealed override bool MatchCore(T data) {
            return match(data);
        }
    }
    //
    public class AssemblyHasAttribute<TAttribute> : HasAttribute<Assembly, TAttribute>
        where TAttribute : Attribute {
        public readonly static IMetaConstraint<Assembly> Instance = new AssemblyHasAttribute<TAttribute>();
        protected AssemblyHasAttribute()
            : base((a, type) => Attribute.IsDefined(a, type)) {
        }
    }
    public class TypeHasAttribute<TAttribute> : HasAttribute<Type, TAttribute>
        where TAttribute : Attribute {
        public readonly static IMetaConstraint<Type> Instance = new TypeHasAttribute<TAttribute>();
        protected TypeHasAttribute()
            : base((t, type) => Attribute.IsDefined(t, type)) {
        }
    }
    public class MemberHasAttribute<TMember, TAttribute> : HasAttribute<TMember, TAttribute>
        where TMember : MemberInfo
        where TAttribute : Attribute {
        public readonly static IMetaConstraint<TMember> Instance = new MemberHasAttribute<TMember, TAttribute>();
        protected MemberHasAttribute()
            : base((m, type) => Attribute.IsDefined(m, type)) {
        }
    }
    public class MemberHasAttribute<TAttribute> : MemberHasAttribute<MemberInfo, TAttribute>
        where TAttribute : Attribute {
        protected MemberHasAttribute() : base() { }
    }
}