namespace MetaValidator.Tests {
    using System.Reflection;
    using MetaValidator.Constraints;
    using NUnit.Framework;
    using BF = System.Reflection.BindingFlags;

    [TestFixture]
    public class AttributeRulesTests {
        [Test]
        public void Test00_AssemblyHasAttribute() {
            var rule = AssemblyHasAttribute<AssemblyTitleAttribute>.Instance;
            Assert.IsTrue(rule.Match(typeof(Foo).Assembly));
        }
        [Test]
        public void Test01_TypeHasAttribute() {
            var rule = TypeHasAttribute<System.ComponentModel.CategoryAttribute>.Instance;
            Assert.IsTrue(rule.Match(typeof(Foo)));
            Assert.IsFalse(rule.Match(typeof(Bar)));
        }
        [Test]
        public void Test02_MemberHasAttribute() {
            var rule = MemberHasAttribute<System.ComponentModel.CategoryAttribute>.Instance;
            Assert.IsFalse(rule.Match(typeof(FooBar).GetProperty("Foo")));
            Assert.IsTrue(rule.Match(typeof(FooBar).GetProperty("Bar", BF.NonPublic | BF.Static)));
        }
    }
}