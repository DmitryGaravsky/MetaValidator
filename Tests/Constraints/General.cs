namespace MetaValidator.Tests {
    using MetaValidator.Core;
    using MetaValidator.Constraints;
    using NUnit.Framework;

    [TestFixture]
    public class GeneralTests_Constraints {
        static readonly IMetaConstraint<object> cTrue = MetaConstraint<object>.True;
        static readonly IMetaConstraint<object> cFalse = MetaConstraint<object>.False;
        static readonly object obj = new object();
        [Test]
        public void Test00_Default() {
            Assert.IsTrue(cTrue.Match(obj));
            Assert.IsFalse(cFalse.Match(obj));
        }
        [Test]
        public void Test01_Invert() {
            Assert.IsFalse(cTrue.Not().Match(obj));
            Assert.IsTrue(cFalse.Not().Match(obj));
        }
        [Test]
        public void Test02_Combine() {
            Assert.IsFalse(cTrue.And(cFalse).Match(obj));
            Assert.IsTrue(cTrue.Or(cFalse).Match(obj));
            Assert.IsFalse(cTrue.Xor(cTrue).Match(obj));
            Assert.IsTrue(cTrue.Xor(cFalse).Match(obj));
            Assert.IsTrue(cTrue.AndNot(cFalse).Match(obj));
            Assert.IsTrue(cFalse.OrNot(cFalse).Match(obj));
        }
        public void Test03_FluentApi_WrapUnwrap() {
            var wrapped1 = TypeConstraint.True.Wrap();
            var wrapped2 = TypeConstraint.True.Wrap();
            Assert.IsTrue(!object.ReferenceEquals(wrapped1, wrapped2));
            Assert.IsTrue(object.ReferenceEquals(wrapped1, wrapped1.Wrap()));
            Assert.IsTrue(object.ReferenceEquals(wrapped1.Unwrap(), wrapped2.Unwrap()));
        }
    }
}