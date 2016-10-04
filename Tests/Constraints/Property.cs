namespace MetaValidator.Tests {
    using System.Reflection;
    using MetaValidator.Core;
    using MetaValidator.Constraints;
    using MetaValidator.Constraints.FluentAPI;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class PropertyConstraintTests {
        readonly static PropertyInfo pFoo = typeof(FooBar).GetProperty("Foo");
        readonly static PropertyInfo pBar = typeof(FooBar).GetProperty("Bar", BindingFlags.Static | BindingFlags.NonPublic);
        [Test]
        public void Test00_PublicProperty() {
            Assert.IsTrue(PublicProperty.Instance.Match(pFoo));
            Assert.IsFalse(PublicProperty.Instance.Match(pBar));
        }
        [Test]
        public void Test01_ReadonlyProperty() {
            Assert.IsFalse(ReadOnlyProperty.Instance.Match(pFoo));
            Assert.IsTrue(ReadOnlyProperty.Instance.Match(pBar));
        }
        [Test]
        public void Test02_PublicAndNotReadOnlyProperty() {
            var c = MetaConstraint<PropertyInfo>.True
                .And(PublicProperty.Instance)
                .AndNot(ReadOnlyProperty.Instance);
            Assert.IsTrue(c.Match(pFoo));
        }
        [Test]
        public void Test03_FluentAPi() {
            Func<IMetaConstraint<PropertyInfo>, ISpecification<PropertyInfo>> c =
                x => (x.IsPublic() & !x.IsReadOnly()).Unwrap();
            Assert.IsTrue(c(null).Match(pFoo));
        }
    }
}