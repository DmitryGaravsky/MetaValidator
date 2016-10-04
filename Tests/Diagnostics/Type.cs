namespace MetaValidator.Tests {
    using MetaValidator.Constraints;
    using NUnit.Framework;
    using MetaValidator.Diagnostics;
    using MetaValidator.Core;

    [TestFixture]
    public class TypeDiagnosticTests : ConfigurationDependentTest {
        static readonly IDiagnostic shouldNotBePublic = new Diagnostics.Diagnostic(errorSpec: PublicType.Instance);
        static readonly IDiagnostic shouldBeStatic = new Diagnostics.Diagnostic(errorSpec: StaticType.Instance.Not());
        //
        [Test]
        public void Test00_PublicType() {
            var factory = cfg.Resolve<IDiagnosticContextFactory>();
            IDiagnosticContext ctx1 = factory.Create(type: typeof(Foo)); //error: public
            Assert.IsFalse(shouldNotBePublic.Validate(ctx1));
            Assert.IsTrue(ctx1.Result.HasErrors);

            IDiagnosticContext ctx2 = factory.Create(type: typeof(Bar));
            Assert.IsTrue(shouldNotBePublic.Validate(ctx2));
            Assert.IsFalse(ctx2.Result.HasErrors);
        }
        [Test]
        public void Test01_StaticType() {
            var factory = cfg.Resolve<IDiagnosticContextFactory>();
            IDiagnosticContext ctx1 = factory.Create(type: typeof(Foo)); // error: non-static
            Assert.IsFalse(shouldBeStatic.Validate(ctx1));
            Assert.IsTrue(ctx1.Result.HasErrors);

            IDiagnosticContext ctx2 = factory.Create(type: typeof(FooBar));
            Assert.IsTrue(shouldBeStatic.Validate(ctx2));
            Assert.IsFalse(ctx2.Result.HasErrors);
        }
    }
}