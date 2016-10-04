namespace MetaValidator.Tests {
    using MetaValidator.Constraints;
    using NUnit.Framework;
    using MetaValidator.Core;
    using MetaValidator.Diagnostics;
    using MetaValidator.Diagnostics.FluentAPI;
    using MetaValidator.Constraints.FluentAPI;
    using System;
    using System.Linq;

    [TestFixture]
    public class ConfigurationTests : ConfigurationDependentTest {
        class TypeShouldNotBePublic : Diagnostic {
            public TypeShouldNotBePublic() : base(errorSpec: PublicType.Instance) { }
        }
        [Test]
        public void Test00_Smoke() {
            var ctx = cfg.Resolve<IDiagnosticContextFactory>().Create(type: typeof(Bar));
            var diagnostics = cfg.Resolve(ctx);
            Assert.IsFalse(diagnostics.Any());
            Assert.IsFalse(ctx.Result.HasErrors);
        }
        [Test]
        public void Test01_RegisterAndResolve_TypeShouldNotBePublic() {
            var spec = StaticType.Instance.OrNot(PublicType.Instance);
            cfg.Register(() => new TypeShouldNotBePublic());
            cfg.Register<Type, TypeShouldNotBePublic>(spec);
            DoTests();
        }
        [Test]
        public void Test02_RegisterAndResolve_TypeShouldNotBePublic_Fluent() {
            var spec = StaticType.Instance.OrNot(PublicType.Instance);
            cfg.Register(spec, new TypeShouldNotBePublic());
            DoTests();
        }
        [Test]
        public void Test02_RegisterAndResolve_TypeShouldNotBePublic_Fluent2() {
            cfg.For<Type>(x => x.IsStatic() | !x.IsPublic())
                .Register(new TypeShouldNotBePublic());
            DoTests();
        }
        void DoTests() {
            AnalyzeNonStaticAndPublicType();
            AnalyzeStaticType();
        }
        void AnalyzeNonStaticAndPublicType() {
            var ctx = cfg.Resolve<IDiagnosticContextFactory>().Create(type: typeof(Foo)); // ignore: non-static
            var diagnostics = cfg.Resolve(ctx); 
            Assert.IsFalse(diagnostics.Any());
        }
        void AnalyzeStaticType() {
            var ctx = cfg.Resolve<IDiagnosticContextFactory>().Create(type: typeof(FooBar));
            var diagnostics = cfg.Resolve(ctx);
            Assert.AreEqual(1, diagnostics.Count());
            Assert.IsTrue(diagnostics.ElementAt(0) is TypeShouldNotBePublic);
            var instance = cfg.Resolve(ctx).ElementAt(0);
            Assert.AreEqual(diagnostics.ElementAt(0), instance);
        }
    }
}