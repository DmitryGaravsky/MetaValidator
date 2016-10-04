namespace MetaValidator.Tests {
    using MetaValidator.Constraints;
    using NUnit.Framework;

    [TestFixture]
    public class AssemblyConstraintTests {
        [Test]
        public void Test00_NonDesignAssembly() {
            Assert.IsFalse(DesignAssembly.Instance.Match(typeof(Foo).Assembly));
        }
    }
}