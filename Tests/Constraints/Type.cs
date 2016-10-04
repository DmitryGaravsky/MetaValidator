namespace MetaValidator.Tests {
    using MetaValidator.Core;
    using MetaValidator.Constraints;
    using MetaValidator.Constraints.FluentAPI;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TypeConstraintTests {
        [Test]
        public void Test00_PublicType() {
            Assert.IsTrue(PublicType.Instance.Match(typeof(Foo)));
            Assert.IsFalse(PublicType.Instance.Match(typeof(Bar)));
        }
        [Test]
        public void Test01_StaticType() {
            Assert.IsFalse(StaticType.Instance.Match(typeof(Foo)));
            Assert.IsTrue(StaticType.Instance.Match(typeof(FooBar)));
        }
        [Test]
        public void Test02_PublicAndStaticType() {
            var c = TypeConstraint.True
                .And(PublicType.Instance)
                .And(StaticType.Instance);
            Assert.IsTrue(c.Match(typeof(FooBar)));
        }
        [Test]
        public void Test03_IsType() {
            Assert.IsFalse(IsType<Bar>.Instance.Match(typeof(Foo)));
            Assert.IsTrue(IsType<object>.Instance.Match(typeof(Foo)));
        }
        [Test]
        public void Test04_HasAttribute() {
            Assert.IsFalse(TypeHasAttribute<System.ComponentModel.CategoryAttribute>.Instance.Match(typeof(Bar)));
            Assert.IsTrue(TypeHasAttribute<System.ComponentModel.CategoryAttribute>.Instance.Match(typeof(Foo)));
        }
        [Test]
        public void Test03_FluentAPi() {
            Func<IMetaConstraint<Type>, ISpecification<Type>> c =
                x =>(
                    x.IsPublic() & x.IsStatic() &
                    (x.Is<object>() ^ x.Is<ValueType>()) &
                    !x.HasAttribute<System.ComponentModel.CategoryAttribute>()).Unwrap();
            Assert.IsTrue(c(null).Match(typeof(FooBar)));
        }
    }
}